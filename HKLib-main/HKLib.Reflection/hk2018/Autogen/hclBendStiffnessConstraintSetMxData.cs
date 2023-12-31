// Automatically Generated

using System.Diagnostics.CodeAnalysis;
using HKLib.hk2018;

namespace HKLib.Reflection.hk2018;

internal class hclBendStiffnessConstraintSetMxData : HavokData<hclBendStiffnessConstraintSetMx>
{
    public hclBendStiffnessConstraintSetMxData(HavokType type, hclBendStiffnessConstraintSetMx instance) : base(type, instance) { }

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
            case "m_constraintId":
            case "constraintId":
            {
                if (instance.m_constraintId is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_batches":
            case "batches":
            {
                if (instance.m_batches is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_singles":
            case "singles":
            {
                if (instance.m_singles is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_maxRestPoseHeightSq":
            case "maxRestPoseHeightSq":
            {
                if (instance.m_maxRestPoseHeightSq is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_clampBendStiffness":
            case "clampBendStiffness":
            {
                if (instance.m_clampBendStiffness is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_useRestPoseConfig":
            case "useRestPoseConfig":
            {
                if (instance.m_useRestPoseConfig is not TGet castValue) return false;
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
            case "m_constraintId":
            case "constraintId":
            {
                if (value is not hkHandle<uint> castValue) return false;
                instance.m_constraintId = castValue;
                return true;
            }
            case "m_batches":
            case "batches":
            {
                if (value is not List<hclBendStiffnessConstraintSetMx.Batch> castValue) return false;
                instance.m_batches = castValue;
                return true;
            }
            case "m_singles":
            case "singles":
            {
                if (value is not List<hclBendStiffnessConstraintSetMx.Single> castValue) return false;
                instance.m_singles = castValue;
                return true;
            }
            case "m_maxRestPoseHeightSq":
            case "maxRestPoseHeightSq":
            {
                if (value is not float castValue) return false;
                instance.m_maxRestPoseHeightSq = castValue;
                return true;
            }
            case "m_clampBendStiffness":
            case "clampBendStiffness":
            {
                if (value is not bool castValue) return false;
                instance.m_clampBendStiffness = castValue;
                return true;
            }
            case "m_useRestPoseConfig":
            case "useRestPoseConfig":
            {
                if (value is not bool castValue) return false;
                instance.m_useRestPoseConfig = castValue;
                return true;
            }
            default:
                return false;
        }
    }
}