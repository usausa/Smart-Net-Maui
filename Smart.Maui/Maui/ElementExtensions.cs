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

            if (parent is T typedParent)
            {
                return typedParent;
            }

            element = parent;
        }
    }

    public static T? FindParent<T>(this Element element, Func<T, bool> predicate)
        where T : Element =>
        element.FindParent<T, Func<T, bool>>(predicate, static (x, p) => p(x));

    public static T? FindParent<T, TState>(this Element element, TState state, Func<T, TState, bool> predicate)
        where T : Element
    {
        var parent = element.Parent;
        while (parent is not null)
        {
            if ((parent is T typedParent) && predicate(typedParent, state))
            {
                return typedParent;
            }

            parent = parent.Parent;
        }

        return null;
    }

    public static T? FindParentOrSelf<T>(this Element element)
        where T : Element =>
        element as T ?? element.FindParent<T>();

    public static T? FindParentOrSelf<T>(this Element element, Func<T, bool> predicate)
        where T : Element =>
        (element is T self) && predicate(self) ? self : element.FindParent(predicate);

    public static T? FindParentOrSelf<T, TState>(this Element element, TState state, Func<T, TState, bool> predicate)
        where T : Element =>
        (element is T self) && predicate(self, state) ? self : element.FindParent(state, predicate);

    // ------------------------------------------------------------
    // Ancestry
    // ------------------------------------------------------------

    public static bool IsDescendantOf(this Element element, Element ancestor)
    {
        var current = element.Parent;
        while (current is not null)
        {
            if (current == ancestor)
            {
                return true;
            }

            current = current.Parent;
        }

        return false;
    }
}
