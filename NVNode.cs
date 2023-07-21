using HKLib.hk2018;
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

    protected override void Process()
    {
        Color facesetColor = Utils.GetRandomColor();
        foreach (hkaiNavMesh.Face face in Mesh.m_faces)
        {
            int startEdgeIndices = face.m_startEdgeIndex;
            short edgeCount = face.m_numEdges;
            for (int i = 0; i < edgeCount - 2; i++)
            {
                int end = i + 2 >= edgeCount ? startEdgeIndices : startEdgeIndices + i + 2;
                Vector3 vert1 = Mesh.m_vertices[Mesh.m_edges[startEdgeIndices].m_a].ToVector3();
                Vector3 vert2 = Mesh.m_vertices[Mesh.m_edges[startEdgeIndices + i + 1].m_a].ToVector3();
                Vector3 vert3 = Mesh.m_vertices[Mesh.m_edges[end].m_a].ToVector3();
                AddVerticesWithFacesets(new[] { vert1, vert2, vert3 }, facesetColor);
            }
        }
    }
}