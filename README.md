# ResearchAndDevRepo

## File Organizer

A C# WPF application for automatic file organization based on file extensions.

### Features

- **Directory Selection**: Choose a target directory to organize
- **Automatic Organization**: Files are automatically moved to folders based on their extensions
- **Scheduled Monitoring**: Uses Quartz.NET to monitor the directory and reorganize files at configurable intervals (default: 5 minutes)
- **Configurable**: Adjust monitoring interval and default settings via configuration file
- **Real-time Logging**: View all operations in a status log within the application

### Getting Started

1. Open the solution in Visual Studio 2022 or later
2. Build the solution (F6)
3. Run the application (F5)

Or using the command line:
```bash
cd FileOrganizer
dotnet build
dotnet run
```

### Documentation

See [FileOrganizer/README.md](FileOrganizer/README.md) for detailed documentation.

### Requirements

- .NET 9.0 SDK or later
- Windows operating system (or cross-platform build with Windows targeting enabled)
