using System.Text.Json;

Root config = JsonSerializer.Deserialize<Root>(File.ReadAllText("config.json")) ?? throw new Exception("Nincs config fájl.");

var url = config.ConfigObject.PLAYLIST_URL;

Console.WriteLine($"URL: {url}");

// try
// {
//     Console.WriteLine("Letöltés...");
//     await VideoHandler.DownloadVideoAsync(url, outputDirectory);
// }
// catch (Exception ex)
// {
//     Console.WriteLine($"Hiba: {ex.Message}");
//     return;
// }

// Console.WriteLine("Videó letöltve.");