string url = "https://www.youtube.com/watch?v=rkjNL4dX-U4";
string outputDirectory = "output";

try
{
    Console.WriteLine("Letöltés...");
    await VideoHandler.DownloadVideoAsync(url, outputDirectory);
}
catch (Exception ex)
{
    Console.WriteLine($"Hiba: {ex.Message}");
    return;
}

Console.WriteLine("Videó letöltve.");