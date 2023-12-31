// Automatically Generated

using System.Diagnostics.CodeAnalysis;
using HKLib.hk2018;

namespace HKLib.Reflection.hk2018;

internal class hclBoneSpaceDeformerLocalBlockPData : HavokData<hclBoneSpaceDeformer.LocalBlockP>
{
    private static readonly System.Reflection.FieldInfo _localPositionInfo = typeof(hclBoneSpaceDeformer.LocalBlockP).GetField("m_localPosition")!;

    public hclBoneSpaceDeformerLocalBlockPData(HavokType type, hclBoneSpaceDeformer.LocalBlockP instance) : base(type, instance) { }

    public override bool TryGetField<TGet>(string fieldName, [MaybeNull] out TGet value)
    {
        value = default;
        switch (fieldName)
        {
            case "m_localPosition":
            case "localPosition":
            {
                if (instance.m_localPosition is not TGet castValue) return false;
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
            case "m_localPosition":
            case "localPosition":
            {
                if (value is not Vector4[] castValue || castValue.Length != 16) return false;
                try
                {
                    _localPositionInfo.SetValue(instance, value);
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