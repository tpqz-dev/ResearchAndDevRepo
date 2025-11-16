using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace CbrViewer;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private List<byte[]> imageData = new();
    private int currentImageIndex = 0;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void OpenFile_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Comic Book Archive (*.cbr)|*.cbr|All files (*.*)|*.*",
            Title = "Select a CBR file"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            LoadCbrFile(openFileDialog.FileName);
        }
    }

    private void LoadCbrFile(string filePath)
    {
        try
        {
            // Clear previous data
            imageData.Clear();
            currentImageIndex = 0;

            // Extract images from CBR (RAR archive)
            using (var archive = ArchiveFactory.Open(filePath))
            {
                var imageEntries = archive.Entries
                    .Where(entry => !entry.IsDirectory && IsImageFile(entry.Key))
                    .OrderBy(entry => entry.Key)
                    .ToList();

                foreach (var entry in imageEntries)
                {
                    using (var stream = entry.OpenEntryStream())
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        imageData.Add(memoryStream.ToArray());
                    }
                }
            }

            if (imageData.Count > 0)
            {
                DisplayCurrentImage();
                UpdateNavigationButtons();
            }
            else
            {
                MessageBox.Show("No images found in the CBR file.", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading CBR file: {ex.Message}", "Error", 
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool IsImageFile(string? filename)
    {
        if (string.IsNullOrEmpty(filename))
            return false;

        var extension = Path.GetExtension(filename).ToLowerInvariant();
        return extension == ".jpg" || extension == ".jpeg" || 
               extension == ".png" || extension == ".gif" || 
               extension == ".bmp" || extension == ".webp";
    }

    private void DisplayCurrentImage()
    {
        if (currentImageIndex >= 0 && currentImageIndex < imageData.Count)
        {
            var bitmap = new BitmapImage();
            using (var stream = new MemoryStream(imageData[currentImageIndex]))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                bitmap.Freeze();
            }

            ImageDisplay.Source = bitmap;
            PageInfoText.Text = $"Page {currentImageIndex + 1} of {imageData.Count}";
        }
    }

    private void UpdateNavigationButtons()
    {
        PreviousButton.IsEnabled = currentImageIndex > 0;
        NextButton.IsEnabled = currentImageIndex < imageData.Count - 1;
    }

    private void Previous_Click(object sender, RoutedEventArgs e)
    {
        if (currentImageIndex > 0)
        {
            currentImageIndex--;
            DisplayCurrentImage();
            UpdateNavigationButtons();
        }
    }

    private void Next_Click(object sender, RoutedEventArgs e)
    {
        if (currentImageIndex < imageData.Count - 1)
        {
            currentImageIndex++;
            DisplayCurrentImage();
            UpdateNavigationButtons();
        }
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }
}