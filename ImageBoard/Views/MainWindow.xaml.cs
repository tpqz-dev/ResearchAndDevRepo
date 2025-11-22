using ImageBoard.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ImageBoard.Views
{
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel => (MainViewModel)DataContext;
        private Point? _dragStartPoint;
        private ImageItemViewModel? _draggedImage;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var state = ViewModel.LoadState();
            if (state != null)
            {
                Width = state.WindowWidth;
                Height = state.WindowHeight;
                Top = state.WindowTop;
                Left = state.WindowLeft;
                Topmost = state.IsTopMost;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.SaveState(Width, Height, Top, Left);
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".webp", ".tiff", ".tif" };
                
                var position = e.GetPosition(MainCanvas);
                var offsetX = 0.0;
                
                foreach (var file in files)
                {
                    if (imageExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                    {
                        ViewModel.AddImage(file, position.X + offsetX, position.Y);
                        offsetX += 20; // Offset subsequent images
                    }
                }
            }
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is ImageItemViewModel imageVm)
            {
                // Deselect all other images
                foreach (var img in ViewModel.Images)
                {
                    img.IsSelected = false;
                }
                
                // Select this image
                imageVm.IsSelected = true;
                ViewModel.SelectedImage = imageVm;

                _dragStartPoint = e.GetPosition(MainCanvas);
                _draggedImage = imageVm;
                border.CaptureMouse();
                e.Handled = true;
            }
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragStartPoint.HasValue && _draggedImage != null && e.LeftButton == MouseButtonState.Pressed)
            {
                var currentPoint = e.GetPosition(MainCanvas);
                var delta = currentPoint - _dragStartPoint.Value;

                _draggedImage.X += delta.X;
                _draggedImage.Y += delta.Y;

                _dragStartPoint = currentPoint;
                e.Handled = true;
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_dragStartPoint.HasValue)
            {
                _dragStartPoint = null;
                _draggedImage = null;
                
                if (sender is Border border)
                {
                    border.ReleaseMouseCapture();
                }
                
                e.Handled = true;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
