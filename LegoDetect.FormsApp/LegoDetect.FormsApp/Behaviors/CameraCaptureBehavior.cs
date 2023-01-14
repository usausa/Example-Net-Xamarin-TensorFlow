namespace LegoDetect.FormsApp.Behaviors;

using Smart.Forms.Interactivity;
using Smart.Forms.Messaging;

using LegoDetect.FormsApp.Messaging;

using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

public sealed class CameraCaptureBehavior : BehaviorBase<CameraView>
{
    public static readonly BindableProperty RequestProperty = BindableProperty.Create(
        nameof(Request),
        typeof(IEventRequest<CameraCaptureEventArgs>),
        typeof(CameraCaptureBehavior),
        propertyChanged: HandleRequestPropertyChanged);

    public IEventRequest<CameraCaptureEventArgs>? Request
    {
        get => (IEventRequest<CameraCaptureEventArgs>)GetValue(RequestProperty);
        set => SetValue(RequestProperty, value);
    }

    protected override void OnDetachingFrom(CameraView bindable)
    {
        if (Request is not null)
        {
            Request.Requested -= EventRequestOnRequested;
        }

        base.OnDetachingFrom(bindable);
    }

    private static void HandleRequestPropertyChanged(BindableObject bindable, object? oldValue, object? newValue)
    {
        ((CameraCaptureBehavior)bindable).OnMessengerPropertyChanged(oldValue as IEventRequest<CameraCaptureEventArgs>, newValue as IEventRequest<CameraCaptureEventArgs>);
    }

    private void OnMessengerPropertyChanged(IEventRequest<CameraCaptureEventArgs>? oldValue, IEventRequest<CameraCaptureEventArgs>? newValue)
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

    private void EventRequestOnRequested(object sender, CameraCaptureEventArgs ea)
    {
        void MediaCaptured(object s, MediaCapturedEventArgs e)
        {
            var camera = (CameraView)s;
            camera.MediaCaptured -= MediaCaptured;
            camera.MediaCaptureFailed -= MediaCaptureFailed;

            ea.CompletionSource.TrySetResult(e.ImageData ?? null);
        }

        void MediaCaptureFailed(object s, string e)
        {
            var camera = (CameraView)s;
            camera.MediaCaptured -= MediaCaptured;
            camera.MediaCaptureFailed -= MediaCaptureFailed;

            ea.CompletionSource.TrySetResult(null);
        }

        if (AssociatedObject is null)
        {
            return;
        }

        AssociatedObject.MediaCaptured += MediaCaptured;
        AssociatedObject.MediaCaptureFailed += MediaCaptureFailed;

        AssociatedObject.Shutter();
    }
}
