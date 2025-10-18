# File Organizer - Usage Guide

## Quick Start

1. **Launch the Application**
   - Run `FileOrganizer.exe` from the build output directory
   - Or use `dotnet run` from the FileOrganizer project directory

2. **Select Target Directory**
   - Click the "Browse..." button
   - Navigate to the directory you want to organize
   - Select the folder and click OK

3. **Configure Monitoring Interval** (Optional)
   - The default interval is 5 minutes
   - Change the value in the "Monitor Interval" field if needed
   - The interval must be at least 1 minute

4. **Start Automatic Organization**
   - Click "Start Organizing" to begin monitoring
   - The application will immediately organize files and then repeat every N minutes
   - You can see all activities in the Status Log area

5. **Manual Organization**
   - Click "Organize Now" to manually trigger organization at any time
   - This works whether monitoring is active or not

6. **Stop Monitoring**
   - Click "Stop Monitoring" to halt automatic organization
   - Files will remain in their organized state

## How Files Are Organized

The application organizes files based on their extensions:

```
Before:
/MyDocuments/
  ├── document.pdf
  ├── photo.jpg
  ├── notes.txt
  └── spreadsheet.xlsx

After:
/MyDocuments/
  ├── pdf/
  │   └── document.pdf
  ├── jpg/
  │   └── photo.jpg
  ├── txt/
  │   └── notes.txt
  └── xlsx/
      └── spreadsheet.xlsx
```

## Features

### Duplicate File Handling
If a file with the same name already exists in the target folder, a timestamp is automatically appended:
- Original: `document.pdf`
- Duplicate: `document_20241018034530.pdf`

### Extension Folders
- Folders are named after the file extension (lowercase)
- Extensions are determined from the file name (e.g., `.pdf` → `pdf` folder)
- Files without extensions are skipped

### Monitoring
- Uses Quartz.NET scheduler for reliable, background monitoring
- Runs on configurable intervals (minimum 1 minute)
- Automatically processes new files added to the directory
- Continues monitoring even when the main window is minimized

### Logging
- All operations are logged in real-time
- Timestamps are included for each log entry
- Log shows:
  - Directory selection
  - Monitoring start/stop events
  - Folder creation
  - File organization progress
  - Any errors encountered

## Configuration

Edit `appsettings.json` to set default values:

```json
{
  "FileOrganizer": {
    "DefaultIntervalMinutes": 5,
    "DefaultTargetDirectory": "C:\\Users\\YourName\\Documents"
  }
}
```

- `DefaultIntervalMinutes`: Sets the initial monitoring interval
- `DefaultTargetDirectory`: Pre-fills the directory path (optional)

## Tips

- **First Time Use**: Choose a test directory with copies of files to ensure the organization works as expected
- **Subdirectories**: The application only processes files in the root of the selected directory, not subdirectories
- **Extension Folders**: Once created, extension folders themselves are not scanned for files
- **Safety**: Always test with non-critical files first
- **Performance**: Large directories with thousands of files may take longer to process

## Troubleshooting

### Application Won't Start
- Ensure .NET 9.0 Runtime is installed
- Check that you're running on a Windows system

### Files Not Organizing
- Verify the directory path is correct
- Check that you have read/write permissions for the directory
- Ensure files have extensions (files without extensions are skipped)

### Monitoring Not Working
- Check the interval value is at least 1 minute
- Look for error messages in the Status Log
- Try stopping and restarting the monitoring

### Configuration Not Loading
- Ensure `appsettings.json` is in the same directory as the executable
- Check that the JSON syntax is valid
- Default values will be used if the configuration file is missing or invalid

## Safety Notes

- The application **moves** files (not copies), so files are relocated from their original location
- Subdirectories in the target folder are not processed
- Extension folders created by the application are not scanned
- Always keep backups of important files
- Test with non-critical data first

## Support

For issues or questions, please refer to the project README or open an issue in the repository.
