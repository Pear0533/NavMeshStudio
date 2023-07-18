using HKLib.hk2018;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace NavMeshStudio;

public sealed class NVNode : GeoNode
{
    private readonly hkaiNavMesh Mesh;

    public NVNode(string name, hkaiNavMesh mesh)
    {
        Name = name;
        Mesh = mesh;
        Process();
    }

    // TODO: Cleanup this method

    protected override void Process()
    {
        List<VertexPositionColor> vertices = new();
        List<VertexPositionColor> facesets = new();
        Color facesetColor = Utils.GetRandomColor();
        foreach (hkaiNavMesh.Face face in Mesh.m_faces)
        {
            int startEdgeIndices = face.m_startEdgeIndex;
            short edgeCount = face.m_numEdges;
            for (int i = 0; i < edgeCount - 2; i++)
            {
                int end = i + 2 >= edgeCount ? startEdgeIndices : startEdgeIndices + i + 2;
                Vector4 vert1 = Mesh.m_vertices[Mesh.m_edges[startEdgeIndices].m_a];
                Vector4 vert2 = Mesh.m_vertices[Mesh.m_edges[startEdgeIndices + i + 1].m_a];
                Vector4 vert3 = Mesh.m_vertices[Mesh.m_edges[end].m_a];
                Vector3 vert1Position = new(vert1.X, vert1.Y, vert1.Z);
                Vector3 vert2Position = new(vert2.X, vert2.Y, vert2.Z);
                Vector3 vert3Position = new(vert3.X, vert3.Y, vert3.Z);
                Vector3 vectorA = vert2Position - vert1Position;
                Vector3 vectorB = vert3Position - vert1Position;
                Vector3 normalVector = Utils3D.CrossProduct(vectorA, vectorB).NormalizeXnaVector3();
                Vector3 lightVector = new Vector3(1, 1, 1).NormalizeXnaVector3();
                // TODO: Method
                facesetColor.R = (byte)(facesetColor.R + Utils3D.DotProduct(normalVector, lightVector));
                facesetColor.G = (byte)(facesetColor.G + Utils3D.DotProduct(normalVector, lightVector));
                facesetColor.B = (byte)(facesetColor.B + Utils3D.DotProduct(normalVector, lightVector));
                vert1Position.FlipYZ();
                vert2Position.FlipYZ();
                vert3Position.FlipYZ();
                vertices.AddRange(new[]
                {
                    new VertexPositionColor(vert1Position, facesetColor),
                    new VertexPositionColor(vert2Position, facesetColor),
                    new VertexPositionColor(vert1Position, facesetColor),
                    new VertexPositionColor(vert3Position, facesetColor),
                    new VertexPositionColor(vert2Position, facesetColor),
                    new VertexPositionColor(vert3Position, facesetColor)
                });
                facesets.AddRange(new[]
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
        }
        Vertices = vertices;
        Facesets = facesets;
    }
}