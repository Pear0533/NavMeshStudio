using Newtonsoft.Json.Linq;
using SoulsFormats;

namespace NavMeshStudio;

public class Cache
{
    public static StudioFile<NVA>? Nva;
    public static StudioFile<BND4>? NvmHktBnd;
    public static StudioFile<MSBE>? Msb;
    public static StudioViewer Viewer = new();
    public static JObject? NvmJson;
}