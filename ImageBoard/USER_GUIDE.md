# ImageBoard - User Guide

## Introduction

ImageBoard is a desktop application that provides an infinite canvas for working with images. It's designed to be minimalist and focused on the visual experience, with no traditional menu bar - all functionality is accessible through context menus and keyboard shortcuts.

## Getting Started

### First Launch

When you first launch ImageBoard, you'll see an empty white canvas. This is your workspace where you can add and organize images.

### Adding Images

There are two ways to add images to your board:

1. **Drag and Drop** (Recommended)
   - Simply drag image files from Windows Explorer and drop them onto the ImageBoard window
   - Multiple images can be dropped at once
   - Images will appear at the drop location

2. **File Dialog**
   - Right-click anywhere on the canvas
   - Select "Add Images..." from the context menu
   - Choose one or more image files
   - Selected images will appear on the canvas

**Supported Formats**: JPEG, PNG, BMP, GIF, WEBP, TIFF

## Navigating the Canvas

### Zoom

- **Mouse Wheel**: Scroll up to zoom in, scroll down to zoom out
- Zoom is centered on your mouse cursor position
- Zoom range: 10% to 1000%

### Pan (Move the View)

You can pan the canvas in two ways:
- **Middle Mouse Button**: Click and hold the middle button, then drag
- **Space + Left Click**: Hold the Space key, then click and hold left mouse button, then drag

The cursor will change to a hand icon while panning.

### Reset View

To return to the default view (100% zoom, centered):
- Right-click on the canvas
- Select "Reset View"

## Working with Images

### Selecting Images

- Click on any image to select it
- Selected images show a blue border
- Only one image can be selected at a time
- Clicking on the canvas background deselects all images

### Moving Images

- Click and drag any image to move it around the canvas
- You don't need to select the image first - just click and drag

### Resizing Images

- **Ctrl + Mouse Wheel** while hovering over an image
- Scroll up to make larger, scroll down to make smaller
- The image will resize proportionally

### Flipping Images

Right-click on an image and select:
- **Flip Horizontal**: Mirror the image left-to-right
- **Flip Vertical**: Mirror the image top-to-bottom

### Layering Images

Control which images appear on top:
- **Bring to Front**: Right-click image → "Bring to Front"
- **Send to Back**: Right-click image → "Send to Back"

### Copying and Pasting

**Copy an Image:**
- Select the image and press **Ctrl+C**, or
- Right-click the image and select "Copy"

**Paste an Image:**
- Press **Ctrl+V**, or
- Right-click on the canvas and select "Paste"
- The pasted image will appear slightly offset from the original

### Deleting Images

- Select an image and press **Delete**, or
- Right-click the image and select "Delete"

### Reset Image Size

To restore an image to its original size:
- Right-click the image
- Select "Reset Size"

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| **Ctrl+C** | Copy selected image |
| **Ctrl+V** | Paste copied image |
| **Delete** | Remove selected image |
| **Ctrl + Mouse Wheel** | Resize image under cursor |
| **Space + Left Click + Drag** | Pan the canvas |

## Always on Top

To keep ImageBoard above all other windows:
- Right-click on the canvas
- Check "Always on Top"
- The window will stay on top until you uncheck this option

This is useful for reference images while working in other applications.

## Context Menus

### Canvas Context Menu (Right-click on background)

- Add Images...
- Paste
- Always on Top (checkbox)
- Reset View
- Exit

### Image Context Menu (Right-click on an image)

- Bring to Front
- Send to Back
- Flip Horizontal
- Flip Vertical
- Copy
- Reset Size
- Delete

## Data Storage and Persistence

### Automatic Saving

ImageBoard automatically saves your work when you close the application. This includes:
- All images on the canvas
- Image positions, sizes, and transformations
- Window size and position
- Zoom and pan state
- "Always on Top" setting

### Where Images Are Stored

When you add an image to the board, it is copied to a local folder:
- Location: `ProjectImages` folder next to the ImageBoard executable
- Each image is saved with a unique filename
- Original files are not modified

### State File

Application state is saved to:
- File: `appstate.json` next to the executable
- Format: Human-readable JSON
- Contains all information needed to restore your session

### Backup Recommendation

To backup your work, copy these items:
- The `ProjectImages` folder (contains all your images)
- The `appstate.json` file (contains layout and settings)

## Tips and Tricks

1. **Quick Reference Board**: Use "Always on Top" to keep reference images visible while working in other applications

2. **Organize by Layers**: Use "Bring to Front" and "Send to Back" to organize overlapping images

3. **Compare Images**: Place multiple versions of an image side-by-side and use zoom to examine details

4. **Temporary Workspace**: Since everything is saved automatically, you can close and reopen ImageBoard without losing your layout

5. **Duplicate Images**: Use Ctrl+C and Ctrl+V to create multiple copies of the same image for comparison

## Troubleshooting

### Images Don't Appear After Adding
- Check that the image format is supported
- Verify the image file is not corrupted
- Try adding a different image

### Images Missing After Restart
- Ensure the `ProjectImages` folder and `appstate.json` are in the same location as the executable
- Don't move or delete files from the `ProjectImages` folder manually

### Performance Issues with Many Images
- Close and reopen the application to free memory
- Consider using smaller image files
- Reduce the number of images on the canvas

### Canvas Won't Pan or Zoom
- Make sure you're not clicking on an image when trying to pan
- Try using the alternative pan method (Space + Left Click instead of Middle Click)
- Use "Reset View" to return to a known state

## Exiting the Application

To close ImageBoard:
- Click the X button on the window title bar
- Right-click the canvas and select "Exit"
- Press Alt+F4

Your work is automatically saved when you exit.
