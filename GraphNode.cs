namespace NavMeshStudio;

public abstract class GraphNode
{
    public readonly List<GraphNode> Children = new();
    public TreeNode View = new();
    public string Name = "";
}