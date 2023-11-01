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
}