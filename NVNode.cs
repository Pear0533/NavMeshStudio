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
                Vector3 vert1Position = new(vert1.X, vert1.Z, vert1.Y);
                Vector3 vert2Position = new(vert2.X, vert2.Z, vert2.Y);
                Vector3 vert3Position = new(vert3.X, vert3.Z, vert3.Y);
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