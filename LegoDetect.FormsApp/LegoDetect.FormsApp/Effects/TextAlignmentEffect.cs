namespace LegoDetect.FormsApp.Effects;

using System.Linq;

using Xamarin.Forms;

public static class TextAlignment
{
    public static readonly BindableProperty OnProperty = BindableProperty.CreateAttached(
        "On",
        typeof(bool),
        typeof(TextAlignment),
        false,
        propertyChanged: OnOnChanged);

    public static bool GetOn(BindableObject bindable) => (bool)bindable.GetValue(OnProperty);

    public static void SetOn(BindableObject bindable, bool value) => bindable.SetValue(OnProperty, value);

    public static readonly BindableProperty VerticalProperty =
        BindableProperty.CreateAttached(
            "Vertical",
            typeof(Xamarin.Forms.TextAlignment),
            typeof(TextAlignment),
            Xamarin.Forms.TextAlignment.Center);

    public static Xamarin.Forms.TextAlignment GetVertical(BindableObject bindable) =>
        (Xamarin.Forms.TextAlignment)bindable.GetValue(VerticalProperty);

    public static void SetVertical(BindableObject bindable, Xamarin.Forms.TextAlignment value) =>
        bindable.SetValue(VerticalProperty, value);

    public static readonly BindableProperty HorizontalProperty =
        BindableProperty.CreateAttached(
            "Horizontal",
            typeof(Xamarin.Forms.TextAlignment),
            typeof(TextAlignment),
            Xamarin.Forms.TextAlignment.Center);

    public static Xamarin.Forms.TextAlignment GetHorizontal(BindableObject bindable) =>
        (Xamarin.Forms.TextAlignment)bindable.GetValue(HorizontalProperty);

    public static void SetHorizontal(BindableObject bindable, Xamarin.Forms.TextAlignment value) =>
        bindable.SetValue(HorizontalProperty, value);

    private static void OnOnChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Element element)
        {
            return;
        }

        if ((bool)newValue)
        {
            element.Effects.Add(new TextAlignmentEffect());
        }
        else
        {
            var effect = element.Effects.FirstOrDefault(x => x is TextAlignmentEffect);
            if (effect != null)
            {
                element.Effects.Remove(effect);
            }
        }
    }
}

public sealed class TextAlignmentEffect : RoutingEffect
{
    public TextAlignmentEffect()
        : base("LegoDetect.TextAlignmentEffect")
    {
    }
}
