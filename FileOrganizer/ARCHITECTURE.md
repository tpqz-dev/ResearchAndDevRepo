# File Organizer - Architecture

## Application Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                     File Organizer WPF Application              │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐  │
│  │              MainWindow (UI Layer)                      │  │
│  │  ┌───────────────────────────────────────────────────┐  │  │
│  │  │  Directory Selector  │  Interval Input            │  │  │
│  │  ├───────────────────────────────────────────────────┤  │  │
│  │  │  Start Btn  │  Stop Btn  │  Organize Now Btn     │  │  │
│  │  ├───────────────────────────────────────────────────┤  │  │
│  │  │  Status Log (TextBlock)                          │  │  │
│  │  └───────────────────────────────────────────────────┘  │  │
│  └─────────────────────────────────────────────────────────┘  │
│                          ↓                                      │
│  ┌─────────────────────────────────────────────────────────┐  │
│  │           Business Logic (MainWindow.xaml.cs)          │  │
│  │                                                         │  │
│  │  • BrowseButton_Click()                                │  │
│  │  • StartButton_Click() → StartMonitoring()             │  │
│  │  • OrganizeFiles() ← FileOrganizerJob                  │  │
│  │  • AddLog()                                            │  │
│  └─────────────────────────────────────────────────────────┘  │
│                          ↓                                      │
│  ┌─────────────────────────────────────────────────────────┐  │
│  │         Quartz.NET Scheduler (Background Service)      │  │
│  │                                                         │  │
│  │  FileOrganizerJob                                      │  │
│  │    ↓ (Every N minutes)                                 │  │
│  │  Execute() → MainWindow.OrganizeFiles()                │  │
│  └─────────────────────────────────────────────────────────┘  │
│                          ↓                                      │
│  ┌─────────────────────────────────────────────────────────┐  │
│  │           Configuration (appsettings.json)             │  │
│  │                                                         │  │
│  │  • DefaultIntervalMinutes: 5                           │  │
│  │  • DefaultTargetDirectory: ""                          │  │
│  └─────────────────────────────────────────────────────────┘  │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

## File Organization Flow

```
┌──────────────────┐
│ User Selects Dir │
└────────┬─────────┘
         ↓
┌────────────────────┐
│ Start Monitoring   │ ←──────────────┐
└────────┬───────────┘                │
         ↓                            │
┌────────────────────┐                │
│ Quartz Job Trigger │                │
└────────┬───────────┘                │
         ↓                            │
┌────────────────────┐                │
│ Scan Directory     │                │
│ Get all files      │                │
└────────┬───────────┘                │
         ↓                            │
    ┌────────────┐                    │
    │ For each   │                    │
    │ file       │                    │
    └────┬───────┘                    │
         ↓                            │
    ┌────────────────┐                │
    │ Get extension  │                │
    └────┬───────────┘                │
         ↓                            │
    ┌────────────────┐                │
    │ Create folder  │                │
    │ if not exists  │                │
    └────┬───────────┘                │
         ↓                            │
    ┌────────────────┐                │
    │ Move file to   │                │
    │ extension dir  │                │
    └────┬───────────┘                │
         ↓                            │
┌────────────────────┐                │
│ Log operation      │                │
└────────┬───────────┘                │
         ↓                            │
┌────────────────────┐                │
│ Wait N minutes     │ ───────────────┘
└────────────────────┘
```

## Component Diagram

