using System.Text.Json;
using Utils;

// TODO: set relative path (abs path is only for testing)
ConfigObject config = JsonSerializer.Deserialize<ConfigObject>(File.ReadAllText("D:\\Libraries\\Documents\\_Programming\\mark_yt_automation\\reciever-dotnet-2\\config.json")) ?? throw new Exception("Nincs config fájl.");
var playlist_id = config.PLAYLIST_ID;
var outputDirectory = config.OUTPUT_DIR;
var apiKey = config.YOUTUBE_API_KEY;

using var client = new HttpClient();

// Checks if playlist contains any videos, downloads them and removes them from the playlist
async Task MainLoop()
{
    var youtube = new YoutubeKeyClient(apiKey);

    // Fetch playlist items
    List<PlaylistItemResponse> playlist;
    try { playlist = await youtube.GetPlaylistItemsAsync(playlist_id); }
    catch (Exception ex)
    {
        Console.WriteLine($"Hiba lejátszási lista lekérése közben: {ex.Message}");
        return;
    }

    if (playlist.Count == 0)
    {
        Console.WriteLine("Lejátszási lista üres.");
        return;
    }

    // Download first video
    try
    {
        Console.WriteLine("Letöltés...");
        await VideoHandler.DownloadVideoAsync(playlist[0].VideoId, outputDirectory);
        Console.WriteLine("Letöltés kész.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Hiba letöltés közben: {ex.Message}");
        return;
    }
}

await MainLoop();