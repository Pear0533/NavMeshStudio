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
        string mapName = Utils.RemoveFileExtensions(Path.GetFileName(Cache.Msb?.Path ?? ""));
        string[] mapDirectories = Utils.TryDirectoryGetFolders(mapFolderPath, Cache.Msb?.Name ?? "", SearchOption.AllDirectories);
        if (mapDirectories.Length != 0)
        {
            MapDependenciesPath = mapDirectories.ElementAtOrDefault(0) ?? "";
            if (Directory.Exists(MapDependenciesPath)) return true;
            Cache.Console.Write($"{mapName} has no scene data");
        }
        else Cache.Console.Write($"{mapName} isn't in the mapstudio folder");
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