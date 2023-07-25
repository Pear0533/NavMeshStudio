using SoulsFormats;

namespace NavMeshStudio;

public class BXF4File : StudioFile
{
    public BXF4File(string path) : base(path)
    {
        Data = BXF4.Read(path, path.Replace("bhd", "bdt"));
    }

    public BXF4 Data { get; set; }
}