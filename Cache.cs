using HKLib.hk2018;
using SoulsFormats;

namespace NavMeshStudio;

public class Cache
{
    public static FromSoftFile<NVA>? Nva;
    public static FromSoftFile<BND4>? NvmHktBnd;
    public static BXF4File? LHkxBhd;
    public static BXF4File? HHkxBhd;
    public static FromSoftFile<MSBE>? Msb;
    public static SceneGraph SceneGraph = new();
    public static Viewer Viewer = new();
    public static Console Console = new();
    public static List<hkaiNavMesh> NavMeshes = new();
    public static List<hknpBodyCinfo> Collisions = new();
    public static List<string> MapPieces = new();
    public static List<hkReferencedObject> QueryMediators = new();
    public static List<hkReferencedObject> UserEdgeSetups = new();

    public static void Clear()
    {
        NavMeshes.Clear();
        Collisions.Clear();
        MapPieces.Clear();
        QueryMediators.Clear();
        UserEdgeSetups.Clear();
    }
}