namespace NavMeshStudio;

public class CollisionUtils
{
    public static void ReadCollisionGeometry()
    {
        Cache.LHkxBhd = MapUtils.GetDependencyFile<BXF4File>($"l{Cache.Msb?.ID}.hkxbhd");
        Cache.HHkxBhd = MapUtils.GetDependencyFile<BXF4File>($"h{Cache.Msb?.ID}.hkxbhd");
    }
}