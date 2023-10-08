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
        RegisterSceneGraphEvents();
        Populate(studio);
    }

    private void RegisterSceneGraphEvents()
    {
        View.NodeMouseClick += (_, e) =>
        {
            if (e.Node.Tag is not NVNode node) return;
            NVNodes.ForEach(i => i.Vertices.ForEach(x => x.Data.Color = x.BaseColorData.Color));
            NVNodes.ForEach(i => i.Facesets.ForEach(x => x.Data.Color = x.BaseColorData.Color));
            node.Vertices.ForEach(i => i.Data.Color = Microsoft.Xna.Framework.Color.Yellow);
            node.Facesets.ForEach(i => i.Data.Color = Microsoft.Xna.Framework.Color.Yellow);
            Cache.Viewer.RefreshGeometry();
        };
    }

    // TODO: Improve performance when reading collisions/map pieces

    private void Populate(NavMeshStudio studio)
    {
        TreeNode navMeshesRootNode = new("NavMeshes");
        TreeNode collisionsRootNode = new("Collisions");
        TreeNode mapPiecesRootNode = new("Map Pieces");
        Cache.NavMeshes.ForEach(i => NVNodes.Add(new NVNode(NVNodes.Count, i)));
        Cache.Collisions.ForEach(i => CLNodes.Add(new CLNode(CLNodes.Count, i)));
        studio.Invoke(() => studio.UpdateStatus("Reading map piece geometry..."));
        Cache.MapPieces.ForEach(i => MPNodes.Add(new MPNode(i)));
        studio.Invoke(studio.ResetStatus);
        View.Invoke(View.Nodes.Clear);
        navMeshesRootNode.Populate(NVNodes);
        collisionsRootNode.Populate(CLNodes);
        mapPiecesRootNode.Populate(MPNodes);
        View.Populate(navMeshesRootNode);
        View.Populate(collisionsRootNode);
        View.Populate(mapPiecesRootNode);
    }
}