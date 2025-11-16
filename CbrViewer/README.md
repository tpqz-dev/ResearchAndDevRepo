# CBR Comic Book Reader

A simple WPF application for viewing comic book archives (.cbr files).

## Features

- **File Selection**: Open and load .cbr (comic book archive) files
- **Image Display**: View comic book pages one at a time
- **Navigation**: Navigate through pages using Previous/Next buttons
- **Page Counter**: Shows current page number and total pages
- **Full Image Support**: Supports common image formats (JPG, PNG, GIF, BMP, WebP)
- **Zoom & Scroll**: Images scale to fit the window, with scrolling for larger images

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- Windows operating system

### Building the Application

```bash
cd CbrViewer
dotnet build
```

### Running the Application

```bash
cd CbrViewer
dotnet run
```

Or run from the solution:

```bash
dotnet run --project CbrViewer/CbrViewer.csproj
```

## Usage

1. Launch the application
2. Click **File → Open CBR File...** from the menu
3. Select a .cbr file from your file system
4. Use the **Previous** and **Next** buttons to navigate between pages
5. The page counter shows your current position (e.g., "Page 1 of 25")

## Supported File Formats

- **.cbr** - Comic Book Archive (RAR format)

The application extracts and displays images from the archive in alphabetical order.

## Technical Details

- Built with WPF (Windows Presentation Foundation)
- Uses SharpCompress library for RAR archive extraction
- Images are loaded into memory for fast navigation
- Supports multiple image formats within the archive

## Dependencies

- [SharpCompress](https://github.com/adamhathcock/sharpcompress) - For RAR archive handling
