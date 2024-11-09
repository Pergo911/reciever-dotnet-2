// JSON config file
public class ConfigObject
{
  public required string PLAYLIST_ID { get; set; }
  public required string OUTPUT_DIR { get; set; }
  public required string YOUTUBE_API_KEY { get; set; }
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