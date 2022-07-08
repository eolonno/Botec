namespace Botec.CommandProcessor.Utilities.Extensions;

public static class EnumerableExtensions
{
    public static T GetRandomElement<T>(this IEnumerable<T> list)
    {
        var random = new Random();
        return list.ElementAt(random.Next(0, list.Count()));
    }
}