namespace LegoDetect.FormsApp.Effects;

using System.Linq;

using Xamarin.Forms;

public static class Border
{
    public static readonly BindableProperty WidthProperty =
        BindableProperty.CreateAttached(
            "Width",
            typeof(double),
            typeof(Border),
            default(double),
            propertyChanged: OnPropertyChanged);

    public static readonly BindableProperty ColorProperty =
        BindableProperty.CreateAttached(
            "Color",
            typeof(Color),
            typeof(Border),
            Color.Transparent);

    public static void SetWidth(BindableObject bindable, double value) => bindable.SetValue(WidthProperty, value);

    public static double GetWidth(BindableObject bindable) => (double)bindable.GetValue(WidthProperty);

    public static void SetColor(BindableObject bindable, Color value) => bindable.SetValue(ColorProperty, value);

    public static Color GetColor(BindableObject bindable) => (Color)bindable.GetValue(ColorProperty);

    private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not VisualElement element)
        {
            return;
        }

        var width = GetWidth(bindable);
        var on = width > 0;
        var effect = element.Effects.OfType<BorderEffect>().FirstOrDefault();
        if (on && effect is null)
        {
            element.Effects.Add(new BorderEffect());
        }
        else if (!on && effect is not null)
        {
            element.Effects.Remove(effect);
        }
    }
}

public sealed class BorderEffect : RoutingEffect
{
    public BorderEffect()
        : base("LegoDetect.BorderEffect")
    {
    }
}
