using System.Reflection;
using HKLib.hk2018;
using static System.Reflection.BindingFlags;

namespace NavMeshStudio;

public static class AttributeUtils
{
    public static List<TreeNode> GetAttributeNodes(object obj)
    {
        Type type = obj.GetType();
        string? rootNodeName = obj is hknpBodyCinfo info ? info.m_name : type.Name;
        TreeNode rootNode = new(rootNodeName);
        FieldInfo[] fields = type.GetFields(Instance | Public | NonPublic);
        List<TreeNode> attributeNodes = new();
        foreach (FieldInfo field in fields)
        {
            TreeNode attributeNode = new(field.Name);
            object? attributeValue = field.GetValue(obj);
            TreeNode? valueNode = GetAttributeValueNode(attributeValue);
            if (valueNode == null) continue;
            attributeNode.Tag = new List<TreeNode> { valueNode };
            attributeNodes.Add(attributeNode);
        }
        rootNode.Tag = attributeNodes;
        return new List<TreeNode> { rootNode };
    }

    private static TreeNode? GetAttributeValueNode(object? obj)
    {
        if (obj == null) return null;
        TreeNode valueNode = new(obj.ToString());
        Type type = obj.GetType();
        if (type.IsArray)
        {
            Array array = (Array)obj;
            if (array.Length == 0) return new TreeNode("Array contains no items");
            List<object> arrayItems = array.Cast<object>().ToList();
            List<TreeNode> itemNodes = new();
            foreach (object item in arrayItems)
            {
                TreeNode? itemNode = GetAttributeValueNode(item);
                if (itemNode != null) itemNodes.Add(itemNode);
            }
            valueNode.Tag = itemNodes;
        }
        else if (type.IsClass && obj is not string) valueNode.Tag = GetAttributeNodes(obj);
        return valueNode;
    }
}