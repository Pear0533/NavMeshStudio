using static NavMeshStudio.AttributeUtils;

namespace NavMeshStudio;

public class Attributes
{
    private readonly List<AttributeNode> AttributeNodes = new();
    private readonly TreeView View = new();

    public Attributes() { }

    public Attributes(NavMeshStudio studio)
    {
        View = studio.attributesTreeView;
        RegisterAttributesPanelEvents();
    }

    private static void ExpandAttributeNode(TreeNode? node)
    {
        if (node?.Tag == null || node.Nodes.Count > 0) return;
        node.Nodes.AddRange(((List<TreeNode>)node.Tag).ToArray());
        node.Expand();
    }

    private void CollapseAttributeNode(TreeNode? node)
    {
        if (node == null) return;
        node.Nodes.Clear();
        View.SelectedNode = null;
    }

    private void RegisterAttributesPanelEvents()
    {
        View.AfterSelect += (_, e) => ExpandAttributeNode(e.Node);
        View.AfterExpand += (_, e) => ExpandAttributeNode(e.Node);
        View.AfterCollapse += (_, e) => CollapseAttributeNode(e.Node);
    }

    public void Clear()
    {
        View.Invoke(View.Nodes.Clear);
        AttributeNodes.Clear();
    }

    private void ProcessAttributeNodes(IEnumerable<TreeNode> attributeNodes)
    {
        attributeNodes.ToList().ForEach(i => AttributeNodes.Add(new AttributeNode(i)));
        AttributeNodes.ForEach(i => View.Invoke(() => View.Nodes.Add(i.View)));
    }

    public void Populate(GeoNode node)
    {
        View.Invoke(View.Nodes.Clear);
        TreeNode[] attributeNodes = node switch
        {
            CLNode clNode => GetAttributeNodes(clNode.Collision).ToArray(),
            NVNode nvNode => GetAttributeNodes(nvNode.Mesh).ToArray(),
            _ => Array.Empty<TreeNode>()
        };
        ProcessAttributeNodes(attributeNodes);
    }
}