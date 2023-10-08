namespace NavMeshStudio;

public static class SceneGraphUtils
{
    public static void Populate<T>(this TreeNode root, List<T> nodes) where T : GraphNode
    {
        nodes.ToList().ForEach(i => root.Nodes.Add(new TreeNode { Tag = i, Text = i.Name }));
    }

    public static void Populate(this TreeView view, TreeNode node)
    {
        view.Invoke(() => view.Nodes.Add(node));
    }
}