public class ConfigObject
{
  public required string PLAYLIST_URL { get; set; }
  public required string OUTPUT_DIR { get; set; }
  public required string YOUTUBE_API_KEY { get; set; }
};

public class Root
{
  public required ConfigObject ConfigObject { get; set; }
};
