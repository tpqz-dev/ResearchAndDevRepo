using CommunityToolkit.Mvvm.ComponentModel;
using ImageBoard.Models;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageBoard.ViewModels
{
    public partial class ImageItemViewModel : ObservableObject
    {
        private readonly ImageItem _model;

        [ObservableProperty]
        private double _x;

        [ObservableProperty]
        private double _y;

        [ObservableProperty]
        private double _width;

        [ObservableProperty]
        private double _height;

        [ObservableProperty]
        private double _scale = 1.0;

        [ObservableProperty]
        private bool _isFlippedX;

        [ObservableProperty]
        private bool _isFlippedY;

        [ObservableProperty]
        private int _zIndex;

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private BitmapImage? _imageSource;

        public Guid Id { get; }
        public string SourcePath { get; }

        public ScaleTransform FlipTransform => new ScaleTransform(
            IsFlippedX ? -1 : 1,
            IsFlippedY ? -1 : 1
        );

        public ImageItemViewModel(ImageItem model, string fullPath)
        {
            _model = model;
            Id = model.Id;
            SourcePath = model.SourcePath;
            _x = model.X;
            _y = model.Y;
            _width = model.Width;
            _height = model.Height;
            _scale = model.Scale;
            _isFlippedX = model.IsFlippedX;
            _isFlippedY = model.IsFlippedY;
            _zIndex = model.ZIndex;

            LoadImage(fullPath);
        }

        private void LoadImage(string fullPath)
        {
            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                bitmap.EndInit();
                bitmap.Freeze();
                ImageSource = bitmap;

                if (Width == 0 || Height == 0)
                {
                    Width = bitmap.PixelWidth;
                    Height = bitmap.PixelHeight;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load image: {ex.Message}");
            }
        }

        public ImageItem ToModel()
        {
            return new ImageItem
            {
                Id = Id,
                SourcePath = SourcePath,
                X = X,
                Y = Y,
                Width = Width,
                Height = Height,
                Scale = Scale,
                IsFlippedX = IsFlippedX,
                IsFlippedY = IsFlippedY,
                ZIndex = ZIndex
            };
        }

        partial void OnIsFlippedXChanged(bool value)
        {
            OnPropertyChanged(nameof(FlipTransform));
        }

        partial void OnIsFlippedYChanged(bool value)
        {
            OnPropertyChanged(nameof(FlipTransform));
        }
    }
}
