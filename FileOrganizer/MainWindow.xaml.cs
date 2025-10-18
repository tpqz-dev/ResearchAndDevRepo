using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FileOrganizer
{
    public partial class MainWindow : Window
    {
        private IScheduler? _scheduler;
        private string _targetDirectory = string.Empty;
        private int _intervalMinutes = 5;
        private IConfiguration? _configuration;

        public MainWindow()
        {
            InitializeComponent();
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                _configuration = builder.Build();

                // Load default values
                var defaultInterval = _configuration["FileOrganizer:DefaultIntervalMinutes"];
                if (int.TryParse(defaultInterval, out int interval) && interval > 0)
                {
                    _intervalMinutes = interval;
                    IntervalTextBox.Text = interval.ToString();
                }

                var defaultDirectory = _configuration["FileOrganizer:DefaultTargetDirectory"];
                if (!string.IsNullOrEmpty(defaultDirectory) && Directory.Exists(defaultDirectory))
                {
                    _targetDirectory = defaultDirectory;
                    DirectoryPathTextBox.Text = defaultDirectory;
                    StartButton.IsEnabled = true;
                    OrganizeNowButton.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                AddLog($"Warning: Could not load configuration: {ex.Message}");
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select target directory to organize";
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _targetDirectory = dialog.SelectedPath;
                    DirectoryPathTextBox.Text = _targetDirectory;
                    StartButton.IsEnabled = true;
                    OrganizeNowButton.IsEnabled = true;
                    AddLog($"Target directory selected: {_targetDirectory}");
                }
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_targetDirectory))
            {
                System.Windows.MessageBox.Show("Please select a target directory first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(IntervalTextBox.Text, out _intervalMinutes) || _intervalMinutes < 1)
            {
                System.Windows.MessageBox.Show("Please enter a valid interval (minimum 1 minute).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                await StartMonitoring();
                StartButton.IsEnabled = false;
                StopButton.IsEnabled = true;
                BrowseButton.IsEnabled = false;
                IntervalTextBox.IsEnabled = false;
                StatusTextBlock.Text = $"Monitoring active (every {_intervalMinutes} minutes)";
                AddLog($"Started monitoring with {_intervalMinutes} minute interval");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error starting monitoring: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await StopMonitoring();
                StartButton.IsEnabled = true;
                StopButton.IsEnabled = false;
                BrowseButton.IsEnabled = true;
                IntervalTextBox.IsEnabled = true;
                StatusTextBlock.Text = "Monitoring stopped";
                AddLog("Monitoring stopped");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error stopping monitoring: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OrganizeNowButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_targetDirectory))
            {
                System.Windows.MessageBox.Show("Please select a target directory first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                OrganizeFiles();
                AddLog("Manual organization completed");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error organizing files: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task StartMonitoring()
        {
            // Create scheduler factory
            StdSchedulerFactory factory = new StdSchedulerFactory();
            _scheduler = await factory.GetScheduler();
            await _scheduler.Start();

            // Define the job
            IJobDetail job = JobBuilder.Create<FileOrganizerJob>()
                .WithIdentity("fileOrganizerJob", "group1")
                .UsingJobData("targetDirectory", _targetDirectory)
                .Build();

            // Define the trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("fileOrganizerTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(_intervalMinutes)
                    .RepeatForever())
                .Build();

            // Schedule the job
            await _scheduler.ScheduleJob(job, trigger);

            // Pass reference to MainWindow for logging
            _scheduler.Context.Put("mainWindow", this);
        }

        private async Task StopMonitoring()
        {
            if (_scheduler != null)
            {
                await _scheduler.Shutdown();
                _scheduler = null;
            }
        }

        public void OrganizeFiles()
        {
            if (!Directory.Exists(_targetDirectory))
            {
                AddLog($"Error: Directory does not exist: {_targetDirectory}");
                return;
            }

            AddLog("Starting file organization...");
            int filesOrganized = 0;

            try
            {
                // Get all files in the target directory (not subdirectories)
                var files = Directory.GetFiles(_targetDirectory);

                foreach (var file in files)
                {
                    try
                    {
                        var fileInfo = new FileInfo(file);
                        var extension = fileInfo.Extension.TrimStart('.').ToLowerInvariant();

                        // Skip files without extension
                        if (string.IsNullOrEmpty(extension))
                        {
                            continue;
                        }

                        // Create extension folder if it doesn't exist
                        var extensionFolder = Path.Combine(_targetDirectory, extension);
                        if (!Directory.Exists(extensionFolder))
                        {
                            Directory.CreateDirectory(extensionFolder);
                            AddLog($"Created folder: {extension}");
                        }

                        // Move file to extension folder
                        var targetPath = Path.Combine(extensionFolder, fileInfo.Name);
                        
                        // Handle duplicate file names
                        if (File.Exists(targetPath))
                        {
                            var nameWithoutExt = Path.GetFileNameWithoutExtension(fileInfo.Name);
                            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                            targetPath = Path.Combine(extensionFolder, $"{nameWithoutExt}_{timestamp}.{extension}");
                        }

                        File.Move(file, targetPath);
                        filesOrganized++;
                    }
                    catch (Exception ex)
                    {
                        AddLog($"Error processing file {Path.GetFileName(file)}: {ex.Message}");
                    }
                }

                AddLog($"Organization complete. {filesOrganized} file(s) organized.");
            }
            catch (Exception ex)
            {
                AddLog($"Error during organization: {ex.Message}");
            }
        }

        public void AddLog(string message)
        {
            Dispatcher.Invoke(() =>
            {
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                LogTextBlock.Text += $"[{timestamp}] {message}\n";
            });
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (_scheduler != null)
            {
                _scheduler.Shutdown().Wait();
            }
        }
    }

    // Quartz Job class
    public class FileOrganizerJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var targetDirectory = context.JobDetail.JobDataMap.GetString("targetDirectory");
            var mainWindow = (MainWindow)context.Scheduler.Context.Get("mainWindow");

            await Task.Run(() =>
            {
                mainWindow.AddLog("Scheduled organization triggered");
                mainWindow.OrganizeFiles();
            });
        }
    }
}
