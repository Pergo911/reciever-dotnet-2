using System.Text.Json;
using Utils;

ConfigObject config = JsonSerializer.Deserialize<ConfigObject>(File.ReadAllText("config.json")) ?? throw new Exception("Nincs config fájl.");
var playlist_id = config.PLAYLIST_ID;
var outputDirectory = config.OUTPUT_DIR;
var clientId = config.CLIENT_ID;
var clientSecret = config.CLIENT_SECRET;
var redirectUri = config.REDIRECT_URI;

var httpClient = new HttpClient();

// Checks if playlist contains any videos, downloads them and removes them from the playlist
async Task MainLoop()
{
    // Get access token
    string accessToken = await OAuth.GetAccessTokenAsync(clientId, clientSecret, redirectUri);

    // Get playlist items
    var playlist = await YoutubeRequests.GetPlaylistItemsAsync(playlist_id, accessToken, httpClient);

    if (playlist.Count == 0)
    {
        return;
    }

    // We address only the first video, the rest will be handled in the next iteration
    var playlistItem = playlist[0];

    // Download video
    try
    {
        Console.WriteLine(playlistItem.videoTitle);
        Console.WriteLine("Letöltés...");
        await VideoHandler.DownloadVideoAsync(playlistItem.videoId, outputDirectory);
        Console.WriteLine("Letöltés kész.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Hiba letöltés közben: {ex.Message}");
        return;
    }

    // Remove downloaded video from playlist
    try
    {
        Console.WriteLine("Törlés...");
        await YoutubeRequests.DeletePlaylistItemAsync(playlistItem.id, accessToken, httpClient);
        Console.WriteLine("Törlés kész.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Hiba törlés közben: {ex.Message}");
        return;
    }
}

var timer = new PeriodicTimer(TimeSpan.FromSeconds(10));

while (await timer.WaitForNextTickAsync())
{
    await MainLoop();
}