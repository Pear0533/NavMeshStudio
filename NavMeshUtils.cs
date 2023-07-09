using HKLib.hk2018;
using HKLib.Serialization.hk2018.Binary;
using Newtonsoft.Json.Linq;
using SoulsFormats;
using static NavMeshStudio.Utils;

namespace NavMeshStudio;

public class NavMeshUtils
{
    private static hkReferencedObject? GetReferencedObject(hkRootLevelContainer container, int index)
    {
        return container.m_namedVariants.ElementAtOrDefault(index)?.m_variant;
    }

    public static JObject? GetNavMeshJson(int type)
    {
        JObject? rootJson = type switch
        {
            1 => ToJson(Cache.Nva?.Data),
            2 => Cache.NvmJson,
            _ => null
        };
        return rootJson;
    }

    public static async Task<bool> GenerateNvmJson()
    {
        if (Cache.NvmJson != null) return true;
        if (Cache.NvmHktBnd == null) return false;
        await Task.Run(() =>
        {
            HavokBinarySerializer serializer = new();
            Cache.NvmJson = new JObject();
            for (int i = 0; i < Cache.NvmHktBnd.Data.Files.Count; ++i)
            {
                BinderFile file = Cache.NvmHktBnd.Data.Files[i];
                hkRootLevelContainer rootLevelContainer = (hkRootLevelContainer)serializer.Read(new MemoryStream(file.Bytes));
                hkReferencedObject? navMesh = GetReferencedObject(rootLevelContainer, 0);
                hkReferencedObject? queryMediator = GetReferencedObject(rootLevelContainer, 1);
                hkReferencedObject? userEdgeSetup = GetReferencedObject(rootLevelContainer, 2);
                JObject? navMeshJson = ToJson(navMesh);
                JObject? queryMediatorJson = ToJson(queryMediator);
                JObject? userEdgeSetupJson = ToJson(userEdgeSetup);
                Cache.NvmJson[(i + 1).ToString()] = new JObject
                {
                    { "NavMesh", navMeshJson },
                    { "QueryMediator", queryMediatorJson },
                    { "UserEdgeSetup", userEdgeSetupJson }
                };
            }
        });
        return true;
    }
}