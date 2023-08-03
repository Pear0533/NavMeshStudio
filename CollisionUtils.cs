using HKLib.hk2018;
using HKLib.Serialization.hk2018.Binary;
using SoulsFormats;

namespace NavMeshStudio;

public class CollisionUtils
{
    private static void AddCollisionsFromBXF4(NavMeshStudio studio, BXF4File? bxf4)
    {
        if (bxf4 == null) return;
        studio.UpdateStatus("Reading collision geometry...");
        HavokBinarySerializer serializer = new();
        serializer.SetCompendium(studio, bxf4);
        List<BinderFile> hkxFiles = bxf4.Data.Files.Where(i => i.Name.EndsWith(".hkx.dcx")).ToList();
        foreach (BinderFile hkxFile in hkxFiles)
        {
            byte[] hkxBytes = DCX.Decompress(hkxFile.Bytes);
            hkRootLevelContainer container = serializer.GetRootLevelContainer(hkxBytes);
            hknpBodyCinfo collision = container.GetBodyCollisionInfo(0);
            Cache.Collisions.Add(collision);
        }
    }

    public static async Task ReadCollisionGeometry(NavMeshStudio studio)
    {
        Cache.LHkxBhd = MapUtils.GetDependencyFile<BXF4File>($"l{Cache.Msb?.ID}.hkxbhd");
        Cache.HHkxBhd = MapUtils.GetDependencyFile<BXF4File>($"h{Cache.Msb?.ID}.hkxbhd");
        if (Cache.LHkxBhd == null) await studio.UpdateStatus("This map doesn't contain l_collision, skipping...", 1000);
        if (Cache.HHkxBhd == null) await studio.UpdateStatus("This map doesn't contain h_collision, skipping...", 1000);
        await Task.Run(() =>
        {
            AddCollisionsFromBXF4(studio, Cache.LHkxBhd);
            AddCollisionsFromBXF4(studio, Cache.HHkxBhd);
        });
    }
}