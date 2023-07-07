using SoulsFormats;

namespace NavMeshStudio;

public class StudioFile<T> where T : SoulsFile<T>, new()
{
    public StudioFile(string path, string fileName, T data)
    {
        Path = path;
        FileName = fileName;
        Data = data;
    }

    public string Path { get; set; }
    public string FileName { get; set; }
    public T Data { get; set; }
}