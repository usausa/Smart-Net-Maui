namespace Smart.Maui;

public static class ElementExtensions
{
    // ------------------------------------------------------------
    // Parent
    // ------------------------------------------------------------

    public static T? FindParent<T>(this Element element)
        where T : Element
    {
        while (true)
        {
            var parent = element.Parent;
            if (parent is null)
            {
                return null;
            }

            if (element is T variable)
            {
                return variable;
            }

            element = parent;
        }
    }
}
