namespace LegoDetect.FormsApp.Droid.Helpers;

using System;

using Android.Views;

public static class AlignmentExtensions
{
    public static GravityFlags ToHorizontalGravity(this Xamarin.Forms.TextAlignment alignment)
    {
        return alignment switch
        {
            Xamarin.Forms.TextAlignment.Center => GravityFlags.CenterHorizontal,
            Xamarin.Forms.TextAlignment.End => GravityFlags.Right,
            Xamarin.Forms.TextAlignment.Start => GravityFlags.Left,
            _ => throw new InvalidOperationException(alignment.ToString())
        };
    }

    public static GravityFlags ToVerticalGravity(this Xamarin.Forms.TextAlignment alignment)
    {
        return alignment switch
        {
            Xamarin.Forms.TextAlignment.Center => GravityFlags.CenterVertical,
            Xamarin.Forms.TextAlignment.End => GravityFlags.Bottom,
            Xamarin.Forms.TextAlignment.Start => GravityFlags.Top,
            _ => throw new InvalidOperationException(alignment.ToString())
        };
    }
}
