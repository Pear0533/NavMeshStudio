namespace NavMeshStudio;

public abstract class GraphNode
{
    private readonly List<GraphNode> Children = new();
    private readonly string Name = "";
}