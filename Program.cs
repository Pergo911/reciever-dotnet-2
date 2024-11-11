using System.Diagnostics;
using System.Text.Json;
using Utils;

ConfigObject config = JsonSerializer.Deserialize<ConfigObject>(File.ReadAllText("config.json")) ?? throw new Exception("Nincs config fájl.");
var playlist_id = config.PLAYLIST_ID;
var outputDirectory = config.OUTPUT_DIR;
var apiKey = config.YOUTUBE_API_KEY;
var clientId = config.CLIENT_ID;
var clientSecret = config.CLIENT_SECRET;
var redirectUri = config.REDIRECT_URI;

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

// await MainLoop();

var ACCESS_TOKEN = await OAuth.GetAccessTokenAsync(clientId, clientSecret, redirectUri);
Console.WriteLine($"Access token: {ACCESS_TOKEN}");