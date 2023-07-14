using HKLib.hk2018;
using Newtonsoft.Json.Linq;
using SoulsFormats;

namespace NavMeshStudio;

public class Cache
{
    public static StudioFile<NVA>? Nva;
    public static StudioFile<BND4>? NvmHktBnd;
    public static StudioFile<MSBE>? Msb;
    public static JObject? NvmJson;
    public static Viewer Viewer = new();
    public static List<hkaiNavMesh> NavMeshes = new();
    public static List<hkReferencedObject> QueryMediators = new();
    public static List<hkReferencedObject> UserEdgeSetups = new();

    public static void Clear()
    {
        NavMeshes.Clear();
        QueryMediators.Clear();
        UserEdgeSetups.Clear();
    }
}