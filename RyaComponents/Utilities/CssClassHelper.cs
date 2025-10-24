namespace RyaComponents.Utilities
{
    public class CssClassHelper
    {
        public static string CombineCssClasses(params string?[] cssClasses)
        {
            return string.Join(' ', cssClasses);
        }
    }
}
