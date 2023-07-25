using HKLib.hk2018;
using HKLib.Serialization.hk2018.Binary;
using SoulsFormats;

namespace NavMeshStudio;

public class CollisionUtils
{
    public static async Task ReadCollisionGeometry(NavMeshStudio studio)
    {
        Cache.LHkxBhd = MapUtils.GetDependencyFile<BXF4File>($"l{Cache.Msb?.ID}.hkxbhd");
        Cache.HHkxBhd = MapUtils.GetDependencyFile<BXF4File>($"h{Cache.Msb?.ID}.hkxbhd");
        studio.UpdateStatus("Reading collision geometry...");
        await Task.Run(() =>
        {
            HavokBinarySerializer serializer = new();
            serializer.SetCompendium(studio, Cache.LHkxBhd);
            List<BinderFile> hkxFiles = Cache.LHkxBhd.Data.Files.Where(i => i.Name.EndsWith(".hkx.dcx")).ToList();
            foreach (BinderFile hkxFile in hkxFiles)
            {
                byte[] hkxBytes = DCX.Decompress(hkxFile.Bytes);
                hkRootLevelContainer container = serializer.GetRootLevelContainer(hkxBytes);
                System.Console.WriteLine(container);
            }
        });
    }
}