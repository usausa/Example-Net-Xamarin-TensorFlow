namespace LegoDetect.FormsApp.Input;

using System.Linq;

using Smart.Forms.Interactivity;

using LegoDetect.FormsApp.Helpers;

using Xamarin.Forms;

public class InputControlBehavior : BehaviorBase<Page>, IInputHandler
{
    protected override void OnAttachedTo(Page bindable)
    {
        base.OnAttachedTo(bindable);

        InputManager.Default.PushHandler(this);
    }

    protected override void OnDetachingFrom(Page bindable)
    {
        InputManager.Default.PopHandler(this);

        base.OnDetachingFrom(bindable);
    }

    public bool Handle(KeyCode key)
    {
        if ((AssociatedObject is null) || !AssociatedObject.IsEnabled)
        {
            return false;
        }

        var focused = FindFocused();
        if (focused is not null)
        {
            if (focused.Behaviors.OfType<IShortcutBehavior>().Any(x => x.Handle(key)))
            {
                return true;
            }
        }

        if (key == KeyCode.Up)
        {
            ElementHelper.MoveFocus(AssociatedObject, false);
            return true;
        }

        if (key == KeyCode.Down)
        {
            ElementHelper.MoveFocus(AssociatedObject, true);
            return true;
        }

        var button = (Button?)ElementHelper.EnumerateActive(AssociatedObject)
            .FirstOrDefault(x => x is Button b && Shortcut.GetKey(b) == key);
        if (button is not null)
        {
            button.SendClicked();
            return true;
        }

        return false;
    }

    public VisualElement? FindFocused()
    {
        return AssociatedObject is not null ? ElementHelper.FindFocused(AssociatedObject) : null;
    }
}
