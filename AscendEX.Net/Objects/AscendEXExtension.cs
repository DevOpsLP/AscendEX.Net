using System.Reflection;

public static class ObjectExtensions
{
    public static Dictionary<string, object> ToDictionary(this object source)
    {
        return source.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .ToDictionary(prop => prop.Name, prop => prop.GetValue(source, null));
    }
}
