using System.Text.Json;

ConfigObject config = JsonSerializer.Deserialize<ConfigObject>(File.ReadAllText("config.json")) ?? throw new Exception("Nincs config fájl.");
var playlist_id = config.PLAYLIST_ID;
var outputDirectory = config.OUTPUT_DIR;
var apiKey = config.YOUTUBE_API_KEY;

// Checks if playlist contains any videos, downloads them and tags them
async void MainLoop()
{
    var url = $"https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&maxResults=50&playlistId={playlist_id}&key={apiKey}";
}