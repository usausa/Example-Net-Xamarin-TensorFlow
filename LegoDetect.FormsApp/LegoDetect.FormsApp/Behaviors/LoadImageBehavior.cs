namespace LegoDetect.FormsApp.Behaviors;

using LegoDetect.FormsApp.Controls;

using Smart;
using Smart.Forms.Interactivity;
using Smart.Forms.Messaging;

using Xamarin.Forms;

public sealed class LoadImageBehavior : BehaviorBase<DetectCanvasView>
{
    public static readonly BindableProperty RequestProperty = BindableProperty.Create(
        nameof(Request),
        typeof(IEventRequest<EventArgs<byte[]>>),
        typeof(LoadImageBehavior),
        null,
        propertyChanged: HandleRequestPropertyChanged);

    public IEventRequest<EventArgs<byte[]>>? Request
    {
        get => (IEventRequest<EventArgs<byte[]>>)GetValue(RequestProperty);
        set => SetValue(RequestProperty, value);
    }

    protected override void OnDetachingFrom(DetectCanvasView bindable)
    {
        if (Request is not null)
        {
            Request.Requested -= EventRequestOnRequested;
        }

        base.OnDetachingFrom(bindable);
    }

    private static void HandleRequestPropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        ((LoadImageBehavior)bindable).OnRequestPropertyChanged(oldValue as IEventRequest<EventArgs<byte[]>>, newValue as IEventRequest<EventArgs<byte[]>>);
    }

    private void OnRequestPropertyChanged(IEventRequest<EventArgs<byte[]>>? oldValue, IEventRequest<EventArgs<byte[]>>? newValue)
    {
        if (oldValue == newValue)
        {
            return;
        }

        if (oldValue is not null)
        {
            oldValue.Requested -= EventRequestOnRequested;
        }

        if (newValue is not null)
        {
            newValue.Requested += EventRequestOnRequested;
        }
    }

    private void EventRequestOnRequested(object sender, EventArgs<byte[]> ea)
    {
        AssociatedObject?.Load(ea.Data);
    }
}
