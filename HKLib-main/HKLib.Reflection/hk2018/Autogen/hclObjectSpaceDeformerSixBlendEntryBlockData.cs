// Automatically Generated

using System.Diagnostics.CodeAnalysis;
using HKLib.hk2018;

namespace HKLib.Reflection.hk2018;

internal class hclObjectSpaceDeformerSixBlendEntryBlockData : HavokData<hclObjectSpaceDeformer.SixBlendEntryBlock> 
{
    private static readonly System.Reflection.FieldInfo _vertexIndicesInfo = typeof(hclObjectSpaceDeformer.SixBlendEntryBlock).GetField("m_vertexIndices")!;
    private static readonly System.Reflection.FieldInfo _boneIndicesInfo = typeof(hclObjectSpaceDeformer.SixBlendEntryBlock).GetField("m_boneIndices")!;
    private static readonly System.Reflection.FieldInfo _boneWeightsInfo = typeof(hclObjectSpaceDeformer.SixBlendEntryBlock).GetField("m_boneWeights")!;
    public hclObjectSpaceDeformerSixBlendEntryBlockData(HavokType type, hclObjectSpaceDeformer.SixBlendEntryBlock instance) : base(type, instance) {}

    public override bool TryGetField<TGet>(string fieldName, [MaybeNull] out TGet value)
    {
        value = default;
        switch (fieldName)
        {
            case "m_vertexIndices":
            case "vertexIndices":
            {
                if (instance.m_vertexIndices is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_boneIndices":
            case "boneIndices":
            {
                if (instance.m_boneIndices is not TGet castValue) return false;
                value = castValue;
                return true;
            }
            case "m_boneWeights":
            case "boneWeights":
            {
                if (instance.m_boneWeights is not TGet castValue) return false;
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
            case "m_vertexIndices":
            case "vertexIndices":
            {
                if (value is not ushort[] castValue || castValue.Length != 16) return false;
                try
                {
                    _vertexIndicesInfo.SetValue(instance, value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            case "m_boneIndices":
            case "boneIndices":
            {
                if (value is not ushort[] castValue || castValue.Length != 96) return false;
                try
                {
                    _boneIndicesInfo.SetValue(instance, value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            case "m_boneWeights":
            case "boneWeights":
            {
                if (value is not ushort[] castValue || castValue.Length != 96) return false;
                try
                {
                    _boneWeightsInfo.SetValue(instance, value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            default:
            return false;
        }
    }

}
