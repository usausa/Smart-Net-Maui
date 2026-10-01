namespace Smart.Maui.Internal;

internal static class PressEvents
{
    public static void Add(View view, EventHandler pressed, EventHandler? released)
    {
        if (view is Button button)
        {
            button.Pressed += pressed;
            button.Released += released;
        }
        else if (view is ImageButton imageButton)
        {
            imageButton.Pressed += pressed;
            imageButton.Released += released;
        }
    }

    public static void Remove(View view, EventHandler pressed, EventHandler? released)
    {
        if (view is Button button)
        {
            button.Pressed -= pressed;
            button.Released -= released;
        }
        else if (view is ImageButton imageButton)
        {
            imageButton.Pressed -= pressed;
            imageButton.Released -= released;
        }
    }
}
