# File Organizer

A C# WPF application for automatic file organization based on file extensions.

## Features

- **Directory Selection**: Choose a target directory to organize
- **Automatic Organization**: Files are automatically moved to folders based on their extensions
- **Scheduled Monitoring**: Uses Quartz.NET to monitor the directory and reorganize files at configurable intervals
- **Configurable Interval**: Set the monitoring interval (default: 5 minutes)
- **Real-time Logging**: View all operations in a status log
- **Manual Organization**: Trigger organization manually at any time

## How It Works

1. Select a target directory using the "Browse..." button
2. Set the monitoring interval in minutes (default: 5 minutes)
3. Click "Start Organizing" to begin automatic monitoring
4. The application will:
   - Scan the target directory for files
   - Create folders named after each file extension (e.g., "txt", "pdf", "jpg")
   - Move files into their corresponding extension folders
   - Repeat this process every N minutes

## Configuration

The application can be configured using the `appsettings.json` file:

```json
{
  "FileOrganizer": {
    "DefaultIntervalMinutes": 5,
    "DefaultTargetDirectory": ""
  }
}
```

- `DefaultIntervalMinutes`: Default monitoring interval in minutes
- `DefaultTargetDirectory`: Default target directory (optional)

## Building

Requirements:
- .NET 9.0 SDK or later
- Windows operating system (or Windows targeting enabled for cross-platform builds)

Build command:
```bash
dotnet build
```

Run command:
```bash
dotnet run
```

## Dependencies

- Quartz.NET 3.13.1 - For scheduled job execution
- Microsoft.Extensions.Configuration - For configuration management
- Microsoft.Extensions.Configuration.Json - For JSON configuration support

## Project Structure

```
FileOrganizer/
├── FileOrganizer.csproj       # Project file
├── App.xaml                   # Application definition
├── App.xaml.cs                # Application code-behind
├── MainWindow.xaml            # Main window UI
├── MainWindow.xaml.cs         # Main window logic
└── appsettings.json           # Configuration file
```

## Usage Notes

- Files without extensions are not organized
- If a file with the same name already exists in the target folder, a timestamp is appended
- The application only organizes files in the root of the target directory (not subdirectories)
- Extension folders created by the application are not scanned for files

## License

This project is part of the ResearchAndDevRepo repository.
