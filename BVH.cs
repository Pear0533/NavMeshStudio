using HKLib.hk2018.hkcdCompressedAabbCodecs;

namespace NavMeshStudio;

public static class BVH
{
    [DllImport("NavGen.dll")]
    public static extern bool BuildBVHForMesh([In] Vector3[] verts, int vcount, [In] ushort[] indices, int icount);

    [DllImport("NavGen.dll")]
    public static extern ulong GetNodeSize();

    [DllImport("NavGen.dll")]
    public static extern ulong GetBVHSize();

    [DllImport("NavGen.dll")]
    public static extern void GetBVHNodes([In] [Out] NativeBVHNode[] buffer);
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct NativeBVHNode
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;
    public uint isLeaf;
    public uint primitiveCount;
    public uint firstChildOrPrimitive;
}
public class BVNode
{
    public bool IsLeaf;

    public bool IsSectionHead;
    public BVNode Left;
    public Vector3 Max;
    public Vector3 Min;
    public uint Primitive;
    public uint PrimitiveCount;
    public BVNode Right;
    public uint UniqueIndicesCount;

    public uint ComputePrimitiveCounts()
    {
        if (IsLeaf)
        {
            return PrimitiveCount;
        }
        PrimitiveCount = Left.ComputePrimitiveCounts() + Right.ComputePrimitiveCounts();
        return PrimitiveCount;
    }

    public HashSet<ushort> ComputeUniqueIndicesCounts(List<ushort> indices)
    {
        if (IsLeaf)
        {
            HashSet<ushort> s = new();
            s.Add(indices[(int)Primitive * 3]);
            s.Add(indices[(int)Primitive * 3 + 1]);
            s.Add(indices[(int)Primitive * 3 + 2]);
            UniqueIndicesCount = 3;
            return s;
        }
        HashSet<ushort> left = Left.ComputeUniqueIndicesCounts(indices);
        HashSet<ushort> right = Right.ComputeUniqueIndicesCounts(indices);
        left.UnionWith(right);
        UniqueIndicesCount = (uint)left.Count;
        return left;
    }

    /// <summary>
    ///     Marks nodes that are the head of sections - independently compressed mesh
    ///     chunks with their own BVH
    /// </summary>
    public void AttemptSectionSplit()
    {
        // Very simple primitive count based splitting heuristic for now
        if (!IsLeaf && (PrimitiveCount > 127 || UniqueIndicesCount > 255))
        {
            IsSectionHead = false;
            Left.IsSectionHead = true;
            Right.IsSectionHead = true;
            Left.AttemptSectionSplit();
            Right.AttemptSectionSplit();
        }
    }

    private static byte CompressDim(float min, float max, float pmin, float pmax)
    {
        float snorm = 226.0f / (pmax - pmin);
        float rmin = MathF.Sqrt(MathF.Max((min - pmin) * snorm, 0));
        float rmax = MathF.Sqrt(MathF.Max((max - pmax) * -snorm, 0));
        byte a = (byte)Math.Min(0xF, (int)MathF.Floor(rmin));
        byte b = (byte)Math.Min(0xF, (int)MathF.Floor(rmax));
        return (byte)(a << 4 | b);
    }

    /// <summary>
    ///     Converts the tree into an axis 4 compressed havok tree
    /// </summary>
    /// <returns></returns>
    public List<Aabb4BytesCodec> BuildAxis4Tree()
    {
        var ret = new List<Aabb4BytesCodec>();

        void CompressNode(BVNode node, Vector3 pbbmin, Vector3 pbbmax)
        {
            int currindex = ret.Count();
            var compressed = new Aabb4BytesCodec();
            ret.Add(compressed);

            // Compress the bounding box
            compressed.m_xyz[0] = CompressDim(node.Min.X, node.Max.X, pbbmin.X, pbbmax.X);
            compressed.m_xyz[1] = CompressDim(node.Min.Y, node.Max.Y, pbbmin.Y, pbbmax.Y);
            compressed.m_xyz[2] = CompressDim(node.Min.Z, node.Max.Z, pbbmin.Z, pbbmax.Z);

            // Read back the decompressed bounding box to use as reference for next compression
            var min = DecompressMin(compressed, pbbmin, pbbmax);
            var max = DecompressMax(compressed, pbbmin, pbbmax);
            if (node.IsLeaf)
            {
                compressed.m_data = (byte)(node.Primitive * 2);
            }
            else
            {
                // Add the left as the very next node
                CompressNode(node.Left, min, max);

                // Encode the index of the right then add it. The index should
                // always be even
                compressed.m_data = (byte)(ret.Count() - currindex | 0x1);

                // Now encode the right
                CompressNode(node.Right, min, max);
            }
        }

        CompressNode(this, Min, Max);
        return ret;
    }

