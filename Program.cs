using System.Text.Json;
using Utils;

ConfigObject config = JsonSerializer.Deserialize<ConfigObject>(File.ReadAllText("config.json")) ?? throw new Exception("Nincs config fájl.");
var playlist_id = config.PLAYLIST_ID;
var outputDirectory = config.OUTPUT_DIR;
var clientId = config.CLIENT_ID;
var clientSecret = config.CLIENT_SECRET;
var redirectUri = config.REDIRECT_URI;
var pollingInterval = config.POLLING_INTERVAL;

var httpClient = new HttpClient();

var blockList = new List<string>();

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

    // We address only the first video (excluding the blocklist), the rest will be handled in the next iteration
    var playlistItem = playlist.FirstOrDefault(item => !blockList.Contains(item.videoId));
    if (playlistItem == null) return;

    Console.WriteLine("\n" + playlistItem.videoTitle);

    // Download video
    try
    {
        Console.Write("Letöltés... ");
        await VideoHandler.DownloadVideoAsync(playlistItem.videoId, outputDirectory);
        Console.WriteLine("Kész.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Hiba letöltés közben: {ex.Message}");

        // Add video to blocklist to prevent further download attempts
        blockList.Add(playlistItem.videoId);
        return;
    }

    // Remove downloaded video from playlist
    try
    {
        Console.Write("Eltávolítás a listából... ");
        await YoutubeRequests.DeletePlaylistItemAsync(playlistItem.id, accessToken, httpClient);
        Console.WriteLine("Kész.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Hiba törlés közben: {ex.Message}");

        // Add video to blocklist to prevent further download attempts
        blockList.Add(playlistItem.videoId);
        return;
    }
}

var timer = new PeriodicTimer(TimeSpan.FromSeconds(pollingInterval));

while (await timer.WaitForNextTickAsync())
{
    await MainLoop();
}