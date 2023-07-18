using SoulsFormats;

namespace NavMeshStudio;

public class MapUtils
{
    public static FLVER2 ReadFLVERFromBND(string bndFilePath)
    {
        BinderFile? flverBinderFile = BND4.Read(bndFilePath).Files.Find(i => i.Name.EndsWith(".flver"));
        return flverBinderFile == null ? new FLVER2() : FLVER2.Read(flverBinderFile.Bytes);
    }

    public static void ReadMapPieceGeometry()
    {
        string mapBndsFolderPath = Path.GetDirectoryName(Cache.NvmHktBnd?.Path) ?? "";
        Cache.MapPieces = Directory.GetFiles(mapBndsFolderPath, "*.mapbnd.dcx").ToList();
    }
}