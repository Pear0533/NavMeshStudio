using Microsoft.Xna.Framework.Graphics;

namespace NavMeshStudio;

public abstract class GeoNode : GraphNode
{
    private readonly List<VertexPositionColor> Facesets = new();
    private readonly bool IsVisible = true;
    protected readonly List<VertexPositionColor> Vertices = new();

    protected virtual void Process()
    {
        throw new NotImplementedException();
    }
}