using HKLib.hk2018;

namespace NavMeshStudio;

public class SceneGraph
{
    private readonly List<GraphNode> Nodes = new();
    private readonly TreeView View;

    public SceneGraph(NavMeshStudio studio)
    {
        View = studio.sceneGraphTreeView;
        Populate();
    }

    private void Populate()
    {
        foreach (hkaiNavMesh navMesh in Cache.NavMeshes)
        {
            NVNode nvNode = new(navMesh);
            Nodes.Add(nvNode);
        }
    }
}