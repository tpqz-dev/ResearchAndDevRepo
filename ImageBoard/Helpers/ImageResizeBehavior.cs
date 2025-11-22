using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ImageBoard.Helpers
{
    /// <summary>
    /// Attached behavior for resizing images with mouse wheel when Ctrl is pressed
    /// </summary>
    public static class ImageResizeBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(ImageResizeBehavior),
                new PropertyMetadata(false, OnIsEnabledChanged));

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Border border)
            {
                if ((bool)e.NewValue)
                {
                    border.MouseWheel += Border_MouseWheel;
                }
                else
                {
                    border.MouseWheel -= Border_MouseWheel;
                }
            }
        }

        private static void Border_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))
                return;

            if (sender is Border border && border.DataContext is ViewModels.ImageItemViewModel imageVm)
            {
                var delta = e.Delta > 0 ? 1.1 : 1 / 1.1;
                
                imageVm.Width *= delta;
                imageVm.Height *= delta;

                e.Handled = true;
            }
        }
    }
}
