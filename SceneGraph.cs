namespace NavMeshStudio;

public class SceneGraph
{
    public readonly List<GraphNode> GraphNodes = new();
    private readonly TreeView View = new();

    public SceneGraph() { }

    public SceneGraph(NavMeshStudio studio)
    {
        View = studio.sceneGraphTreeView;
        Populate();
    }

    private void Populate()
    {
        Cache.NavMeshes.ForEach(i => GraphNodes.Add(new NVNode(GraphNodes.Count.ToString(), i)));
        View.Invoke(View.Nodes.Clear);
        GraphNodes.ForEach(i => View.Invoke(() => View.Nodes.Add(i.Name)));
    }
}