    public List<Aabb5BytesCodec> BuildAxis5Tree()
    {
        var ret = new List<Aabb5BytesCodec>();

        void CompressNode(BVNode node, Vector3 pbbmin, Vector3 pbbmax, bool root = false)
        {
            int currindex = ret.Count();
            var compressed = new Aabb5BytesCodec();
            ret.Add(compressed);

            // Compress the bounding box
            compressed.m_xyz[0] = CompressDim(node.Min.X, node.Max.X, pbbmin.X, pbbmax.X);
            compressed.m_xyz[1] = CompressDim(node.Min.Y, node.Max.Y, pbbmin.Y, pbbmax.Y);
            compressed.m_xyz[2] = CompressDim(node.Min.Z, node.Max.Z, pbbmin.Z, pbbmax.Z);

            // Read back the decompressed bounding box to use as reference for next compression
            var min = DecompressMin(compressed, pbbmin, pbbmax);
            var max = DecompressMax(compressed,pbbmin, pbbmax);
            if (node.IsLeaf)
            {
                ushort data = (ushort)node.Primitive;
                compressed.m_loData = (byte)(data & 0xFF);
                compressed.m_hiData = (byte)(data >> 8 & 0x7F);
            }
            else
            {
                // Add the left as the very next node
                CompressNode(node.Left, min, max);

                // Encode the index of the right then add it. The index should
                // always be even
                ushort data = (ushort)((ret.Count() - currindex) / 2);
                compressed.m_loData = (byte)(data & 0xFF);
                compressed.m_hiData = (byte)(data >> 8 & 0x7F | 0x80);

                // Now encode the right
                CompressNode(node.Right, min, max);
            }
            if (root)
            {
                compressed.m_xyz[0] = 0;
                compressed.m_xyz[1] = 0;
                compressed.m_xyz[2] = 0;
            }
        }

        CompressNode(this, Min, Max, true);
        return ret;
    }

    public Vector3 DecompressMin(CompressedAabbCodec codec, Vector3 parentMin, Vector3 parentMax)
    {
        float x = (codec.m_xyz[0] >> 4) * (codec.m_xyz[0] >> 4) * (1.0f / 226.0f) * (parentMax.X - parentMin.X) + parentMin.X;
        float y = (codec.m_xyz[1] >> 4) * (codec.m_xyz[1] >> 4) * (1.0f / 226.0f) * (parentMax.Y - parentMin.Y) + parentMin.Y;
        float z = (codec.m_xyz[2] >> 4) * (codec.m_xyz[2] >> 4) * (1.0f / 226.0f) * (parentMax.Z - parentMin.Z) + parentMin.Z;
        return new Vector3(x, y, z);
    }

    public Vector3 DecompressMax(CompressedAabbCodec codec, Vector3 parentMin, Vector3 parentMax)
    {
        float x = -((codec.m_xyz[0] & 0x0F) * (codec.m_xyz[0] & 0x0F)) * (1.0f / 226.0f) * (parentMax.X - parentMin.X) + parentMax.X;
        float y = -((codec.m_xyz[1] & 0x0F) * (codec.m_xyz[1] & 0x0F)) * (1.0f / 226.0f) * (parentMax.Y - parentMin.Y) + parentMax.Y;
        float z = -((codec.m_xyz[2] & 0x0F) * (codec.m_xyz[2] & 0x0F)) * (1.0f / 226.0f) * (parentMax.Z - parentMin.Z) + parentMax.Z;
        return new Vector3(x, y, z);
    }

    public List<Aabb6BytesCodec> BuildAxis6Tree()
    {
        var ret = new List<Aabb6BytesCodec>();

        void CompressNode(BVNode node, Vector3 pbbmin, Vector3 pbbmax, bool root = false)
        {
            int currindex = ret.Count();
            var compressed = new Aabb6BytesCodec();
            ret.Add(compressed);

            // Compress the bounding box
            compressed.m_xyz[0] = CompressDim(node.Min.X, node.Max.X, pbbmin.X, pbbmax.X);
            compressed.m_xyz[1] = CompressDim(node.Min.Y, node.Max.Y, pbbmin.Y, pbbmax.Y);
            compressed.m_xyz[2] = CompressDim(node.Min.Z, node.Max.Z, pbbmin.Z, pbbmax.Z);

            // Read back the decompressed bounding box to use as reference for next compression
            var min = DecompressMin(compressed, pbbmin, pbbmax);
            var max = DecompressMax(compressed, pbbmin, pbbmax);
            if (node.IsLeaf)
            {
                uint data = node.Primitive;
                compressed.m_loData = (ushort)(data & 0xFFFF);
                compressed.m_hiData = (byte)(data >> 16 & 0x7F);
            }
            else
            {
                // Add the left as the very next node
                CompressNode(node.Left, min, max);

                // Encode the index of the right then add it. The index should
                // always be even
                ushort data = (ushort)((ret.Count() - currindex) / 2);
                compressed.m_loData = (ushort)(data & 0xFFFF);
                compressed.m_hiData = (byte)(data >> 16 & 0x7F | 0x80);

                // Now encode the right
                CompressNode(node.Right, min, max);
            }
            if (root)
            {
                compressed.m_xyz[0] = 0;
                compressed.m_xyz[1] = 0;
                compressed.m_xyz[2] = 0;
            }
        }

        CompressNode(this, Min, Max, true);
        return ret;
    }
}