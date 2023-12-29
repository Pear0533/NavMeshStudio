using HKLib.hk2018;

namespace NavMeshStudio;

public class SceneGraph
{
    public readonly List<MPNode> MPNodes = new();
    public readonly List<NVNode> NVNodes = new();
    public readonly List<CLNode> CLNodes = new();
    private readonly TreeView View = new();
    // TODO: Store the Studio instance in Cache
    private readonly NavMeshStudio Studio = null!;

    public SceneGraph() { }

    public SceneGraph(NavMeshStudio studio)
    {
        View = studio.sceneGraphTreeView;
        Studio = studio;
        RegisterSceneGraphEvents();
        Populate();
    }

    public static void Deselect<T>(List<T> nodes) where T : GeoNode
    {
        nodes.ForEach(i => i.DispFacesets.ForEach(x => x.Data.Color = x.BaseColorData.Color));
    }

    public void DeselectAll(bool refreshGeo = true)
    {
        Deselect(NVNodes);
        Deselect(CLNodes);
        Studio.navMeshEditingPanel.Invoke(Studio.navMeshEditingPanel.Controls.Clear);
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

    public void Select(NavMeshStudio studio, GeoNode node)
    {
        bool isNodeSelected = node.DispFacesets.All(i => i.Data.Color == Microsoft.Xna.Framework.Color.Yellow);
        DeselectAll(false);
        if (!isNodeSelected && node is not MPNode)
        {
            node.DispFacesets.ForEach(i => i.Data.Color = Microsoft.Xna.Framework.Color.Yellow);
            if (node is CLNode clNode)
            {
                Button bakeNavMeshButton = new() { Text = @"Bake NavMesh", AutoSize = true };
                bakeNavMeshButton.Click += async (_, _) => await NavMeshUtils.BakeNavMesh(studio, clNode);
                Studio.navMeshEditingPanel.Invoke(() => Studio.navMeshEditingPanel.Controls.Add(bakeNavMeshButton));
            }
        }
        Cache.Viewer.RefreshGeometry();
        View.Invoke(() => View.SelectedNode = isNodeSelected && node.DispFacesets.Count > 0 ? null : node.View);
        if (node.DispFacesets.Count == 0) Cache.Console.Write("The selected node contains no geometry");
    }

    private void RegisterSceneGraphEvents()
    {
        View.AfterSelect += (_, e) =>
        {
            if (!IsActionByMouse(e.Action) && !IsActionByKeyboard(e.Action)) return;
            if (e.Node?.Tag == null) return;
            GeoNode node = (GeoNode)e.Node.Tag;
            Select(Studio, node);
        };
    }

    public void MapNVNode(hkaiNavMesh mesh)
    {
        NVNodes.Add(new NVNode(NVNodes.Count, mesh));
        // TODO: Store the name of the root node into a constant variable
        TreeNode? root = View.Nodes.Cast<TreeNode>().ToList().Find(i => i.Text == @"NavMeshes");
        root?.Populate(new List<NVNode> { NVNodes[^1] });
        Cache.Viewer.RefreshGeometry(true);
    }

    // TODO: Improve performance when reading collisions/map pieces

    private void Populate()
    {
        TreeNode navMeshesRootNode = new("NavMeshes");
        TreeNode collisionsRootNode = new("Collisions");
        TreeNode mapPiecesRootNode = new("Map Pieces");
        Cache.NavMeshes.ForEach(i => NVNodes.Add(new NVNode(NVNodes.Count, i)));
        Cache.Collisions.ForEach(i => CLNodes.Add(new CLNode(i.m_name ?? "", i)));
        Studio.Invoke(() => Studio.UpdateStatus("Reading map piece geometry..."));
        Cache.MapPieces.ForEach(i => MPNodes.Add(new MPNode(i)));
        Studio.Invoke(Studio.ResetStatus);
        View.Invoke(View.Nodes.Clear);
        navMeshesRootNode.Populate(NVNodes);
        collisionsRootNode.Populate(CLNodes);
        mapPiecesRootNode.Populate(MPNodes);
        View.Populate(navMeshesRootNode);
        View.Populate(collisionsRootNode);
        View.Populate(mapPiecesRootNode);
    }
}