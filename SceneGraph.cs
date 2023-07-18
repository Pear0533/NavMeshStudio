namespace NavMeshStudio;

public class SceneGraph
{
    public readonly List<MPNode> MPNodes = new();
    public readonly List<NVNode> NVNodes = new();
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
        TreeNode mapPiecesRootNode = new("MapPieces");
        // TODO: Formulate a generic method for creating nodes from objects
        Cache.NavMeshes.ForEach(i => NVNodes.Add(new NVNode((NVNodes.Count + 1).ToString(), i)));
        // TODO: Use actual names for the map pieces in the scene graph
        Cache.MapPieces.ForEach(i => MPNodes.Add(new MPNode((MPNodes.Count + 1).ToString(), i)));
        View.Invoke(View.Nodes.Clear);
        // TODO: Formulate a generic method for adding nodes to a root
        NVNodes.ForEach(i => navMeshesRootNode.Nodes.Add(i.Name));
        MPNodes.ForEach(i => mapPiecesRootNode.Nodes.Add(i.Name));
        // TODO: Create a method for adding a root node to the scene graph
        View.Invoke(() => View.Nodes.Add(navMeshesRootNode));
        View.Invoke(() => View.Nodes.Add(mapPiecesRootNode));
    }
}