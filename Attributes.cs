namespace NavMeshStudio;

public class Attributes
{
    public readonly List<AttributeNode> AttributeNodes = new();
    private readonly TreeView View = new();

    public Attributes() { }

    public Attributes(NavMeshStudio studio)
    {
        View = studio.attributesTreeView;
    }

    // TODO: Use the AttributeNode class + AttributeNodes

    public void Populate(GeoNode node)
    {
        View.Invoke(View.Nodes.Clear);
        Type type = node.GetType();
        AttributeNodeGenerator generator = new();
        if (type == typeof(CLNode))
        {
            CLNode clNode = (CLNode)node;
            TreeNode attrClNode = generator.GenerateTreeNodes(clNode.Collision);
            View.Invoke(() => View.Nodes.Add(attrClNode));
        }
        else if (type == typeof(NVNode))
        {
            NVNode nvNode = (NVNode)node;
            TreeNode attrNvNode = generator.GenerateTreeNodes(nvNode.Mesh);
            View.Invoke(() => View.Nodes.Add(attrNvNode));
        }
    }
}