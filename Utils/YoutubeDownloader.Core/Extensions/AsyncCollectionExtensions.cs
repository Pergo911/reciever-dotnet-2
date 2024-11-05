using System.Runtime.CompilerServices;

namespace YoutubeDownloader.Core.Utils.Extensions;

public static class AsyncCollectionExtensions
{
    private static async ValueTask<IReadOnlyList<T>> CollectAsync<T>(
        this IAsyncEnumerable<T> asyncEnumerable
    )
    {
        var list = new List<T>();

        await foreach (var i in asyncEnumerable)
            list.Add(i);

        return list;
    }

    public static ValueTaskAwaiter<IReadOnlyList<T>> GetAwaiter<T>(
        this IAsyncEnumerable<T> asyncEnumerable
    ) => asyncEnumerable.CollectAsync().GetAwaiter();
}
