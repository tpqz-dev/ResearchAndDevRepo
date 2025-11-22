# ResearchAndDevRepo

This repository contains Windows desktop applications built with C# and WPF.

## Projects

### Image Board

A desktop application for visualizing, organizing, and manipulating images on an infinite workspace.

**Features:**
- Drag & Drop image import
- Infinite canvas with zoom and pan
- Image manipulation (move, resize, flip)
- State persistence between sessions
- Context menus and keyboard shortcuts

**Quick Start:**
```bash
cd ImageBoard
dotnet build
dotnet run
```

See [ImageBoard/README.md](ImageBoard/README.md) for detailed documentation.

**Requirements:**
- .NET 8.0 SDK or later
- Windows operating system

### File Organizer

A C# WPF application for automatic file organization based on file extensions.

**Features:**
- **Directory Selection**: Choose a target directory to organize
- **Automatic Organization**: Files are automatically moved to folders based on their extensions
- **Scheduled Monitoring**: Uses Quartz.NET to monitor the directory and reorganize files at configurable intervals (default: 5 minutes)
- **Configurable**: Adjust monitoring interval and default settings via configuration file
- **Real-time Logging**: View all operations in a status log within the application

**Quick Start:**
```bash
cd FileOrganizer
dotnet build
dotnet run
```

See [FileOrganizer/README.md](FileOrganizer/README.md) for detailed documentation.

**Requirements:**
- .NET 9.0 SDK or later
- Windows operating system

## Building All Projects

From the repository root:
```bash
dotnet build
```

This will build both projects in the solution.
