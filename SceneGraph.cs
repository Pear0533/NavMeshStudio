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

    public static void Deselect<T>(List<T> nodes) where T : GeoNode
    {
        nodes.ForEach(i => i.Facesets.ForEach(x => x.Data.Color = x.BaseColorData.Color));
    }

    public void DeselectAll(bool refreshGeo = true)
    {
        Deselect(NVNodes);
        Deselect(CLNodes);
        Cache.Attributes.Clear();
        View.Invoke(() => View.SelectedNode = null);
        if (refreshGeo) Cache.Viewer.RefreshGeometry();
    }

    private static bool IsActionByMouse(TreeViewAction action)
    {
        return action is TreeViewAction.ByMouse;
    }

    private static bool IsActionByKeyboard(TreeViewAction action)
    {
        return action is TreeViewAction.ByKeyboard;
    }

    public void Select(GeoNode node)
    {
        bool isNodeSelected = node.Facesets.All(i => i.Data.Color == Microsoft.Xna.Framework.Color.Yellow);
        DeselectAll(false);
        if (!isNodeSelected && node is not MPNode)
        {
            node.Facesets.ForEach(i => i.Data.Color = Microsoft.Xna.Framework.Color.Yellow);
            Cache.Attributes.Populate(node);
        }
        Cache.Viewer.RefreshGeometry();
        View.Invoke(() => View.SelectedNode = isNodeSelected && node.Facesets.Count > 0 ? null : node.View);
        if (node.Facesets.Count == 0) Cache.Console.Write("The selected node contains no geometry");
    }

    private void RegisterSceneGraphEvents()
    {
        View.AfterSelect += (_, e) =>
        {
            if (!IsActionByMouse(e.Action) && !IsActionByKeyboard(e.Action)) return;
            if (e.Node?.Tag == null) return;
            // TODO: Load the selected node's properties in the attributes panel
            GeoNode node = (GeoNode)e.Node.Tag;
            Select(node);
        };
    }

    // TODO: Improve performance when reading collisions/map pieces

    private void Populate(NavMeshStudio studio)
    {
        TreeNode navMeshesRootNode = new("NavMeshes");
        TreeNode collisionsRootNode = new("Collisions");
        TreeNode mapPiecesRootNode = new("Map Pieces");
        Cache.NavMeshes.ForEach(i => NVNodes.Add(new NVNode(NVNodes.Count, i)));
        Cache.Collisions.ForEach(i => CLNodes.Add(new CLNode(i.m_name ?? "", i)));
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