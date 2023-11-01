using System.Reflection;

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

    // TODO: Cleanup + use AttributeNodes list and the AttributeNode class

    public void Populate(object obj, TreeNode? node = null)
    {
        Type type = obj.GetType();
        if (node == null)
        {
            node = new TreeNode("ROOT");
            View.Nodes.Add(node);
        }
        foreach (FieldInfo field in type.GetFields())
        {
            if (field.Name == "Empty") continue;
            object? value = field.GetValue(obj);
            if (value == null) continue;
            string fieldValue = $"{field.Name}: {value}";
            TreeNode fieldNode = node.Nodes.Add(fieldValue);
            Populate(value, fieldNode);
        }
    }
}