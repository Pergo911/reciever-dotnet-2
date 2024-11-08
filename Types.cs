public class ConfigObject
{
  public required string PLAYLIST_ID { get; set; }
  public required string OUTPUT_DIR { get; set; }
  public required string YOUTUBE_API_KEY { get; set; }
}

public class Playlist
{
  public required List<PlaylistItem> items { get; set; }
}

public class PlaylistItem
{
  public required ContentDetails contentDetails { get; set; }
}

public class ContentDetails
{
  public required string videoId { get; set; }
}