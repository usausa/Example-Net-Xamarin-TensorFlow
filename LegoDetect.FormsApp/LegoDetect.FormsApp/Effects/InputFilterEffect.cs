namespace LegoDetect.FormsApp.Effects;

using System;
using System.Linq;

using Xamarin.Forms;

public sealed class InputFilterEffect : RoutingEffect
{
    public static readonly BindableProperty RuleProperty = BindableProperty.CreateAttached(
        "Rule",
        typeof(Func<string, bool>),
        typeof(InputFilterEffect),
        null,
        propertyChanged: OnChanged);

    public static Func<string, bool>? GetRule(BindableObject bindable) => (Func<string, bool>?)bindable.GetValue(RuleProperty);

    public static void SetRule(BindableObject bindable, Func<string, bool?> value) => bindable.SetValue(RuleProperty, value);

    private static void OnChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is not Element element)
        {
            return;
        }

        if (oldValue is not null)
        {
            var effect = element.Effects.FirstOrDefault(x => x is InputFilterEffect);
            if (effect != null)
            {
                element.Effects.Remove(effect);
            }
        }

        if (newValue is not null)
        {
            element.Effects.Add(new InputFilterEffect());
        }
    }

    public InputFilterEffect()
        : base("LegoDetect.InputFilterEffect")
    {
    }
}
