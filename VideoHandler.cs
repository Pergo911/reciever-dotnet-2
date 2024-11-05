// https://github.com/Tyrrrz/YoutubeExplode/
// https://github.com/Tyrrrz/YoutubeDownloader/ (Utils/Tagging és Utils/YoutubeDownloader.Core mappák)
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode.Videos;
using YoutubeExplode.Converter;
using YoutubeDownloader.Core.Tagging;
using YoutubeDownloader.Core.Utils;

public static class VideoHandler
{
    public static async Task DownloadVideoAsync(string url, string outputDirectory)
    {
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        if (url.Contains("playlist")) { throw new InvalidDataException("URL egy lejátszási lista"); }

        var youtube = new YoutubeClient();
        Video video = await youtube.Videos.GetAsync(url);

        string path = Path.Combine(outputDirectory, PathEx.EscapeFileName(video.Title) + ".mp3");

        await youtube.Videos.DownloadAsync(url, path, o => o
            .SetContainer(Container.Mp3)
            .SetPreset(ConversionPreset.Medium)
            .SetFFmpegPath("ffmpeg.exe")
        );

        MediaTagInjector tagInjector = new();
        await tagInjector.InjectTagsAsync(path, video);
    }
}