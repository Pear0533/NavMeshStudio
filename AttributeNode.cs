namespace NavMeshStudio;

public sealed class AttributeNode : GraphNode
{
    public AttributeNode(TreeNode view)
    {
        View = view;
        Name = View.Name;
    }
}