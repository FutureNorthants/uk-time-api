# UK Time API - CHATBOT

A simple AWS Lambda function that returns the current UK time with timezone information.

## Features

- Returns current UK time in GMT/BST
- Automatically detects daylight saving time
- Cross-platform timezone handling (Windows/Linux)
- JSON API response with CORS headers

## API Response

```json
{
  "currentTime": "2024-01-15 14:30:45",
  "timeZone": "GMT (Greenwich Mean Time)",
  "isDaylightSaving": false,
  "message": "Current UK time retrieved successfully"
}
```

## Quick Start

### Prerequisites
- .NET 8.0 SDK
- AWS CLI configured
- Amazon.Lambda.Tools

### Build & Test
```bash
dotnet build
dotnet test
```

### Deploy to AWS Lambda
```bash
cd src/UKTimeAPI
dotnet lambda deploy-function
```

## Project Structure

```
├── src/UKTimeAPI/           # Lambda function source
│   ├── Function.cs          # Main handler
│   └── Models/              # Response models
├── test/UKTimeAPI.Tests/    # Unit tests
└── README.md
```

## License

MIT
