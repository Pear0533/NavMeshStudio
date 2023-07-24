using SoulsFormats;

namespace NavMeshStudio;

public class StudioFile<T> where T : SoulsFile<T>, new()
{
    public StudioFile(string path)
    {
        Path = path;
        FileName = System.IO.Path.GetFileName(Path);
        Name = Utils.RemoveFileExtensions(FileName);
        Tokens = Name.Split('_');
        Prefix = Tokens[0][..1];
        ID = Tokens[0][1..] + '_' + string.Join('_', Tokens.Skip(1));
        Data = SoulsFile<T>.Read(path);
    }

    public string Path { get; set; }
    public string FileName { get; set; }
    public string Name { get; set; }
    public string[] Tokens { get; set; }
    public string Prefix { get; set; }
    public string ID { get; set; }
    public T Data { get; set; }
}