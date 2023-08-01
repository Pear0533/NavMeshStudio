using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector3 = System.Numerics.Vector3;
using Color = Microsoft.Xna.Framework.Color;

namespace NavMeshStudio;

public static class BBoxBuffer
{
    public static void SetVerticesFromBoundingBox(this CLNode clNode, BoundingBox boundingBox)
    {
        List<VertexPositionColor> vertices = new();
        const float ratio = 5.0f;
        Vector3 xOffset = new((boundingBox.Max.X - boundingBox.Min.X) / ratio, 0, 0);
        Vector3 yOffset = new(0, (boundingBox.Max.Y - boundingBox.Min.Y) / ratio, 0);
        Vector3 zOffset = new(0, 0, (boundingBox.Max.Z - boundingBox.Min.Z) / ratio);
        Microsoft.Xna.Framework.Vector3[] corners = boundingBox.GetCorners();
        Color vertexColor = Utils.GetRandomColor();
        AddVertex(vertices, corners[0], vertexColor);
        AddVertex(vertices, corners[0] + xOffset, vertexColor);
        AddVertex(vertices, corners[0], vertexColor);
        AddVertex(vertices, corners[0] - yOffset, vertexColor);
        AddVertex(vertices, corners[0], vertexColor);
        AddVertex(vertices, corners[0] - zOffset, vertexColor);
        AddVertex(vertices, corners[1], vertexColor);
        AddVertex(vertices, corners[1] - xOffset, vertexColor);
        AddVertex(vertices, corners[1], vertexColor);
        AddVertex(vertices, corners[1] - yOffset, vertexColor);
        AddVertex(vertices, corners[1], vertexColor);
        AddVertex(vertices, corners[1] - zOffset, vertexColor);
        AddVertex(vertices, corners[2], vertexColor);
        AddVertex(vertices, corners[2] - xOffset, vertexColor);
        AddVertex(vertices, corners[2], vertexColor);
        AddVertex(vertices, corners[2] + yOffset, vertexColor);
        AddVertex(vertices, corners[2], vertexColor);
        AddVertex(vertices, corners[2] - zOffset, vertexColor);
        AddVertex(vertices, corners[3], vertexColor);
        AddVertex(vertices, corners[3] + xOffset, vertexColor);
        AddVertex(vertices, corners[3], vertexColor);
        AddVertex(vertices, corners[3] + yOffset, vertexColor);
        AddVertex(vertices, corners[3], vertexColor);
        AddVertex(vertices, corners[3] - zOffset, vertexColor);
        AddVertex(vertices, corners[4], vertexColor);
        AddVertex(vertices, corners[4] + xOffset, vertexColor);
        AddVertex(vertices, corners[4], vertexColor);
        AddVertex(vertices, corners[4] - yOffset, vertexColor);
        AddVertex(vertices, corners[4], vertexColor);
        AddVertex(vertices, corners[4] + zOffset, vertexColor);
        AddVertex(vertices, corners[5], vertexColor);
        AddVertex(vertices, corners[5] - xOffset, vertexColor);
        AddVertex(vertices, corners[5], vertexColor);
        AddVertex(vertices, corners[5] - yOffset, vertexColor);
        AddVertex(vertices, corners[5], vertexColor);
        AddVertex(vertices, corners[5] + zOffset, vertexColor);
        AddVertex(vertices, corners[6], vertexColor);
        AddVertex(vertices, corners[6] - xOffset, vertexColor);
        AddVertex(vertices, corners[6], vertexColor);
        AddVertex(vertices, corners[6] + yOffset, vertexColor);
        AddVertex(vertices, corners[6], vertexColor);
        AddVertex(vertices, corners[6] + zOffset, vertexColor);
        AddVertex(vertices, corners[7], vertexColor);
        AddVertex(vertices, corners[7] + xOffset, vertexColor);
        AddVertex(vertices, corners[7], vertexColor);
        AddVertex(vertices, corners[7] + yOffset, vertexColor);
        AddVertex(vertices, corners[7], vertexColor);
        AddVertex(vertices, corners[7] + zOffset, vertexColor);
        clNode.Vertices = vertices;
    }

    private static void AddVertex(this ICollection<VertexPositionColor> vertices, Microsoft.Xna.Framework.Vector3 position, Color vertexColor)
    {
        vertices.Add(new VertexPositionColor(position, vertexColor));
    }
}