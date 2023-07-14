using HKLib.hk2018;

namespace NavMeshStudio;

public sealed class NVNode : GeoNode
{
    private readonly hkaiNavMesh NavMesh;

    public NVNode(hkaiNavMesh navMesh)
    {
        NavMesh = navMesh;
        Process();
    }

    protected override void Process()
    {
        throw new NotImplementedException();
    }
}