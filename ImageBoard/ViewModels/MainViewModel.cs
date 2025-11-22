using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ImageBoard.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace ImageBoard.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private const string StateFileName = "appstate.json";
        private const string ImagesFolder = "ProjectImages";

        [ObservableProperty]
        private ObservableCollection<ImageItemViewModel> _images = new();

        [ObservableProperty]
        private double _viewTranslateX;

        [ObservableProperty]
        private double _viewTranslateY;

        [ObservableProperty]
        private double _viewScale = 1.0;

        [ObservableProperty]
        private bool _isTopMost;

        [ObservableProperty]
        private ImageItemViewModel? _selectedImage;

        private string _baseDirectory;
        private string _imagesDirectory;
        private int _nextZIndex = 1;
        private ImageItemViewModel? _copiedImage;

        public MainViewModel()
        {
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _imagesDirectory = Path.Combine(_baseDirectory, ImagesFolder);
            Directory.CreateDirectory(_imagesDirectory);
        }

        private string GenerateUniqueImagePath(string sourceFilePath)
        {
            var extension = Path.GetExtension(sourceFilePath);
            return Path.Combine(_imagesDirectory, Guid.NewGuid().ToString() + extension);
        }

        public void ClearSelection()
        {
            foreach (var img in Images)
            {
                img.IsSelected = false;
            }
        }

        [RelayCommand]
        private void AddImages()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.webp;*.tiff;*.tif",
                Title = "Select Images"
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (var filePath in dialog.FileNames)
                {
                    AddImage(filePath, 100, 100);
                }
            }
        }

        public void AddImage(string sourceFilePath, double x, double y)
        {
            try
            {
                var destPath = GenerateUniqueImagePath(sourceFilePath);
                
                File.Copy(sourceFilePath, destPath, true);

                var relativePath = Path.GetRelativePath(_baseDirectory, destPath);
                var imageItem = new ImageItem
                {
                    SourcePath = relativePath,
                    X = x,
                    Y = y,
                    ZIndex = _nextZIndex++
                };

                var viewModel = new ImageItemViewModel(imageItem, destPath);
                Images.Add(viewModel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void DeleteImage(ImageItemViewModel? image)
        {
            if (image != null && Images.Contains(image))
            {
                Images.Remove(image);
                if (SelectedImage == image)
                {
                    SelectedImage = null;
                }
            }
        }

        [RelayCommand]
        private void BringToFront(ImageItemViewModel? image)
        {
            if (image != null)
            {
                image.ZIndex = _nextZIndex++;
            }
        }

        [RelayCommand]
        private void SendToBack(ImageItemViewModel? image)
        {
            if (image != null)
            {
                var minZ = Images.Any() ? Images.Min(i => i.ZIndex) : 0;
                image.ZIndex = minZ - 1;
            }
        }

        [RelayCommand]
        private void FlipHorizontal(ImageItemViewModel? image)
        {
            if (image != null)
            {
                image.IsFlippedX = !image.IsFlippedX;
            }
        }

        [RelayCommand]
        private void FlipVertical(ImageItemViewModel? image)
        {
            if (image != null)
            {
                image.IsFlippedY = !image.IsFlippedY;
            }
        }

        [RelayCommand]
        private void ResetSize(ImageItemViewModel? image)
        {
            if (image != null && image.ImageSource != null)
            {
                image.Width = image.ImageSource.PixelWidth;
                image.Height = image.ImageSource.PixelHeight;
                image.Scale = 1.0;
            }
        }

        [RelayCommand]
        private void ResetView()
        {
            ViewScale = 1.0;
            ViewTranslateX = 0;
            ViewTranslateY = 0;
        }

        [RelayCommand]
        private void ToggleTopMost()
        {
            IsTopMost = !IsTopMost;
        }

        [RelayCommand]
        private void CopyImage(ImageItemViewModel? image)
        {
            if (image != null)
            {
                _copiedImage = image;
            }
        }

        [RelayCommand]
        private void PasteImage()
        {
            if (_copiedImage != null)
            {
                try
                {
                    var sourcePath = Path.Combine(_baseDirectory, _copiedImage.SourcePath);
                    if (File.Exists(sourcePath))
                    {
                        var destPath = GenerateUniqueImagePath(sourcePath);
                        
                        File.Copy(sourcePath, destPath, true);

                        var relativePath = Path.GetRelativePath(_baseDirectory, destPath);
                        var imageItem = new ImageItem
                        {
                            SourcePath = relativePath,
                            X = _copiedImage.X + 20,
                            Y = _copiedImage.Y + 20,
                            Width = _copiedImage.Width,
                            Height = _copiedImage.Height,
                            Scale = _copiedImage.Scale,
                            IsFlippedX = _copiedImage.IsFlippedX,
                            IsFlippedY = _copiedImage.IsFlippedY,
                            ZIndex = _nextZIndex++
                        };

                        var viewModel = new ImageItemViewModel(imageItem, destPath);
                        Images.Add(viewModel);
                        
                        // Select the pasted image
                        ClearSelection();
                        viewModel.IsSelected = true;
                        SelectedImage = viewModel;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to paste image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void SaveState(double windowWidth, double windowHeight, double windowTop, double windowLeft)
        {
            try
            {
                var state = new BoardState
                {
                    Images = Images.Select(i => i.ToModel()).ToList(),
                    WindowWidth = windowWidth,
                    WindowHeight = windowHeight,
                    WindowTop = windowTop,
                    WindowLeft = windowLeft,
                    IsTopMost = IsTopMost,
                    ViewTranslateX = ViewTranslateX,
                    ViewTranslateY = ViewTranslateY,
                    ViewScale = ViewScale
                };

                var json = JsonSerializer.Serialize(state, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(Path.Combine(_baseDirectory, StateFileName), json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save state: {ex.Message}");
            }
        }

        public BoardState? LoadState()
        {
            try
            {
                var stateFile = Path.Combine(_baseDirectory, StateFileName);
                if (File.Exists(stateFile))
                {
                    var json = File.ReadAllText(stateFile);
                    var state = JsonSerializer.Deserialize<BoardState>(json);
                    
                    if (state != null)
                    {
                        ViewTranslateX = state.ViewTranslateX;
                        ViewTranslateY = state.ViewTranslateY;
                        ViewScale = state.ViewScale;
                        IsTopMost = state.IsTopMost;

                        foreach (var imageItem in state.Images)
                        {
                            var fullPath = Path.Combine(_baseDirectory, imageItem.SourcePath);
                            if (File.Exists(fullPath))
                            {
                                var viewModel = new ImageItemViewModel(imageItem, fullPath);
                                Images.Add(viewModel);
                                _nextZIndex = Math.Max(_nextZIndex, imageItem.ZIndex + 1);
                            }
                        }

                        return state;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load state: {ex.Message}");
            }

            return null;
        }
    }
}
