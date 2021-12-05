[assembly: Xamarin.Forms.ExportRenderer(typeof(Xamarin.Forms.Entry), typeof(LegoDetect.FormsApp.Droid.Renderers.PhysicalKeyEntryRenderer))]

namespace LegoDetect.FormsApp.Droid.Renderers;

using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

using LegoDetect.FormsApp.Droid.Helpers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

public class PhysicalKeyEntryRenderer : Xamarin.Forms.Platform.Android.EntryRenderer, TextView.IOnEditorActionListener
{
    public PhysicalKeyEntryRenderer(Context context)
        : base(context)
    {
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            EditText?.SetOnEditorActionListener(null);
        }

        base.Dispose(disposing);
    }

    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
        base.OnElementChanged(e);

        // Disable show input on focus
        EditText.ShowSoftInputOnFocus = false;

        // Select all
        EditText.SetSelectAllOnFocus(true);

        // Override editor action
        if (e.OldElement is not null)
        {
            EditText.SetOnEditorActionListener(null);
        }

        if (e.NewElement is not null)
        {
            EditText.SetOnEditorActionListener(this);
        }
    }

    bool TextView.IOnEditorActionListener.OnEditorAction(TextView? v, ImeAction actionId, KeyEvent? e)
    {
        if ((e is not null) && (e.KeyCode == Keycode.Enter) && (e.Action == KeyEventActions.Up))
        {
            ((IEntryController)Element).SendCompleted();
        }

        return true;
    }

    // [MEMO] EntryRenderer.OnFocusChangeRequested hack
    protected override void OnFocusChangeRequested(object sender, VisualElement.FocusRequestArgs e)
    {
        if (Control == null)
        {
            return;
        }

        e.Result = true;

        if (e.Focus)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Control == null || Control.IsDisposed())
                {
                    return;
                }

                Control?.RequestFocus();
            });
        }
        else
        {
            Control.ClearFocus();
        }
    }
}
