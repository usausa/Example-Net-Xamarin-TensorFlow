namespace LegoDetect.FormsApp.Behaviors;

using System;
using System.ComponentModel;
using System.Linq;

using Smart.Forms.Interactivity;

using LegoDetect.FormsApp.Helpers;
using LegoDetect.FormsApp.Models.Entry;

using Xamarin.Forms;

public sealed class EntryBindBehavior : BehaviorBase<Entry>
{
    private bool updating;

    protected override void OnAttachedTo(Entry bindable)
    {
        base.OnAttachedTo(bindable);

        var controller = EntryBind.GetModel(bindable);
        bindable.Completed += BindableOnCompleted;
        bindable.TextChanged += BindableOnTextChanged;
        controller.FocusRequested += ControllerOnFocusRequested;
        controller.PropertyChanged += ControllerOnPropertyChanged;
    }

    protected override void OnDetachingFrom(Entry bindable)
    {
        var controller = EntryBind.GetModel(bindable);
        bindable.Completed -= BindableOnCompleted;
        bindable.TextChanged -= BindableOnTextChanged;
        controller.FocusRequested -= ControllerOnFocusRequested;
        controller.PropertyChanged -= ControllerOnPropertyChanged;

        base.OnDetachingFrom(bindable);
    }

    private void ControllerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        var entry = AssociatedObject;
        if (entry is null)
        {
            return;
        }

        if (e.PropertyName == nameof(EntryModel.Text))
        {
            var controller = EntryBind.GetModel(entry);
            updating = true;
            entry.Text = controller.Text;
            updating = false;
        }
        else if (e.PropertyName == nameof(EntryModel.Enable))
        {
            var controller = EntryBind.GetModel(entry);
            entry.IsEnabled = controller.Enable;
        }
    }

    private void ControllerOnFocusRequested(object sender, EventArgs e)
    {
        AssociatedObject?.Focus();
    }

    private void BindableOnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (updating)
        {
            return;
        }

        var entry = (Entry)sender;
        var controller = EntryBind.GetModel(entry);
        controller.Text = e.NewTextValue;
    }

    private static void BindableOnCompleted(object sender, EventArgs e)
    {
        var entry = (Entry)sender;
        var controller = EntryBind.GetModel(entry);
        var ice = new EntryCompleteEvent();
        controller.HandleCompleted(ice);
        if (!ice.HasError)
        {
            ElementHelper.MoveFocusInPage(entry, true);
        }
    }
}

public sealed class EntryBind
{
    public static readonly BindableProperty ModelProperty = BindableProperty.CreateAttached(
        "Model",
        typeof(LegoDetect.FormsApp.Models.Entry.IEntryController),
        typeof(EntryBind),
        null,
        propertyChanged: BindChanged);

    public static LegoDetect.FormsApp.Models.Entry.IEntryController GetModel(BindableObject bindable) =>
        (LegoDetect.FormsApp.Models.Entry.IEntryController)bindable.GetValue(ModelProperty);

    public static void SetModel(BindableObject bindable, LegoDetect.FormsApp.Models.Entry.IEntryController value) =>
        bindable.SetValue(ModelProperty, value);

    private static void BindChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        if (bindable is not Entry entry)
        {
            return;
        }

        if (oldValue is not null)
        {
            var behavior = entry.Behaviors.FirstOrDefault(x => x is EntryBindBehavior);
            if (behavior is not null)
            {
                entry.Behaviors.Remove(behavior);
            }
        }

        if (newValue is not null)
        {
            entry.Behaviors.Add(new EntryBindBehavior());
        }
    }
}
