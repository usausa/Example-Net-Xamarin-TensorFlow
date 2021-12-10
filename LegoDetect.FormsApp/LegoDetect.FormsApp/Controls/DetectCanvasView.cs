namespace LegoDetect.FormsApp.Controls;

using System;

using Models;

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
        using var backgroundPaint = new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.White };

        canvas.DrawRect(info.Rect, backgroundPaint);

        if (image is null)
        {
            return;
        }

        var scale = Math.Min(info.Width / (float)image.Width, info.Height / (float)image.Height);

        var scaleWidth = scale * image.Width;
        var scaleHeight = scale * image.Height;

        var left = (info.Width - scaleWidth) / 2;
        var top = (info.Height - scaleHeight) / 2;

        // Draw bitmap
        canvas.DrawBitmap(image, new SKRect(left, top, left + scaleWidth, top + scaleHeight));

        // Draw box
        if (DetectResult is not null)
        {
            foreach (var result in DetectResult)
            {
                var scaledBoxLeft = left + (scaleWidth * result.Bounds.Left);
                var scaledBoxWidth = scaleWidth * result.Bounds.Width;
                var scaledBoxTop = top + (scaleHeight * result.Bounds.Top);
                var scaledBoxHeight = scaleHeight * result.Bounds.Height;
                using var path = CreateBoxPath(scaledBoxLeft, scaledBoxTop, scaledBoxWidth, scaledBoxHeight);
                using var strokePaint = new SKPaint
                {
                    IsAntialias = true,
                    Style = SKPaintStyle.Stroke,
                    Color = result.Score > 0.75 ? SKColors.Red : result.Score > 0.5 ? SKColors.OrangeRed : SKColors.Yellow,
                    StrokeWidth = 3
                };

                canvas.DrawPath(path, strokePaint);
            }
        }
    }

    private static SKPath CreateBoxPath(float left, float top, float width, float height)
    {
        var path = new SKPath();
        path.MoveTo(left, top);
        path.LineTo(left + width, top);
        path.LineTo(left + width, top + height);
        path.LineTo(left, top + height);
        path.LineTo(left, top);
        return path;
    }

    public void Load(byte[] data)
    {
        image = SKBitmap.Decode(data);
        InvalidateSurface();
    }
}
