namespace NavMeshStudio;

public class SceneGraph
{
    public readonly List<GraphNode> NVNodes = new();
    private readonly TreeView View = new();

    public SceneGraph() { }

    public SceneGraph(NavMeshStudio studio)
    {
        View = studio.sceneGraphTreeView;
        Populate();
    }

    private void Populate()
    {
        TreeNode navMeshesRootNode = new("NavMeshes");
        Cache.NavMeshes.ForEach(i => NVNodes.Add(new NVNode((NVNodes.Count + 1).ToString(), i)));
        View.Invoke(View.Nodes.Clear);
        NVNodes.ForEach(i => View.Invoke(() => navMeshesRootNode.Nodes.Add(i.Name)));
        View.Invoke(() => View.Nodes.Add(navMeshesRootNode));
    }
}