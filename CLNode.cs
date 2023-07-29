using HKLib.hk2018;
using HKLib.hk2018.hkcdStaticMeshTree;
using Color = Microsoft.Xna.Framework.Color;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace NavMeshStudio;

public sealed class CLNode : GeoNode
{
    private readonly hknpBodyCinfo Collision;
    private hknpCompressedMeshShapeData CollisionData = null!;
    private Section CurrentSection = null!;
    private Primitive CurrentTri = null!;

    public CLNode(int clNodesCount, hknpBodyCinfo collision)
    {
        Name = (clNodesCount + 1).ToString();
        Collision = collision;
        Process();
    }

    private void TryAddVertexAndFaceset(ICollection<Vector3> vertices, ICollection<int> indices, int facesetIndex)
    {
        if (CurrentTri.m_indices[facesetIndex] < CollisionData.m_meshTree.m_sharedVertices.Count)
        {
            // Double-check m_firstPackedVertexIndex
            ushort index = (ushort)(CurrentTri.m_indices[facesetIndex] + CurrentSection.m_firstPackedVertexIndex);
            indices.Add(vertices.Count);
            System.Numerics.Vector3 vert = Utils3D.Decompress(CollisionData.m_meshTree.m_packedVertices.ElementAtOrDefault(index), CurrentSection.SmallVertexScale,
                CurrentSection.SmallVertexOffset);
            vertices.Add(vert.TransformVert(Collision));
        }
        else
        {
            ushort index = (ushort)CollisionData.m_meshTree.m_sharedVertices.ElementAtOrDefault((int)(CurrentTri.m_indices[facesetIndex]
                + CurrentSection.m_firstSharedVertexIndex
                - CollisionData.m_meshTree.m_sharedVertices.Count));
            indices.Add(vertices.Count);
            var vert = Utils3D.Decompress(CollisionData.m_meshTree.m_sharedVertices.ElementAtOrDefault(index), CollisionData.BoundingBoxMin, CollisionData.BoundingBoxMax);
            vertices.Add(vert.TransformVert(Collision));
        }
    }

    protected override void Process()
    {
        List<Vector3> vertices = new();
        List<int> indices = new();
        fsnpCustomParamCompressedMeshShape meshShape = (fsnpCustomParamCompressedMeshShape)Collision.m_shape!;
        CollisionData = (hknpCompressedMeshShapeData)meshShape.m_data!;
        foreach (Section section in CollisionData.m_meshTree.m_sections)
        {
            CurrentSection = section;
            for (int i = 0; i < CurrentSection.m_numPrimitives; i++)
            {
                // Double-check m_firstPrimitiveIndex
                CurrentTri = CollisionData.m_meshTree.m_primitives.ElementAtOrDefault((int)(i + CurrentSection.m_firstPrimitiveIndex)) ?? new Primitive();
                // Don't know what to do with this shape yet
                if (CurrentTri.m_indices[0] == 0xDE && CurrentTri.m_indices[1] == 0xAD && CurrentTri.m_indices[2] == 0xDE && CurrentTri.m_indices[3] == 0xAD)
                {
                    continue;
                }
                TryAddVertexAndFaceset(vertices, indices, 0);
                TryAddVertexAndFaceset(vertices, indices, 1);
                TryAddVertexAndFaceset(vertices, indices, 2);
                if (CurrentTri.m_indices[2] != CurrentTri.m_indices[3])
                {
                    indices.Add(vertices.Count);
                    vertices.Add(vertices[^3]);
                    indices.Add(vertices.Count);
                    vertices.Add(vertices[^2]);
                    TryAddVertexAndFaceset(vertices, indices, 3);
                }
                for (int n = 0; n < indices.Count; n += 3)
                {
                    Vector3 vert1 = vertices[indices[i]];
                    Vector3 vert2 = vertices[indices[i + 1]];
                    Vector3 vert3 = vertices[indices[i + 2]];
                    AddVerticesWithFacesets(new[] { vert1, vert2, vert3 }, Color.Yellow);
                }
            }
        }
    }
}