// JSON config file
public class ConfigObject
{
  public required string PLAYLIST_ID { get; set; }
  public required string OUTPUT_DIR { get; set; }
  public required string CLIENT_ID { get; set; }
  public required string CLIENT_SECRET { get; set; }
  public required string REDIRECT_URI { get; set; }
}

// JSON response from YouTube API
public class PlaylistResponse
{
  public required List<PlaylistItemResponse> items { get; set; }
}
public class PlaylistItemResponse
{
  public required string id { get; set; }
  public required ContentDetailsResponse contentDetails { get; set; }
  public required SnippetResponse snippet { get; set; }
}
public class ContentDetailsResponse
{
  public required string videoId { get; set; }
}

public class SnippetResponse
{
  public required string title { get; set; }
}

public class PlaylistItem
{
  public required string id { get; set; }
  public required string videoId { get; set; }
  public required string videoTitle { get; set; }
}

// OAuth2
public class TokenObject
{
  public required string accessToken { get; set; }
  public required string refreshToken { get; set; }
  public required DateTime expiration { get; set; }
}
public class TokenExchangeResponse
{
  public required string access_token { get; set; }
  public required int expires_in { get; set; }
  public required string refresh_token { get; set; }
}
public class TokenRefreshResponse
{
  public required string access_token { get; set; }
  public required int expires_in { get; set; }
}