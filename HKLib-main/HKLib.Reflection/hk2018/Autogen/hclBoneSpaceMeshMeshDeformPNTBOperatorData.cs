// Automatically Generated

using System.Diagnostics.CodeAnalysis;
using HKLib.hk2018;

namespace HKLib.Reflection.hk2018;

internal class hclBoneSpaceMeshMeshDeformPNTBOperatorData : HavokData<hclBoneSpaceMeshMeshDeformPNTBOperator>
{
    public hclBoneSpaceMeshMeshDeformPNTBOperatorData(HavokType type, hclBoneSpaceMeshMeshDeformPNTBOperator instance) : base(type, instance) { }

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
            case "m_operatorID":
            case "operatorID":
            {
                if (instance.m_operatorID is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_usedBuffers":
            case "usedBuffers":
            {
                if (instance.m_usedBuffers is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_usedTransformSets":
            case "usedTransformSets":
            {
                if (instance.m_usedTransformSets is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_inputBufferIdx":
            case "inputBufferIdx":
            {
                if (instance.m_inputBufferIdx is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_outputBufferIdx":
            case "outputBufferIdx":
            {
                if (instance.m_outputBufferIdx is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_scaleNormalBehaviour":
            case "scaleNormalBehaviour":
            {
                if (instance.m_scaleNormalBehaviour is TGet castValue)
                {
                    value = castValue;
                    return true;
                }
                if ((uint)instance.m_scaleNormalBehaviour is TGet uintValue)
                {
                    value = uintValue;
                    return true;
                }
                return false;
            }
            case "m_inputTrianglesSubset":
            case "inputTrianglesSubset":
            {
                if (instance.m_inputTrianglesSubset is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_boneSpaceDeformer":
            case "boneSpaceDeformer":
            {
                if (instance.m_boneSpaceDeformer is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_localPNTBs":
            case "localPNTBs":
            {
                if (instance.m_localPNTBs is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_localUnpackedPNTBs":
            case "localUnpackedPNTBs":
            {
                if (instance.m_localUnpackedPNTBs is not TGet castValue) return false;
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
            case "m_operatorID":
            case "operatorID":
            {
                if (value is not uint castValue) return false;
                instance.m_operatorID = castValue;
                return true;
            }
            case "m_usedBuffers":
            case "usedBuffers":
            {
                if (value is not List<hclClothState.BufferAccess> castValue) return false;
                instance.m_usedBuffers = castValue;
                return true;
            }
            case "m_usedTransformSets":
            case "usedTransformSets":
            {
                if (value is not List<hclClothState.TransformSetAccess> castValue) return false;
                instance.m_usedTransformSets = castValue;
                return true;
            }
            case "m_inputBufferIdx":
            case "inputBufferIdx":
            {
                if (value is not uint castValue) return false;
                instance.m_inputBufferIdx = castValue;
                return true;
            }
            case "m_outputBufferIdx":
            case "outputBufferIdx":
            {
                if (value is not uint castValue) return false;
                instance.m_outputBufferIdx = castValue;
                return true;
            }
            case "m_scaleNormalBehaviour":
            case "scaleNormalBehaviour":
            {
                if (value is hclBoneSpaceMeshMeshDeformOperator.ScaleNormalBehaviour castValue)
                {
                    instance.m_scaleNormalBehaviour = castValue;
                    return true;
                }
                if (value is uint uintValue)
                {
                    instance.m_scaleNormalBehaviour = (hclBoneSpaceMeshMeshDeformOperator.ScaleNormalBehaviour)uintValue;
                    return true;
                }
                return false;
            }
            case "m_inputTrianglesSubset":
            case "inputTrianglesSubset":
            {
                if (value is not List<ushort> castValue) return false;
                instance.m_inputTrianglesSubset = castValue;
                return true;
            }
            case "m_boneSpaceDeformer":
            case "boneSpaceDeformer":
            {
                if (value is not hclBoneSpaceDeformer castValue) return false;
                instance.m_boneSpaceDeformer = castValue;
                return true;
            }
            case "m_localPNTBs":
            case "localPNTBs":
            {
                if (value is not List<hclBoneSpaceDeformer.LocalBlockPNTB> castValue) return false;
                instance.m_localPNTBs = castValue;
                return true;
            }
            case "m_localUnpackedPNTBs":
            case "localUnpackedPNTBs":
            {
                if (value is not List<hclBoneSpaceDeformer.LocalBlockUnpackedPNTB> castValue) return false;
                instance.m_localUnpackedPNTBs = castValue;
                return true;
            }
            default:
                return false;
        }
    }
}