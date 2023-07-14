using Microsoft.Xna.Framework.Graphics;

namespace NavMeshStudio;

public abstract class GeoNode : GraphNode
{
    private readonly List<VertexPositionColor> Facesets = new();
    public List<VertexPositionColor> Vertices = new();

    protected virtual void Process()
    {
        throw new NotImplementedException();
    }
}