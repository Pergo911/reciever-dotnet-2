using System.Text.Json;

namespace Utils;

public class YoutubeKeyClient
{
  public YoutubeKeyClient(string apiKey)
  {
    ApiKey = apiKey;
    Client.BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/");
  }

  public string ApiKey { get; set; }

  private HttpClient Client { get; set; } = new HttpClient();

  public async Task<List<PlaylistItemResponse>> GetPlaylistItemsAsync(string playlistId)
  {
    var url = $"playlistItems?part=contentDetails&maxResults=50&playlistId={playlistId}&key={ApiKey}";

    var response = await Client.GetAsync(url);

    if (!response.IsSuccessStatusCode)
    {
      throw new Exception($"{response.StatusCode}\n{await response.Content.ReadAsStringAsync()}");
    }

    var json = await response.Content.ReadAsStringAsync();

    var playlist = JsonSerializer.Deserialize<Playlist>(json) ?? throw new Exception("Lejátszási lista JSON hibás.");

    if (playlist.items.Count == 0)
    {
      return [];
    }

    return playlist.items.Select(item => new PlaylistItemResponse
    {
      Id = item.id,
      VideoId = item.contentDetails.videoId
    }).ToList();
  }
}

public static class YoutubeOAuthClient
{
  /// ...
}