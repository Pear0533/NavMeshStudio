using HKLib.hk2018;
using SoulsFormats;

namespace NavMeshStudio;

public class Cache
{
    public static StudioFile<NVA>? Nva;
    public static StudioFile<BND4>? NvmHktBnd;
    public static StudioFile<MSBE>? Msb;
    public static SceneGraph SceneGraph = new();
    public static Viewer Viewer = new();
    public static Console Console = new();
    public static List<hkaiNavMesh> NavMeshes = new();
    public static List<string> MapPieces = new();
    public static List<hkReferencedObject> QueryMediators = new();
    public static List<hkReferencedObject> UserEdgeSetups = new();

    public static void Clear()
    {
        NavMeshes.Clear();
        QueryMediators.Clear();
        UserEdgeSetups.Clear();
    }
}