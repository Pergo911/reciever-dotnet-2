// JSON config file
public class ConfigObject
{
  public required string PLAYLIST_ID { get; set; }
  public required string OUTPUT_DIR { get; set; }
  public required string YOUTUBE_API_KEY { get; set; }

  public required string CLIENT_ID { get; set; }

  public required string CLIENT_SECRET { get; set; }

  public required string REDIRECT_URI { get; set; }
}

// JSON response from YouTube API
public class Playlist
{
  public required List<PlaylistItem> items { get; set; }
}
public class PlaylistItem
{
  public required string id { get; set; }
  public required ContentDetails contentDetails { get; set; }
}
public class ContentDetails
{
  public required string videoId { get; set; }
}

public class PlaylistItemResponse
{
  public required string Id { get; set; }
  public required string VideoId { get; set; }
}

// Required for OAuth2
public class TokenObject {
  public required string accessToken {get; set;}
  public required string refreshToken {get; set;}
  public required DateTime expiration {get; set;}
}
public class TokenExchangeResponse {
  public required string access_token {get; set;}
  public required int expires_in {get; set;}
  public required string refresh_token {get; set;}
}

public class TokenRefreshResponse {
  public required string access_token {get; set;}
  public required int expires_in {get; set;}
}