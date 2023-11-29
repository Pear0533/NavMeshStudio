using Color = Microsoft.Xna.Framework.Color;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace NavMeshStudio;

public abstract class GeoNode : GraphNode
{
    public List<GeoElement> DispFacesets = new();
    public List<GeoElement> DispVertices = new();
    // TODO: Navmeshes and map pieces
    public List<Vector3> Vertices = new();
    public List<int> Facesets = new();

    protected void AddVertices(Vector3[] vertices, Color facesetColor, bool generateFacesets = true)
    {
        Vector3 vectorA = vertices[1] - vertices[0];
        Vector3 vectorB = vertices[2] - vertices[0];
        Vector3 normalVector = Utils3D.CrossProduct(vectorA, vectorB).NormalizeXnaVector3();
        Vector3 lightVector = new Vector3(1, 1, 1).NormalizeXnaVector3();
        float dotProduct = Utils3D.DotProduct(normalVector, lightVector);
        facesetColor.R = (byte)(facesetColor.R + (int)(facesetColor.R * dotProduct));
        facesetColor.G = (byte)(facesetColor.G + (int)(facesetColor.G * dotProduct));
        facesetColor.B = (byte)(facesetColor.B + (int)(facesetColor.B * dotProduct));
        vertices[0].FlipYZ();
        vertices[1].FlipYZ();
        vertices[2].FlipYZ();
        DispVertices.AddRange(Utils3D.GetVertices(vertices, Color.Black));
        if (generateFacesets) DispFacesets.AddRange(Utils3D.GetFacesets(vertices, facesetColor));
    }

    public List<Vector3> GetVertexPositions()
    {
        return DispVertices.Select(i => i.Data.Position).ToList();
    }

    protected virtual void Process()
    {
        throw new NotImplementedException();
    }
}