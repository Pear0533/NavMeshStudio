using HKLib.hk2018;
using HKLib.hk2018.hkcdStaticMeshTree;

namespace NavMeshStudio;

public sealed class CLNode : GeoNode
{
    private readonly hknpBodyCinfo Collision;

    public CLNode(int clNodesCount, hknpBodyCinfo collision)
    {
        Name = (clNodesCount + 1).ToString();
        Collision = collision;
        Process();
    }

    protected override void Process()
    {
        List<Vector3> vertices = new();
        List<int> indices = new();
        fsnpCustomParamCompressedMeshShape meshShape = (fsnpCustomParamCompressedMeshShape)Collision.m_shape!;
        hknpCompressedMeshShapeData collisionData = (hknpCompressedMeshShapeData)meshShape.m_data!;
        foreach (Section section in collisionData.m_meshTree.m_sections)
        {
            // ...
        }
    }
}