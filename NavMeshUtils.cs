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

    public static async Task<bool> ReadNavMeshGeometry(NavMeshStudio studio)
    {
        Cache.NvmHktBnd = MapUtils.GetDependencyFile<FromSoftFile<BND4>>($"{Cache.Msb?.Name}.nvmhktbnd.dcx");
        studio.UpdateStatus("Reading navmesh geometry...");
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

    public static async Task<JObject> GenerateNvmJson()
    {
        JObject nvmJson = new();
        await Task.Run(() =>
        {
            for (int i = 0; i < Cache.NavMeshes.Count; i++)
            {
                nvmJson[(i + 1).ToString()] = new JObject
                {
                    { "NavMesh", ToJson(Cache.NavMeshes[i]) },
                    { "QueryMediator", ToJson(Cache.QueryMediators[i]) },
                    { "UserEdgeSetup", ToJson(Cache.UserEdgeSetups[i]) }
                };
            }
        });
        return nvmJson;
    }
}