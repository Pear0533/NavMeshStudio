using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace NavMeshStudio;

public abstract class GeoNode : GraphNode
{
    public List<VertexPositionColor> Facesets = new();
    public List<VertexPositionColor> Vertices = new();

    // TODO: Cleanup this method

    protected void AddVerticesWithFacesets(Vector3[] vertices, Color facesetColor)
    {
        Vector3 vert1Position = vertices[0];
        Vector3 vert2Position = vertices[1];
        Vector3 vert3Position = vertices[2];
        Vector3 vectorA = vert2Position - vert1Position;
        Vector3 vectorB = vert3Position - vert1Position;
        Vector3 normalVector = Utils3D.CrossProduct(vectorA, vectorB).NormalizeXnaVector3();
        Vector3 lightVector = new Vector3(1, 1, 1).NormalizeXnaVector3();
        float dotProduct = Utils3D.DotProduct(normalVector, lightVector);
        facesetColor.R = (byte)(facesetColor.R + (int)(facesetColor.R * dotProduct));
        facesetColor.G = (byte)(facesetColor.G + (int)(facesetColor.G * dotProduct));
        facesetColor.B = (byte)(facesetColor.B + (int)(facesetColor.B * dotProduct));
        vert1Position.FlipYZ();
        vert2Position.FlipYZ();
        vert3Position.FlipYZ();
        Vertices.AddRange(new[]
        {
            new VertexPositionColor(vert1Position, facesetColor),
            new VertexPositionColor(vert2Position, facesetColor),
            new VertexPositionColor(vert1Position, facesetColor),
            new VertexPositionColor(vert3Position, facesetColor),
            new VertexPositionColor(vert2Position, facesetColor),
            new VertexPositionColor(vert3Position, facesetColor)
        });
        Facesets.AddRange(new[]
        {
            new VertexPositionColor(vert1Position, facesetColor),
            new VertexPositionColor(vert3Position, facesetColor),
            new VertexPositionColor(vert2Position, facesetColor),
            new VertexPositionColor(vert1Position, facesetColor),
            new VertexPositionColor(vert3Position, facesetColor),
            new VertexPositionColor(vert2Position, facesetColor),
            new VertexPositionColor(vert1Position, facesetColor),
            new VertexPositionColor(vert2Position, facesetColor),
            new VertexPositionColor(vert3Position, facesetColor)
        });
    }

    protected virtual void Process()
    {
        throw new NotImplementedException();
    }
}