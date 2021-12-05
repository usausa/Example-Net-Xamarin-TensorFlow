[assembly: Xamarin.Forms.ExportEffect(typeof(LegoDetect.FormsApp.Droid.Effects.TextAlignmentPlatformEffect), nameof(LegoDetect.FormsApp.Effects.TextAlignmentEffect))]

namespace LegoDetect.FormsApp.Droid.Effects;

using System.ComponentModel;

using Android.Widget;

using LegoDetect.FormsApp.Droid.Helpers;
using LegoDetect.FormsApp.Effects;

using Xamarin.Forms.Platform.Android;

public sealed class TextAlignmentPlatformEffect : PlatformEffect
{
    protected override void OnAttached()
    {
        UpdateAlignment();
    }

    protected override void OnDetached()
    {
    }

    protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnElementPropertyChanged(args);

        if ((args.PropertyName == TextAlignment.VerticalProperty.PropertyName) ||
            (args.PropertyName == TextAlignment.HorizontalProperty.PropertyName))
        {
            UpdateAlignment();
        }
    }

    private void UpdateAlignment()
    {
        if (Control is TextView textView)
        {
            textView.Gravity = TextAlignment.GetVertical(Element).ToVerticalGravity() |
                               TextAlignment.GetHorizontal(Element).ToHorizontalGravity();
        }
    }
}
