namespace NavMeshStudio;

public abstract class GraphNode
{
    private readonly List<GraphNode> Children = new();
    public string Name = "";
}