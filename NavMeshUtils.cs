using HKLib.hk2018;
using HKLib.Serialization.hk2018.Binary;
using Newtonsoft.Json.Linq;
using SoulsFormats;
using static NavMeshStudio.Utils;

namespace NavMeshStudio;

public class NavMeshUtils
{
    private static hkReferencedObject GetReferencedObject(hkRootLevelContainer container, int index)
    {
        return container.m_namedVariants.ElementAtOrDefault(index)?.m_variant ?? new hkReferencedObject();
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

    public static async Task<bool> ReadNavMeshGeometry()
    {
        if (Cache.NvmHktBnd == null) return false;
        await Task.Run(() =>
        {
            Cache.Clear();
            HavokBinarySerializer serializer = new();
            foreach (BinderFile file in Cache.NvmHktBnd.Data.Files)
            {
                hkRootLevelContainer rootLevelContainer = (hkRootLevelContainer)serializer.Read(new MemoryStream(file.Bytes));
                hkReferencedObject navMesh = GetReferencedObject(rootLevelContainer, 0);
                hkReferencedObject queryMediator = GetReferencedObject(rootLevelContainer, 1);
                hkReferencedObject userEdgeSetup = GetReferencedObject(rootLevelContainer, 2);
                if (navMesh is not hkaiNavMesh mesh) continue;
                Cache.NavMeshes.Add(mesh);
                Cache.QueryMediators.Add(queryMediator);
                Cache.UserEdgeSetups.Add(userEdgeSetup);
            }
        });
        return true;
    }

    public static async Task<bool> GenerateNvmJson()
    {
        if (Cache.NvmJson != null) return true;
        await Task.Run(() =>
        {
            Cache.NvmJson = new JObject();
            for (int i = 0; i < Cache.NavMeshes.Count; i++)
            {
                Cache.NvmJson[(i + 1).ToString()] = new JObject
                {
                    { "NavMesh", ToJson(Cache.NavMeshes[i]) },
                    { "QueryMediator", ToJson(Cache.QueryMediators[i]) },
                    { "UserEdgeSetup", ToJson(Cache.UserEdgeSetups[i]) }
                };
            }
        });
        return true;
    }
}