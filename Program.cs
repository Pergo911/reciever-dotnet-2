using System.Text.Json;

ConfigObject config = JsonSerializer.Deserialize<ConfigObject>(File.ReadAllText("D:\\Libraries\\Documents\\_Programming\\mark_yt_automation\\reciever-dotnet-2\\config.json")) ?? throw new Exception("Nincs config fájl.");
var playlist_id = config.PLAYLIST_ID;
var outputDirectory = config.OUTPUT_DIR;
var apiKey = config.YOUTUBE_API_KEY;

using var client = new HttpClient();

// Checks if playlist contains any videos, downloads them and tags them
async Task MainLoop()
{
    var url = $"https://www.googleapis.com/youtube/v3/playlistItems?part=contentDetails&maxResults=50&playlistId={playlist_id}&key={apiKey}";

    var response = await client.GetAsync(url);

    if (!response.IsSuccessStatusCode)
    {
        throw new Exception($"Failed to fetch playlist: {response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");
    }

    var json = await response.Content.ReadAsStringAsync();

    var playlist = JsonSerializer.Deserialize<Playlist>(json) ?? throw new Exception("Failed to parse JSON.");

    if (playlist.items.Count == 0)
    {
        // TODO: remove to prevent spam
        Console.WriteLine("No videos found in playlist.");
        return;
    }

    foreach (var item in playlist.items)
    {
        Console.WriteLine(item.contentDetails.videoId);
    }
}

await MainLoop();