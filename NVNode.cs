using HKLib.hk2018;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace NavMeshStudio;

public sealed class NVNode : GeoNode
{
    private readonly hkaiNavMesh Mesh;

    public NVNode(hkaiNavMesh mesh)
    {
        Mesh = mesh;
        Process();
    }

    // TODO: Cleanup this method

    protected override void Process()
    {
        int idx = 0;
        int maxcluster = 0;
        foreach (hkaiNavMesh.Face face in Mesh.m_faces)
        {
            if (face.m_clusterIndex > maxcluster) maxcluster = face.m_clusterIndex;
            int startEdgeIndices = face.m_startEdgeIndex;
            short edgeCount = face.m_numEdges;
            for (int i = 0; i < edgeCount - 2; i++)
            {
                int end = i + 2 >= edgeCount ? startEdgeIndices : startEdgeIndices + i + 2;
                Vector4 vert1 = Mesh.m_vertices[Mesh.m_edges[startEdgeIndices].m_a];
                Vector4 vert2 = Mesh.m_vertices[Mesh.m_edges[startEdgeIndices + i + 1].m_a];
                Vector4 vert3 = Mesh.m_vertices[Mesh.m_edges[end].m_a];
                Vertices[idx] = new VertexPositionColor(new Vector3(vert1.X, vert1.Y, vert1.Z), Color.Pink);
                Vertices[idx + 1] = new VertexPositionColor(new Vector3(vert2.X, vert2.Y, vert2.Z), Color.Pink);
                Vertices[idx + 2] = new VertexPositionColor(new Vector3(vert3.X, vert3.Y, vert3.Z), Color.Pink);
                idx += 3;
            }
        }
    }
}