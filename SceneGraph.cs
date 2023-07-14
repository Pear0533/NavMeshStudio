namespace NavMeshStudio;

public class SceneGraph
{
    private readonly TreeView View;
    private readonly List<GraphNode> Nodes = new();

    public SceneGraph(NavMeshStudio studio)
    {
        View = studio.sceneGraphTreeView;
    }
}