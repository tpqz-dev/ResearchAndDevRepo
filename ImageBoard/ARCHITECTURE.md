# ImageBoard Architecture

## Overview

ImageBoard is a WPF desktop application built using the MVVM (Model-View-ViewModel) pattern. It allows users to work with images on an infinite canvas with zoom, pan, and various manipulation features.

## Architecture Pattern

### MVVM (Model-View-ViewModel)

The application follows the MVVM pattern strictly:

- **Models**: Pure data classes with no UI logic
- **ViewModels**: Business logic and state management, using CommunityToolkit.Mvvm
- **Views**: XAML-based UI with minimal code-behind

## Project Structure

```
ImageBoard/
├── Models/                  # Data models
│   ├── ImageItem.cs        # Represents an image on the board
│   └── BoardState.cs       # Application state for persistence
│
├── ViewModels/             # MVVM ViewModels
│   ├── ImageItemViewModel.cs    # ViewModel for individual images
│   └── MainViewModel.cs         # Main application ViewModel
│
├── Views/                  # XAML Views
│   ├── MainWindow.xaml          # Main application window
│   └── MainWindow.xaml.cs       # Code-behind for events
│
├── Helpers/                # Utility classes
│   ├── Converters.cs           # XAML value converters
│   ├── ZoomPanBehavior.cs      # Attached behavior for canvas zoom/pan
│   └── ImageResizeBehavior.cs  # Attached behavior for image resizing
│
├── App.xaml                # Application definition
└── ImageBoard.csproj       # Project file
```

## Key Components

### Models

#### ImageItem
Represents an image on the board with properties:
- `Id`: Unique identifier (Guid)
- `SourcePath`: Relative path to the image file
- `X, Y`: Position on canvas
- `Width, Height`: Image dimensions
- `Scale`: Zoom factor
- `IsFlippedX, IsFlippedY`: Flip transformations
- `ZIndex`: Layer ordering

#### BoardState
Complete application state for persistence:
- `Images`: Collection of ImageItems
- `WindowWidth, WindowHeight, WindowTop, WindowLeft`: Window properties
- `IsTopMost`: Always-on-top state
- `ViewTranslateX, ViewTranslateY`: Pan position
- `ViewScale`: Zoom level

### ViewModels

#### MainViewModel
Main application logic:
- Manages collection of images
- Handles image import (drag-drop and file dialog)
- Implements commands for manipulation
- Handles state persistence (save/load)
- Manages view transformation (zoom/pan)

Commands implemented:
- `AddImagesCommand`: Open file dialog to add images
- `DeleteImageCommand`: Remove an image
- `BringToFrontCommand`: Bring image to front
- `SendToBackCommand`: Send image to back
- `FlipHorizontalCommand`: Flip image horizontally
- `FlipVerticalCommand`: Flip image vertically
- `ResetSizeCommand`: Reset image to original size
- `ResetViewCommand`: Reset zoom and pan
- `ToggleTopMostCommand`: Toggle always-on-top
- `CopyImageCommand`: Copy selected image
- `PasteImageCommand`: Paste copied image

#### ImageItemViewModel
Individual image logic:
- Wraps ImageItem model
- Loads and displays image
- Manages selection state
- Handles flip transformations
- Provides two-way property binding

### Views

#### MainWindow
Main application window featuring:
- Infinite canvas for image display
- Drag-drop support for importing images
- Context menus (global and per-image)
- Keyboard shortcut handling
- Window state persistence

### Helpers

#### ZoomPanBehavior
Attached behavior for canvas navigation:
- Mouse wheel zoom (centered on cursor)
- Middle-click or Space+Left-click pan
- Updates ViewModel properties via two-way binding

#### ImageResizeBehavior
Attached behavior for image resizing:
- Ctrl + Mouse Wheel to resize images
- Proportional scaling

#### Converters
XAML value converters:
- `BoolToBrushConverter`: Converts boolean to brush for selection border
- `BoolToThicknessConverter`: Converts boolean to thickness for selection border

## Data Flow

### Image Import Flow
1. User drags files or uses "Add Images..." menu
2. MainViewModel validates file types
3. Images copied to `ProjectImages` folder
4. ImageItem created with metadata
5. ImageItemViewModel wraps model and loads image
6. Image added to canvas via ItemsControl binding

### Persistence Flow
1. On window closing, MainViewModel.SaveState() is called
2. Current state serialized to JSON
3. Saved to `appstate.json` in app directory
4. On startup, MainViewModel.LoadState() reads JSON
5. Images and window state restored

### User Interaction Flow
1. User interacts with UI (click, drag, keyboard)
2. Events trigger commands in ViewModel
3. ViewModel updates observable properties
4. WPF binding system updates UI automatically
5. Changes persisted on app close

## Technical Decisions

### Why .NET 8?
- LTS (Long Term Support) version
- Best performance for WPF
- Modern C# features

### Why CommunityToolkit.Mvvm?
- Source generators reduce boilerplate
- `[ObservableProperty]` and `[RelayCommand]` attributes
- Industry standard for modern MVVM

### Why System.Text.Json?
- Built-in to .NET
- High performance
- Modern and maintained

### Why Attached Behaviors?
- Keeps code-behind minimal
- Reusable across controls
- Maintains MVVM separation

## State Management

The application state is fully serializable:
- Window position and size
- All images with their properties
- View transformation (zoom/pan)
- UI settings (always-on-top)

State is saved automatically on close and restored on start, providing a seamless user experience.

## Extension Points

The architecture supports easy extension:
- Add new image manipulation commands in MainViewModel
- Create new attached behaviors for additional features
- Extend ImageItem model for more metadata
- Add new converters for custom UI bindings
