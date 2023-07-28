namespace NavMeshStudio;

public class SceneGraph
{
    public readonly List<MPNode> MPNodes = new();
    public readonly List<NVNode> NVNodes = new();
    public readonly List<CLNode> CLNodes = new();
    private readonly TreeView View = new();

    public SceneGraph() { }

    public SceneGraph(NavMeshStudio studio)
    {
        View = studio.sceneGraphTreeView;
        Populate(studio);
    }

    // TODO: Improve performance when reading map pieces

    private void Populate(NavMeshStudio studio)
    {
        TreeNode navMeshesRootNode = new("NavMeshes");
        TreeNode mapPiecesRootNode = new("Map Pieces");
        studio.Invoke(() => studio.UpdateStatus("Reading map piece geometry..."));
        Cache.NavMeshes.ForEach(i => NVNodes.Add(new NVNode(NVNodes.Count, i)));
        Cache.Collisions.ForEach(i => CLNodes.Add(new CLNode(NVNodes.Count, i)));
        Cache.MapPieces.ForEach(i => MPNodes.Add(new MPNode(i)));
        studio.Invoke(() => studio.ToggleStudioControls(true));
        studio.Invoke(studio.ResetStatus);
        View.Invoke(View.Nodes.Clear);
        navMeshesRootNode.Populate(NVNodes);
        mapPiecesRootNode.Populate(MPNodes);
        View.Populate(navMeshesRootNode);
        View.Populate(mapPiecesRootNode);
    }
}