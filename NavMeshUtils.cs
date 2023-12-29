using HKLib.hk2018;
using HKLib.Serialization.hk2018.Binary;
using Newtonsoft.Json.Linq;
using SoulsFormats;
using static NavMeshStudio.Utils;

namespace NavMeshStudio;

public static class NavMeshUtils
{
    private static void AddNavMeshDataToMap(hkRootLevelContainer container, bool updateSceneGraph = true)
    {
        hkReferencedObject navMesh = container.GetReferencedObject(0);
        hkReferencedObject queryMediator = container.GetReferencedObject(1);
        hkReferencedObject userEdgeSetup = container.GetReferencedObject(2);
        if (navMesh is not hkaiNavMesh mesh) return;
        Cache.NavMeshes.Add(mesh);
        Cache.QueryMediators.Add(queryMediator);
        Cache.UserEdgeSetups.Add(userEdgeSetup);
        if (updateSceneGraph) Cache.SceneGraph.MapNVNode(mesh);
    }

    public static async Task BakeNavMesh(NavMeshStudio studio, CLNode node)
    {
        hkRootLevelContainer container = new();
        await Task.Run(() =>
        {
            hkaiNavMeshBuilder builder = new();
            hkaiNavMeshBuilder.BuildParams buildParams = hkaiNavMeshBuilder.BuildParams.DefaultParams();
            List<Vector3> vertices = node.Vertices.Select(i => i.ToNumerics()).ToList();
            studio.UpdateStatus($"Building navmesh for {node.Name}...");
            // TODO: For some reason navmesh building doesn't work for some collisions, needs investigation
            container = builder.BuildNavmesh(buildParams, vertices, node.Facesets);
        });
        AddNavMeshDataToMap(container);
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
                AddNavMeshDataToMap(container, false);
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