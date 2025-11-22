using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ImageBoard.Helpers
{
    public static class ZoomPanBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached(
                "IsEnabled",
                typeof(bool),
                typeof(ZoomPanBehavior),
                new PropertyMetadata(false, OnIsEnabledChanged));

        private static readonly DependencyProperty LastPanPointProperty =
            DependencyProperty.RegisterAttached(
                "LastPanPoint",
                typeof(Point?),
                typeof(ZoomPanBehavior),
                new PropertyMetadata(null));

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        private static Point? GetLastPanPoint(DependencyObject obj)
        {
            return (Point?)obj.GetValue(LastPanPointProperty);
        }

        private static void SetLastPanPoint(DependencyObject obj, Point? value)
        {
            obj.SetValue(LastPanPointProperty, value);
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Canvas canvas)
            {
                if ((bool)e.NewValue)
                {
                    canvas.MouseWheel += Canvas_MouseWheel;
                    canvas.MouseDown += Canvas_MouseDown;
                    canvas.MouseMove += Canvas_MouseMove;
                    canvas.MouseUp += Canvas_MouseUp;
                }
                else
                {
                    canvas.MouseWheel -= Canvas_MouseWheel;
                    canvas.MouseDown -= Canvas_MouseDown;
                    canvas.MouseMove -= Canvas_MouseMove;
                    canvas.MouseUp -= Canvas_MouseUp;
                }
            }
        }

        private static void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is not Canvas canvas) return;
            
            var transform = canvas.RenderTransform as TransformGroup;
            if (transform == null) return;

            var scaleTransform = transform.Children[0] as ScaleTransform;
            var translateTransform = transform.Children[1] as TranslateTransform;
            
            if (scaleTransform == null || translateTransform == null) return;

            var mousePos = e.GetPosition(canvas);
            var oldScale = scaleTransform.ScaleX;
            var delta = e.Delta > 0 ? 1.1 : 1 / 1.1;
            var newScale = oldScale * delta;

            // Limit zoom range
            newScale = Math.Max(0.1, Math.Min(10, newScale));

            // Zoom towards mouse position
            var scaleDiff = newScale / oldScale;
            translateTransform.X = mousePos.X - (mousePos.X - translateTransform.X) * scaleDiff;
            translateTransform.Y = mousePos.Y - (mousePos.Y - translateTransform.Y) * scaleDiff;

            scaleTransform.ScaleX = newScale;
            scaleTransform.ScaleY = newScale;

            e.Handled = true;
        }

        private static void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Canvas canvas) return;

            if (e.MiddleButton == MouseButtonState.Pressed || 
                (e.LeftButton == MouseButtonState.Pressed && Keyboard.IsKeyDown(Key.Space)))
            {
                SetLastPanPoint(canvas, e.GetPosition(canvas));
                canvas.CaptureMouse();
                canvas.Cursor = Cursors.Hand;
                e.Handled = true;
            }
        }

        private static void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is not Canvas canvas) return;
            
            var lastPanPoint = GetLastPanPoint(canvas);
            if (lastPanPoint.HasValue && canvas.IsMouseCaptured)
            {
                var transform = canvas.RenderTransform as TransformGroup;
                if (transform == null) return;

                var translateTransform = transform.Children[1] as TranslateTransform;
                if (translateTransform == null) return;

                var currentPoint = e.GetPosition(canvas);
                var delta = currentPoint - lastPanPoint.Value;

                translateTransform.X += delta.X;
                translateTransform.Y += delta.Y;

                SetLastPanPoint(canvas, currentPoint);
                e.Handled = true;
            }
        }

        private static void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Canvas canvas) return;

            if (GetLastPanPoint(canvas).HasValue)
            {
                SetLastPanPoint(canvas, null);
                canvas.ReleaseMouseCapture();
                canvas.Cursor = Cursors.Arrow;
                e.Handled = true;
            }
        }
    }
}
