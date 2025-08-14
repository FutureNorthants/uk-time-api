using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System;
using System.Collections.Generic;
using System.Text.Json;
using UKTimeAPI.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace UKTimeAPI;

public class Function
{
    /// <summary>
    /// Lambda function handler that returns current UK time
    /// </summary>
    /// <param name="request">API Gateway proxy request</param>
    /// <param name="context">Lambda context</param>
    /// <returns>API Gateway proxy response</returns>
    public APIGatewayProxyResponse FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        try
        {
            context.Logger.LogInformation("Processing UK Time API request");

            // Get UK time zone info (works on both Windows and Linux)
            TimeZoneInfo ukTimeZone;
            try
            {
                ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");
            }
            catch
            {
                ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            }
            DateTime ukTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ukTimeZone);
            bool isDaylightSaving = ukTimeZone.IsDaylightSavingTime(ukTime);

            // Create response
            var timeResponse = new TimeResponse
            {
                CurrentTime = ukTime.ToString("yyyy-MM-dd HH:mm:ss"),
                TimeZone = isDaylightSaving ? "BST (British Summer Time)" : "GMT (Greenwich Mean Time)",
                IsDaylightSaving = isDaylightSaving,
                Message = $"Current UK time retrieved successfully"
            };

            var response = new APIGatewayProxyResponse
            {
                StatusCode = 200,
                Body = JsonSerializer.Serialize(timeResponse, new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                }),
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Methods", "GET, POST, OPTIONS" },
                    { "Access-Control-Allow-Headers", "Content-Type, X-API-Key" }
                }
            };

            context.Logger.LogInformation($"Successfully returned UK time: {ukTime}");
            return response;
        }
        catch (Exception ex)
        {
            context.Logger.LogError($"Error processing request: {ex.Message}");

            var errorResponse = new ErrorResponse
            {
                Error = "Internal server error occurred while processing the request",
                StatusCode = 500
            };

            return new APIGatewayProxyResponse
            {
                StatusCode = 500,
                Body = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions 
                { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                }),
                Headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" },
                    { "Access-Control-Allow-Origin", "*" }
                }
            };
        }
    }
}