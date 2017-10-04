using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Win2DLeakTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Win2DPage : Page
    {
        public Win2DPage()
        {
            this.InitializeComponent();
            var coreWindow = SystemNavigationManager.GetForCurrentView();
            coreWindow.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            coreWindow.BackRequested += (s, e) =>
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
                else
                {
                    Frame.Navigate(typeof(MainPage));
                }

                e.Handled = true;
            };
        }

        CanvasBitmap _imageBitmap;

        private void CanvasControl_OnCreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(InitiateResources(sender).AsAsyncAction());
        }

        private async Task InitiateResources(CanvasControl canvasControl)
        {
            var imageStorageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/test_image.png"));
            _imageBitmap = await CanvasBitmap.LoadAsync(canvasControl, await imageStorageFile.OpenAsync(FileAccessMode.Read));
        }

        private void CanvasControl_OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            Draw(args.DrawingSession);
        }

        private void Draw(CanvasDrawingSession session)
        {
            var scaleEffect = new ScaleEffect
            {
                Source = _imageBitmap,
                Scale = new Vector2(GetScaleFromBounds())
            };
            var otherEffect = new GaussianBlurEffect
            {
                Source = scaleEffect,
                BlurAmount = 1.3f,
                Optimization = EffectOptimization.Quality
            };

            session.DrawImage(otherEffect);
        }

        private float GetScaleFromBounds()
        {
            var scale = Math.Min(
                WinCanvas.ActualWidth / _imageBitmap.Bounds.Width,
                WinCanvas.ActualHeight / _imageBitmap.Bounds.Height);

            return (float)scale;
        }

        void Win2DPage_OnUnloaded(object sender, RoutedEventArgs e)
        {
            WinCanvas.RemoveFromVisualTree();
            WinCanvas = null;
            _imageBitmap.Dispose();
            _imageBitmap = null;
        }

        void Win2DPage_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            WinCanvas.Invalidate();
        }
    }
}
