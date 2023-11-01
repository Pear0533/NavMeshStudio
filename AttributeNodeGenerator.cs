using System.Reflection;
using HKLib.hk2018;
using static System.Reflection.BindingFlags;

namespace NavMeshStudio;

public class AttributeNodeGenerator
{
    public TreeNode GenerateTreeNodes(object obj)
    {
        TreeNode rootNode;
        if (obj is hknpBodyCinfo info) rootNode = new TreeNode(info.m_name);
        else rootNode = new TreeNode(obj.GetType().Name);
        Type type = obj.GetType();
        FieldInfo[] fields = type.GetFields(Instance | Public | NonPublic);
        foreach (FieldInfo field in fields)
        {
            TreeNode fieldNode = new(field.Name);
            object? fieldValue = field.GetValue(obj);
            TreeNode? valueNode = CreateValueNode(fieldValue);
            if (valueNode == null) continue;
            fieldNode.Nodes.Add(valueNode);
            rootNode.Nodes.Add(fieldNode);
        }
        return rootNode;
    }

    private static TreeNode? CreateValueNode(object? value)
    {
        if (value == null) return null;
        TreeNode valueNode = new(value.ToString());
        if (value.GetType().IsArray)
        {
            Array arrayValue = (Array)value;
            if (arrayValue.Length == 0)
            {
                TreeNode emptyCollectionNode = new("The collection is empty");
                return emptyCollectionNode;
            }
            for (int i = 0; i < arrayValue.Length; i++)
            {
                object? elementValue = arrayValue.GetValue(i);
                TreeNode? elementNode = CreateValueNode(elementValue);
                if (elementNode != null) valueNode.Nodes.Add(elementNode);
            }
        }
        else if (value.GetType().IsClass && value is not string)
        {
            AttributeNodeGenerator generator = new();
            TreeNode customTypeNode = generator.GenerateTreeNodes(value);
            valueNode.Nodes.Add(customTypeNode);
        }
        return valueNode;
    }
}