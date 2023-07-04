// Automatically Generated

using System.Diagnostics.CodeAnalysis;
using HKLib.hk2018.hkaiNavMeshEdgeZipper;

namespace HKLib.Reflection.hk2018;

internal class hkaiNavMeshEdgeZipperIntervalData : HavokData<Interval> 
{
    public hkaiNavMeshEdgeZipperIntervalData(HavokType type, Interval instance) : base(type, instance) {}

    public override bool TryGetField<TGet>(string fieldName, [MaybeNull] out TGet value)
    {
        value = default;
        switch (fieldName)
        {
            case "m_startT":
            case "startT":
            {
                if (instance.m_startT is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_endT":
            case "endT":
            {
                if (instance.m_endT is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            default:
            return false;
        }
    }

    public override bool TrySetField<TSet>(string fieldName, TSet value)
    {
        switch (fieldName)
        {
            case "m_startT":
            case "startT":
            {
                if (value is not float castValue) return false;
                instance.m_startT = castValue;
                return true;
            }
            case "m_endT":
            case "endT":
            {
                if (value is not float castValue) return false;
                instance.m_endT = castValue;
                return true;
            }
            default:
            return false;
        }
    }

}
