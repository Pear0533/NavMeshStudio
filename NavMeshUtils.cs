using HKLib.hk2018;
using HKLib.Serialization.hk2018.Binary;
using Newtonsoft.Json.Linq;
using SoulsFormats;
using static NavMeshStudio.Utils;

namespace NavMeshStudio;

public class NavMeshUtils
{
    public static void BakeNavMeshes(CLNode node)
    {
        hkaiNavMeshBuilder builder = new();
        hkaiNavMeshBuilder.BuildParams buildParams = hkaiNavMeshBuilder.BuildParams.DefaultParams();
        List<Vector3> vertices = node.Vertices.Select(i => i.ToNumerics()).ToList();
        // TODO: For some reason navmesh building doesn't work for some collisions, needs investigation
        hkaiNavMesh navMesh = builder.BuildNavmesh(buildParams, vertices, node.Facesets).GetNavMesh();
        System.Console.WriteLine(navMesh);
    }

    public static async Task ReadNavMeshGeometry(NavMeshStudio studio)
    {
        Cache.NvmHktBnd = MapUtils.GetDependencyFile<FromSoftFile<BND4>>($"{Cache.Msb?.Name}.nvmhktbnd.dcx");
        if (Cache.NvmHktBnd == null)
        {
            await studio.UpdateStatus("This map doesn't contain navmesh, skipping...", 1000);
            return;
        }
        studio.UpdateStatus("Reading navmesh geometry...");
        await Task.Run(() =>
        {
            HavokBinarySerializer serializer = new();
            foreach (BinderFile file in Cache.NvmHktBnd.Data.Files)
            {
                hkRootLevelContainer container = serializer.GetRootLevelContainer(file.Bytes);
                hkReferencedObject navMesh = container.GetReferencedObject(0);
                hkReferencedObject queryMediator = container.GetReferencedObject(1);
                hkReferencedObject userEdgeSetup = container.GetReferencedObject(2);
                if (navMesh is not hkaiNavMesh mesh) continue;
                Cache.NavMeshes.Add(mesh);
                Cache.QueryMediators.Add(queryMediator);
                Cache.UserEdgeSetups.Add(userEdgeSetup);
            }
        });
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