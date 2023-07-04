// Automatically Generated

using System.Diagnostics.CodeAnalysis;
using HKLib.hk2018;
using HKLib.hk2018.hkaiNavMeshClearanceCacheSeeding;

namespace HKLib.Reflection.hk2018;

internal class hkaiNavMeshClearanceCacheSeedingCacheDataSetData : HavokData<CacheDataSet> 
{
    public hkaiNavMeshClearanceCacheSeedingCacheDataSetData(HavokType type, CacheDataSet instance) : base(type, instance) {}

    public override bool TryGetField<TGet>(string fieldName, [MaybeNull] out TGet value)
    {
        value = default;
        switch (fieldName)
        {
            case "m_propertyBag":
            case "propertyBag":
            {
                if (instance.m_propertyBag is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_cacheDatas":
            case "cacheDatas":
            {
                if (instance.m_cacheDatas is not TGet castValue) return false;
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
            case "m_propertyBag":
            case "propertyBag":
            {
                if (value is not hkPropertyBag castValue) return false;
                instance.m_propertyBag = castValue;
                return true;
            }
            case "m_cacheDatas":
            case "cacheDatas":
            {
                if (value is not List<CacheData> castValue) return false;
                instance.m_cacheDatas = castValue;
                return true;
            }
            default:
            return false;
        }
    }

}
