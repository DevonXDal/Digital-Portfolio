namespace Portfolio.Helpers;
/// ----- THIS FILE WAS TAKEN FROM THE VELUSIA SAMPLE PROJECT WHICH WAS LICENSED UNDER THE APACHE 2.0 LICENSE -----
public static class AsyncEnumerableExtensions
{
    public static Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return ExecuteAsync();

        async Task<List<T>> ExecuteAsync()
        {
            var list = new List<T>();

            await foreach (var element in source)
            {
                list.Add(element);
            }

            return list;
        }
    }
}