```
╔═══════════════════════════════════════════════════════════════╗
║                      FileOrganizer.csproj                     ║
╠═══════════════════════════════════════════════════════════════╣
║                                                               ║
║  Dependencies:                                                ║
║  ┌──────────────────────────────────────────────────────┐   ║
║  │  Quartz (3.13.1)                                     │   ║
║  │  - IScheduler                                        │   ║
║  │  - IJob (FileOrganizerJob)                          │   ║
║  │  - ITrigger (SimpleSchedule)                        │   ║
║  └──────────────────────────────────────────────────────┘   ║
║                                                               ║
║  ┌──────────────────────────────────────────────────────┐   ║
║  │  Microsoft.Extensions.Configuration (9.0.0)          │   ║
║  │  - IConfiguration                                    │   ║
║  │  - ConfigurationBuilder                              │   ║
║  └──────────────────────────────────────────────────────┘   ║
║                                                               ║
║  ┌──────────────────────────────────────────────────────┐   ║
║  │  Microsoft.Extensions.Configuration.Json (9.0.0)     │   ║
║  │  - AddJsonFile()                                     │   ║
║  └──────────────────────────────────────────────────────┘   ║
║                                                               ║
║  ┌──────────────────────────────────────────────────────┐   ║
║  │  System.Windows (WPF)                                │   ║
║  │  - Window, Application                               │   ║
║  │  - UI Controls                                       │   ║
║  └──────────────────────────────────────────────────────┘   ║
║                                                               ║
║  ┌──────────────────────────────────────────────────────┐   ║
║  │  System.Windows.Forms                                │   ║
║  │  - FolderBrowserDialog                               │   ║
║  └──────────────────────────────────────────────────────┘   ║
║                                                               ║
╚═══════════════════════════════════════════════════════════════╝
```

## Class Diagram

```
┌─────────────────────────────────┐
│         Application             │
├─────────────────────────────────┤
│ + OnStartup()                   │
└────────────┬────────────────────┘
             │ creates
             ↓
┌─────────────────────────────────┐
│         MainWindow              │
├─────────────────────────────────┤
│ - _scheduler: IScheduler?       │
│ - _targetDirectory: string      │
│ - _intervalMinutes: int         │
│ - _configuration: IConfiguration│
├─────────────────────────────────┤
│ + LoadConfiguration()           │
│ + BrowseButton_Click()          │
│ + StartButton_Click()           │
│ + StopButton_Click()            │
│ + OrganizeNowButton_Click()     │
│ + StartMonitoring()             │
│ + StopMonitoring()              │
│ + OrganizeFiles()               │
│ + AddLog()                      │
└────────────┬────────────────────┘
             │ uses
             ↓
┌─────────────────────────────────┐
│     FileOrganizerJob            │
│        (IJob)                   │
├─────────────────────────────────┤
│ + Execute(context)              │
│   → calls MainWindow            │
│      .OrganizeFiles()           │
└─────────────────────────────────┘
```

## Data Flow

```
┌──────────────┐
│ User Input   │ (Select directory, Set interval)
└──────┬───────┘
       ↓
┌──────────────────┐
│ Configuration    │ (Load from appsettings.json)
│ Initialization   │
└──────┬───────────┘
       ↓
┌──────────────────┐
│ Scheduler Setup  │ (Create Quartz job and trigger)
└──────┬───────────┘
       ↓
┌──────────────────┐
│ Job Execution    │ (Triggered by schedule)
└──────┬───────────┘
       ↓
┌──────────────────┐
│ File Scanning    │ (Directory.GetFiles)
└──────┬───────────┘
       ↓
┌──────────────────┐
│ File Processing  │ (For each file)
│                  │  1. Get extension
│                  │  2. Create folder
│                  │  3. Move file
└──────┬───────────┘
       ↓
┌──────────────────┐
│ Logging          │ (Update UI log)
└──────────────────┘
```

## Thread Model

```
Main UI Thread:
  ├─ Window initialization
  ├─ User interaction handling
  ├─ UI updates (AddLog via Dispatcher)
  └─ Configuration loading

Background Thread (Quartz):
  ├─ Job scheduling
  ├─ Trigger execution
  └─ File operations
      └─ Calls back to UI thread for logging
```

## Error Handling Strategy

```
User Action → Try/Catch → MessageBox (Error)
                ↓
           AddLog (Error message)

File Operation → Try/Catch → Continue with next file
                    ↓
               AddLog (Error details)

Configuration Load → Try/Catch → Use defaults
                        ↓
                   AddLog (Warning)

Scheduler → Try/Catch → Stop gracefully
               ↓
          Notify user
```
