using SoulsFormats;

namespace NavMeshStudio;

public static class MapUtils
{
    public static string MapDependenciesPath = "";

    public static FLVER2 ReadFLVERFromBND(string bndFilePath)
    {
        BinderFile? flverBinderFile = BND4.Read(bndFilePath).Files.Find(i => i.Name.EndsWith(".flver"));
        return flverBinderFile == null ? new FLVER2() : FLVER2.Read(flverBinderFile.Bytes);
    }

    public static bool SetMapDependenciesPath(this NavMeshStudio studio)
    {
        string mapFolderPath = Utils.MoveUpDirectory(Cache.Msb?.Path ?? "", 2);
        string[] mapDirectories = Directory.GetDirectories(mapFolderPath, Cache.Msb?.Name ?? "", SearchOption.AllDirectories);
        MapDependenciesPath = mapDirectories.ElementAtOrDefault(0) ?? "";
        if (Directory.Exists(MapDependenciesPath)) return true;
        Cache.Console.Write($"{Utils.RemoveFileExtensions(Path.GetFileName(Cache.Msb?.Path ?? ""))} has no scene data");
        studio.ResetStatus();
        return false;
    }

    public static T? GetDependencyFile<T>(string fileName) where T : StudioFile
    {
        if (string.IsNullOrEmpty(MapDependenciesPath)) return null;
        string? filePath = Directory.GetFiles(MapDependenciesPath, fileName).ElementAtOrDefault(0);
        if (filePath == null) return null;
        return (T)Activator.CreateInstance(typeof(T), filePath)!;
    }

    public static void ReadMapPieceGeometry()
    {
        Cache.MapPieces = Directory.GetFiles(MapDependenciesPath, "*.mapbnd.dcx").ToList();
    }
}