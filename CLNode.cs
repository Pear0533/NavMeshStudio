using HKLib.hk2018;
using HKLib.hk2018.hkcdStaticMeshTree;
using Color = Microsoft.Xna.Framework.Color;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace NavMeshStudio;

public sealed class CLNode : GeoNode
{
    private readonly hknpBodyCinfo Collision;
    private hknpCompressedMeshShapeData MeshShapeData = null!;
    private Section CurrentSection = null!;
    private Primitive CurrentTri = null!;

    public CLNode(int clNodesCount, hknpBodyCinfo collision)
    {
        Name = (clNodesCount + 1).ToString();
        Collision = collision;
        Process();
    }

    // TODO: We'll need to get collisions to render as bounding boxes
    // TODO: Double-check this method to ensure that no oversights have been made

    private void TryAddVertexAndIndex(ICollection<Vector3> vertices, ICollection<int> indices, int indicesIndex)
    {
        Vector3 smallVertexOffset = new(CurrentSection.m_codecParms[0], CurrentSection.m_codecParms[1], CurrentSection.m_codecParms[2]);
        Vector3 smallVertexScale = new(CurrentSection.m_codecParms[3], CurrentSection.m_codecParms[4], CurrentSection.m_codecParms[5]);
        if (CurrentTri.m_indices[indicesIndex] < MeshShapeData.m_meshTree.m_sharedVertices.Count)
        {
            ushort index = (ushort)(CurrentTri.m_indices[indicesIndex] + CurrentSection.m_firstPackedVertexIndex);
            indices.Add(vertices.Count);
            System.Numerics.Vector3 vertex =
                Utils3D.DecompressPackedVertex(MeshShapeData.m_meshTree.m_packedVertices.ElementAtOrDefault(index), smallVertexScale, smallVertexOffset);
            vertices.Add(vertex.TransformVertex(Collision));
        }
        else
        {
            ushort index = MeshShapeData.m_meshTree.m_sharedVerticesIndex.ElementAtOrDefault((int)(CurrentTri.m_indices[indicesIndex]
                + CurrentSection.m_firstSharedVertexIndex
                - MeshShapeData.m_meshTree.m_sharedVertices.Count));
            indices.Add(vertices.Count);
            System.Numerics.Vector3 vertex = Utils3D.DecompressSharedVertex(MeshShapeData.m_meshTree.m_sharedVertices.ElementAtOrDefault(index),
                MeshShapeData.m_meshTree.m_domain.m_min, MeshShapeData.m_meshTree.m_domain.m_max);
            vertices.Add(vertex.TransformVertex(Collision));
        }
    }

    // TODO: Might need to account for m_firstPrimitiveIndex

    protected override void Process()
    {
        if (Collision.m_shape == null) return;
        Color facesetColor = Color.Blue;
        List<Vector3> vertices = new();
        List<int> indices = new();
        fsnpCustomParamCompressedMeshShape meshShape = (fsnpCustomParamCompressedMeshShape)Collision.m_shape;
        if (meshShape.m_data == null) return;
        MeshShapeData = (hknpCompressedMeshShapeData)meshShape.m_data;
        foreach (Section section in MeshShapeData.m_meshTree.m_sections)
        {
            CurrentSection = section;
            for (int i = 0; i < CurrentSection.m_numPrimitives; i++)
            {
                Primitive? tri = MeshShapeData.m_meshTree.m_primitives.ElementAtOrDefault((int)(i + CurrentSection.m_firstPrimitiveIndex));
                if (tri == null) continue;
                CurrentTri = tri;
                // TODO: Might need to eliminate or modify this comparison to avoid artifacts
                if (CurrentTri.m_indices[0] == 0xDE && CurrentTri.m_indices[1] == 0xAD && CurrentTri.m_indices[2] == 0xDE && CurrentTri.m_indices[3] == 0xAD)
                {
                    continue;
                }
                TryAddVertexAndIndex(vertices, indices, 0);
                TryAddVertexAndIndex(vertices, indices, 1);
                TryAddVertexAndIndex(vertices, indices, 2);
                if (CurrentTri.m_indices[2] == CurrentTri.m_indices[3]) continue;
                indices.Add(vertices.Count);
                vertices.Add(vertices[^3]);
                indices.Add(vertices.Count);
                vertices.Add(vertices[^2]);
                TryAddVertexAndIndex(vertices, indices, 3);
            }
        }
        for (int i = 0; i < indices.Count; i += 3)
        {
            Vector3 vert1 = vertices[indices[i]];
            Vector3 vert2 = vertices[indices[i + 1]];
            Vector3 vert3 = vertices[indices[i + 2]];
            AddVertices(new[] { vert1, vert2, vert3 }, facesetColor, false);
        }
    }
}