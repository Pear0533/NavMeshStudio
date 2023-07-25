using SoulsFormats;

namespace NavMeshStudio;

public class FromSoftFile<T> : StudioFile where T : SoulsFile<T>, new()
{
    public FromSoftFile(string path) : base(path)
    {
        Data = SoulsFile<T>.Read(path);
    }

    public T Data { get; set; }
}