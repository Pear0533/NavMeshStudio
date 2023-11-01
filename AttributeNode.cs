namespace NavMeshStudio;

public sealed class AttributeNode : GraphNode
{
    public AttributeNode(string name, TreeNode view)
    {
        View = view;
        Name = name;
    }
}