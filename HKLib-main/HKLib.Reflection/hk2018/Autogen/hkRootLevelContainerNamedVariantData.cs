// Automatically Generated

using System.Diagnostics.CodeAnalysis;
using HKLib.hk2018;

namespace HKLib.Reflection.hk2018;

internal class hkRootLevelContainerNamedVariantData : HavokData<hkRootLevelContainer.NamedVariant> 
{
    public hkRootLevelContainerNamedVariantData(HavokType type, hkRootLevelContainer.NamedVariant instance) : base(type, instance) {}

    public override bool TryGetField<TGet>(string fieldName, [MaybeNull] out TGet value)
    {
        value = default;
        switch (fieldName)
        {
            case "m_name":
            case "name":
            {
                if (instance.m_name is null)
                {
                    return true;
                }
                if (instance.m_name is TGet castValue)
                {
                    value = castValue;
                    return true;
                }
                return false;
            }
            case "m_className":
            case "className":
            {
                if (instance.m_className is null)
                {
                    return true;
                }
                if (instance.m_className is TGet castValue)
                {
                    value = castValue;
                    return true;
                }
                return false;
            }
            case "m_variant":
            case "variant":
            {
                if (instance.m_variant is null)
                {
                    return true;
                }
                if (instance.m_variant is TGet castValue)
                {
                    value = castValue;
                    return true;
                }
                return false;
            }
            default:
            return false;
        }
    }

    public override bool TrySetField<TSet>(string fieldName, TSet value)
    {
        switch (fieldName)
        {
            case "m_name":
            case "name":
            {
                if (value is null)
                {
                    instance.m_name = default;
                    return true;
                }
                if (value is string castValue)
                {
                    instance.m_name = castValue;
                    return true;
                }
                return false;
            }
            case "m_className":
            case "className":
            {
                if (value is null)
                {
                    instance.m_className = default;
                    return true;
                }
                if (value is string castValue)
                {
                    instance.m_className = castValue;
                    return true;
                }
                return false;
            }
            case "m_variant":
            case "variant":
            {
                if (value is null)
                {
                    instance.m_variant = default;
                    return true;
                }
                if (value is hkReferencedObject castValue)
                {
                    instance.m_variant = castValue;
                    return true;
                }
                return false;
            }
            default:
            return false;
        }
    }

}
