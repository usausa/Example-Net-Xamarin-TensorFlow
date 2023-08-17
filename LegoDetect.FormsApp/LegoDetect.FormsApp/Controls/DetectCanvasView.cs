namespace LegoDetect.FormsApp.Controls;

using LegoDetect.FormsApp.Models;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using Xamarin.Forms;

public class DetectCanvasView : SKCanvasView
{
    private SKBitmap? image;

    public static readonly BindableProperty DetectResultProperty =
        BindableProperty.CreateAttached(
            nameof(DetectResult),
            typeof(DetectResult[]),
            typeof(DetectCanvasView),
            null,
            propertyChanged: HandleDetectResultPropertyChanged);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Ignore")]
    public DetectResult[]? DetectResult
    {
        get => (DetectResult[])GetValue(DetectResultProperty);
        set => SetValue(DetectResultProperty, value);
    }

    private static void HandleDetectResultPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((DetectCanvasView)bindable).InvalidateSurface();
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        var info = e.Info;
        var canvas = e.Surface.Canvas;

        // Fill background
        using var backgroundPaint = new SKPaint();
        backgroundPaint.Style = SKPaintStyle.Fill;
        backgroundPaint.Color = SKColors.White;

        canvas.DrawRect(info.Rect, backgroundPaint);

        if (image is null)
        {
            return;
        }

        var imageAspect = (float)image.Width / image.Height;
        var infoAspect = (float)info.Width / info.Height;
        float scaleWidth;
        float scaleHeight;
        float left;
        float top;
        if (infoAspect > imageAspect)
        {
            scaleHeight = info.Height;
            scaleWidth = info.Height * imageAspect;
            top = 0;
            left = (info.Width - scaleWidth) / 2;
        }
        else
        {
            scaleWidth = info.Width;
            scaleHeight = scaleWidth * imageAspect;
            left = 0;
            top = (info.Height - scaleHeight) / 2;
        }

        // Draw bitmap
        canvas.DrawBitmap(image, new SKRect(left, top, left + scaleWidth, top + scaleHeight));

        // Draw box
        if (DetectResult is not null)
        {
            foreach (var result in DetectResult)
            {
                var scaledBoxLeft = left + (scaleWidth * result.Bounds.Left);
                var scaledBoxTop = top + (scaleHeight * result.Bounds.Top);
                var scaledBoxRight = left + (scaleWidth * result.Bounds.Right);
                var scaledBoxBottom = top + (scaleHeight * result.Bounds.Bottom);
                using var path = CreateBoxPath(scaledBoxLeft, scaledBoxTop, scaledBoxRight, scaledBoxBottom);
                using var strokePaint = new SKPaint();
                strokePaint.IsAntialias = true;
                strokePaint.Style = SKPaintStyle.Stroke;
                strokePaint.Color = result.Score > 0.75 ? SKColors.Red : result.Score > 0.5 ? SKColors.OrangeRed : SKColors.Yellow;
                strokePaint.StrokeWidth = 3;

                canvas.DrawPath(path, strokePaint);
            }
        }
    }

    private static SKPath CreateBoxPath(float left, float top, float right, float bottom)
    {
        var path = new SKPath();
        path.MoveTo(left, top);
        path.LineTo(right, top);
        path.LineTo(right, bottom);
        path.LineTo(left, bottom);
        path.LineTo(left, top);
        return path;
    }

    public void Load(byte[] data)
    {
        image = SKBitmap.Decode(data);
        InvalidateSurface();
    }
}
