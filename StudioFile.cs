using SoulsFormats;

namespace NavMeshStudio;

public class StudioFile<T> where T : SoulsFile<T>, new()
{
    public StudioFile(string path, T data)
    {
        Path = path;
        Data = data;
    }

    public string Path { get; set; }
    public T Data { get; set; }
}