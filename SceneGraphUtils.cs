namespace NavMeshStudio;

public static class SceneGraphUtils
{
    public static void Populate<T>(this TreeNode root, List<T> nodes) where T : GraphNode
    {
        foreach (T node in nodes)
        {
            TreeNode view = new() { Tag = node, Text = node.Name };
            node.View = view;
            root.Nodes.Add(view);
        }
    }

    public static void Populate(this TreeView view, TreeNode node)
    {
        view.Invoke(() => view.Nodes.Add(node));
    }
}