using SoulsFormats;

namespace NavMeshStudio;

public class MapUtils
{
    public static async Task ReadMapPieces()
    {
        string mapBndsFolderPath = Path.GetDirectoryName(Cache.NvmHktBnd?.Path) ?? "";
        List<string> mapBndFilePaths = Directory.GetFiles(mapBndsFolderPath, "*.mapbnd.dcx").ToList();
        await Task.Run(() =>
        {
            mapBndFilePaths.ForEach(i =>
            {
                BinderFile? flverBinderFile = BND4.Read(i).Files.Find(x => x.Name.EndsWith(".flver"));
                if (flverBinderFile == null) return;
                Cache.MapPieces.Add(FLVER2.Read(flverBinderFile.Bytes));
            });
        });
    }
}