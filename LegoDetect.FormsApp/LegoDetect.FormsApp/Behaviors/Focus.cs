namespace LegoDetect.FormsApp.Behaviors;

using Xamarin.Forms;

public static class Focus
{
    public static readonly BindableProperty DefaultProperty = BindableProperty.CreateAttached(
        "Default",
        typeof(bool),
        typeof(Focus),
        false);

    public static bool GetDefault(BindableObject bindable) => (bool)bindable.GetValue(DefaultProperty);

    public static void SetDefault(BindableObject bindable, bool value) => bindable.SetValue(DefaultProperty, value);
}
