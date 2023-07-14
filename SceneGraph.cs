namespace NavMeshStudio;

public class SceneGraph
{
    private readonly List<GraphNode> Nodes = new();
    private readonly TreeView View;

    public SceneGraph(NavMeshStudio studio)
    {
        View = studio.sceneGraphTreeView;
    }
}