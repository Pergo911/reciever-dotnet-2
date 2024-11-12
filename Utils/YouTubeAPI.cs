using System.Net;
using System.Text.Json;

namespace Utils;

public static class YoutubeRequests
{

  public static async Task<List<PlaylistItem>> GetPlaylistItemsAsync(string playlistId, string accessToken, HttpClient httpClient)
  {
    var req = new HttpRequestMessage
    {
      Method = HttpMethod.Get,
      RequestUri = new Uri($"https://www.googleapis.com/youtube/v3/playlistItems?part=contentDetails,snippet&playlistId={playlistId}&maxResults=50"),
      Headers = {
       {HttpRequestHeader.Authorization.ToString(), $"Bearer {accessToken}"},
      }
    };

    var res = await httpClient.SendAsync(req);

    if (!res.IsSuccessStatusCode)
    {
      throw new Exception($"Hiba a lekérdezés során: {res.StatusCode}\n{await res.Content.ReadAsStringAsync()}");
    }

    var response = JsonSerializer.Deserialize<PlaylistResponse>(await res.Content.ReadAsStringAsync()) ?? throw new Exception("Hiba a JSON feldolgozás során.");

    return response.items.Select(item => new PlaylistItem
    {
      id = item.id,
      videoId = item.contentDetails.videoId,
      videoTitle = item.snippet.title
    }).ToList();
  }

  public static async Task DeletePlaylistItemAsync(string id, string accessToken, HttpClient httpClient)
  {
    var req = new HttpRequestMessage
    {
      Method = HttpMethod.Delete,
      RequestUri = new Uri($"https://www.googleapis.com/youtube/v3/playlistItems?id={id}"),
      Headers = {
       {HttpRequestHeader.Authorization.ToString(), $"Bearer {accessToken}"},
      }
    };

    var res = await httpClient.SendAsync(req);

    if (!res.IsSuccessStatusCode)
    {
      throw new Exception($"Hiba lejátszási lista elem törlés során: {res.StatusCode}\n{await res.Content.ReadAsStringAsync()}");
    }
  }
}