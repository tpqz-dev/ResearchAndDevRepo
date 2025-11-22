# Image Board - WPF Application

A desktop application for visualizing, organizing, and manipulating images on an infinite workspace.

## Features

- **Infinite Canvas**: Work with images on a limitless workspace
- **Drag & Drop**: Import images by dragging files from Windows Explorer
- **Zoom & Pan**: 
  - Zoom with mouse wheel (centered on cursor)
  - Pan with middle mouse button or Space + Left Click
- **Image Manipulation**:
  - Move images by dragging
  - Resize images
  - Flip horizontal/vertical
  - Bring to front/Send to back
  - Reset to original size
- **Persistence**: Application state (window position, images, view) is automatically saved and restored
- **Context Menus**:
  - Right-click on canvas for global commands
  - Right-click on images for image-specific commands
- **Always on Top**: Toggle window to stay on top of other windows

## Technical Details

- **Framework**: .NET 8.0 (LTS)
- **UI**: WPF (Windows Presentation Foundation)
- **Pattern**: MVVM (Model-View-ViewModel)
- **Dependencies**:
  - CommunityToolkit.Mvvm - MVVM helpers
  - System.Text.Json - State serialization (built-in)

## Getting Started

### Building

```bash
cd ImageBoard
dotnet build
```

### Running

```bash
cd ImageBoard
dotnet run
```

Or build and run from the solution:

```bash
dotnet build
dotnet run --project ImageBoard/ImageBoard.csproj
```

## Usage

1. **Add Images**: 
   - Drag and drop image files onto the application window
   - Right-click on canvas and select "Add Images..."
   
2. **Navigate**:
   - Zoom: Scroll mouse wheel
   - Pan: Middle click and drag, or Space + Left Click and drag

3. **Manipulate Images**:
   - Select: Left click on an image
   - Move: Drag selected image
   - Right-click on image for more options

4. **State Persistence**:
   - The application automatically saves its state when closed
   - On next launch, all images and view settings are restored
   - State is saved in `appstate.json` next to the executable
   - Images are stored in `ProjectImages` folder

## Project Structure

```
ImageBoard/
├── Models/               # Data models
│   ├── ImageItem.cs     # Image data
│   └── BoardState.cs    # Application state
├── ViewModels/          # MVVM ViewModels
│   ├── ImageItemViewModel.cs
│   └── MainViewModel.cs
├── Views/               # XAML Views
│   ├── MainWindow.xaml
│   └── MainWindow.xaml.cs
├── Helpers/             # Utility classes
│   ├── Converters.cs    # XAML converters
│   └── ZoomPanBehavior.cs
├── App.xaml
└── ImageBoard.csproj
```

## Supported Image Formats

- JPEG (.jpg, .jpeg)
- PNG (.png)
- BMP (.bmp)
- GIF (.gif)
- WEBP (.webp)
- TIFF (.tiff, .tif)

## Keyboard Shortcuts

(To be implemented in future versions)
- Ctrl+C: Copy selected image
- Ctrl+V: Paste image from clipboard
- Delete: Remove selected image

## Requirements

- Windows operating system
- .NET 8.0 Runtime or SDK
