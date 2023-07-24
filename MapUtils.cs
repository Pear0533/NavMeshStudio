using SoulsFormats;

namespace NavMeshStudio;

public class MapUtils
{
    public static string MapBndsFolderPath = "";

    public static FLVER2 ReadFLVERFromBND(string bndFilePath)
    {
        BinderFile? flverBinderFile = BND4.Read(bndFilePath).Files.Find(i => i.Name.EndsWith(".flver"));
        return flverBinderFile == null ? new FLVER2() : FLVER2.Read(flverBinderFile.Bytes);
    }

    public static bool SetMapBndsFolderPath()
    {
        if (Cache.Msb == null) return false;
        string mapFolderPath = Utils.MoveUpDirectory(Cache.Msb.Path, 2);
        string[] mapDirectories = Directory.GetDirectories(mapFolderPath, Cache.Msb.Name, SearchOption.AllDirectories);
        MapBndsFolderPath = mapDirectories.ElementAtOrDefault(0) ?? "";
        return true;
    }

    public static void ReadMapPieceGeometry()
    {
        Cache.MapPieces = Directory.GetFiles(MapBndsFolderPath, "*.mapbnd.dcx").ToList();
    }
}