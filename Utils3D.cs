using HKLib.hk2018;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Vector4 = System.Numerics.Vector4;

namespace NavMeshStudio;

public static class Utils3D
{
    public static bool RayIntersectsTriangle(Ray ray, List<Vector3> vertices, out Vector3 intersection)
    {
        intersection = Vector3.Zero;
        Vector3 e1 = vertices[1] - vertices[0];
        Vector3 e2 = vertices[2] - vertices[0];
        Vector3 h = Vector3.Cross(ray.Direction, e2);
        float a = Vector3.Dot(e1, h);
        if (a is > -float.Epsilon and < float.Epsilon) return false;
        float f = 1.0f / a;
        Vector3 s = ray.Position - vertices[0];
        float u = f * Vector3.Dot(s, h);
        if (u is < 0.0f or > 1.0f) return false;
        Vector3 q = Vector3.Cross(s, e1);
        float v = f * Vector3.Dot(ray.Direction, q);
        if (v < 0.0f || u + v > 1.0f) return false;
        float t = f * Vector3.Dot(e2, q);
        if (!(t > float.Epsilon)) return false;
        intersection = ray.Position + ray.Direction * t;
        return true;
    }

    public static System.Numerics.Vector3 DecompressSharedVertex(ulong vertex, Vector4 bbMin, Vector4 bbMax)
    {
        float scaleX = (bbMax.X - bbMin.X) / ((1 << 21) - 1);
        float scaleY = (bbMax.Y - bbMin.Y) / ((1 << 21) - 1);
        float scaleZ = (bbMax.Z - bbMin.Z) / ((1 << 22) - 1);
        float x = (vertex & 0x1FFFFF) * scaleX + bbMin.X;
        float y = (vertex >> 21 & 0x1FFFFF) * scaleY + bbMin.Y;
        float z = (vertex >> 42 & 0x3FFFFF) * scaleZ + bbMin.Z;
        return new System.Numerics.Vector3(x, y, z);
    }

    public static System.Numerics.Vector3 DecompressPackedVertex(uint vertex, Vector3 scale, Vector3 offset)
    {
        float x = (vertex & 0x7FF) * scale.X + offset.X;
        float y = (vertex >> 11 & 0x7FF) * scale.Y + offset.Y;
        float z = (vertex >> 22 & 0x3FF) * scale.Z + offset.Z;
        return new System.Numerics.Vector3(x, y, z);
    }

    public static Vector3 TransformVertex(this System.Numerics.Vector3 vertex, hknpBodyCinfo collisionInfo)
    {
        Vector3 newVert = new(vertex.X, vertex.Y, vertex.Z);
        Vector3 trans = new(collisionInfo.m_position.X, collisionInfo.m_position.Y, collisionInfo.m_position.Z);
        return Vector3.Transform(newVert, collisionInfo.m_orientation) + trans;
    }

    public static List<GeoElement> GetVertices(IReadOnlyList<Vector3> vertices, Color facesetColor)
    {
        List<GeoElement> vertexPositions = new();
        vertexPositions.AddRange(new[]
        {
            new GeoElement(new VertexPositionColor(vertices[0], facesetColor)),
            new GeoElement(new VertexPositionColor(vertices[1], facesetColor)),
            new GeoElement(new VertexPositionColor(vertices[0], facesetColor)),
            new GeoElement(new VertexPositionColor(vertices[2], facesetColor)),
            new GeoElement(new VertexPositionColor(vertices[1], facesetColor)),
            new GeoElement(new VertexPositionColor(vertices[2], facesetColor))
        });
        return vertexPositions;
    }

    public static List<GeoElement> GetFacesets(IReadOnlyList<Vector3> vertices, Color facesetColor)
    {
        List<GeoElement> facesets = new();
        facesets.AddRange(new[]
        {
            new GeoElement(new VertexPositionColor(vertices[0], facesetColor)),
            new GeoElement(new VertexPositionColor(vertices[2], facesetColor)),
            new GeoElement(new VertexPositionColor(vertices[1], facesetColor)),
            new GeoElement(new VertexPositionColor(vertices[0], facesetColor)),
            new GeoElement(new VertexPositionColor(vertices[2], facesetColor)),
            new GeoElement(new VertexPositionColor(vertices[1], facesetColor)),
            new GeoElement(new VertexPositionColor(vertices[0], facesetColor)),
            new GeoElement(new VertexPositionColor(vertices[1], facesetColor)),
            new GeoElement(new VertexPositionColor(vertices[2], facesetColor))
        });
        return facesets;
    }

    public static Vector3 ToVector3(this Vector4 vector)
    {
        return new Vector3(vector.X, vector.Y, vector.Z);
    }

    public static List<int> GetIndices(List<GeoElement> vertices)
    {
        int index = 0;
        List<int> indices = new();
        for (int i = 0; i < vertices.Count - 1; i++)
        {
            indices.AddRange(new[] { index, index + 1, index + 2 });
            index += 3;
        }
        return indices;
    }

    public static void FlipYZ(ref this Vector3 vector)
    {
        float x = vector.X;
        float y = vector.Y;
        float z = vector.Z;
        vector = new Vector3(x, z, y);
    }

    public static Vector3 NormalizeXnaVector3(this Vector3 vector)
    {
        float x = vector.X;
        float y = vector.Y;
        float z = vector.Z;
        float num = MathF.Sqrt(x * x + y * y + z * z);
        num = 1f / num;
        x *= num;
        y *= num;
        z *= num;
        return new Vector3(x, y, z);
    }

    public static Vector3 CrossProduct(Vector3 vector1, Vector3 vector2)
    {
        float x1 = vector1.X;
        float y1 = vector1.Y;
        float z1 = vector1.Z;
        float x2 = vector2.X;
        float y2 = vector2.Y;
        float z2 = vector2.Z;
        return new Vector3(y1 * z2 - z1 * y2, z1 * x2 - x1 * z2, x1 * y2 - y1 * x2);
    }

    public static float DotProduct(Vector3 vector1, Vector3 vector2)
    {
        float x1 = vector1.X;
        float y1 = vector1.Y;
        float z1 = vector1.Z;
        float x2 = vector2.X;
        float y2 = vector2.Y;
        float z2 = vector2.Z;
        return x1 * x2 + y1 * y2 + z1 * z2;
    }

    public static System.Numerics.Vector3 RotatePoint(this System.Numerics.Vector3 point, float pitch, float roll, float yaw)
    {
        System.Numerics.Vector3 rotatedPoint = new(0, 0, 0);
        double cosa = Math.Cos(yaw);
        double sina = Math.Sin(yaw);
        double cosb = Math.Cos(pitch);
        double sinb = Math.Sin(pitch);
        double cosc = Math.Cos(roll);
        double sinc = Math.Sin(roll);
        double Axx = cosa * cosb;
        double Axy = cosa * sinb * sinc - sina * cosc;
        double Axz = cosa * sinb * cosc + sina * sinc;
        double Ayx = sina * cosb;
        double Ayy = sina * sinb * sinc + cosa * cosc;
        double Ayz = sina * sinb * cosc - cosa * sinc;
        double Azx = -sinb;
        double Azy = cosb * sinc;
        double Azz = cosb * cosc;
        float px = point.X;
        float py = point.Y;
        float pz = point.Z;
        rotatedPoint.X = (float)(Axx * px + Axy * py + Axz * pz);
        rotatedPoint.Y = (float)(Ayx * px + Ayy * py + Ayz * pz);
        rotatedPoint.Z = (float)(Azx * px + Azy * py + Azz * pz);
        return rotatedPoint;
    }

    public static System.Numerics.Vector3 RotateLine(System.Numerics.Vector3 point, System.Numerics.Vector3 org, System.Numerics.Vector3 direction, double theta)
    {
        double x = point.X;
        double y = point.Y;
        double z = point.Z;
        double a = org.X;
        double b = org.Y;
        double c = org.Z;
        double nu = direction.X / direction.Length();
        double nv = direction.Y / direction.Length();
        double nw = direction.Z / direction.Length();
        double[] rP = new double[3];
        rP[0] = (a * (nv * nv + nw * nw) - nu * (b * nv + c * nw - nu * x - nv * y - nw * z)) * (1 - Math.Cos(theta))
            + x * Math.Cos(theta)
            + (-c * nv + b * nw - nw * y + nv * z) * Math.Sin(theta);
        rP[1] = (b * (nu * nu + nw * nw) - nv * (a * nu + c * nw - nu * x - nv * y - nw * z)) * (1 - Math.Cos(theta))
            + y * Math.Cos(theta)
            + (c * nu - a * nw + nw * x - nu * z) * Math.Sin(theta);
        rP[2] = (c * (nu * nu + nv * nv) - nw * (a * nu + b * nv - nu * x - nv * y - nw * z)) * (1 - Math.Cos(theta))
            + z * Math.Cos(theta)
            + (-b * nu + a * nv - nv * x + nu * y) * Math.Sin(theta);
        System.Numerics.Vector3 rotatedLine = new((float)rP[0], (float)rP[1], (float)rP[2]);
        return rotatedLine;
    }

    public static System.Numerics.Vector3 CrossProduct(System.Numerics.Vector3 vector1, System.Numerics.Vector3 vector2)
    {
        float x1 = vector1.X;
        float y1 = vector1.Y;
        float z1 = vector1.Z;
        float x2 = vector2.X;
        float y2 = vector2.Y;
        float z2 = vector2.Z;
        return new System.Numerics.Vector3(y1 * z2 - z1 * y2, z1 * x2 - x1 * z2, x1 * y2 - y1 * x2);
    }

    public static System.Numerics.Vector3 NormalizeNumericsVector3(this System.Numerics.Vector3 vector)
    {
        float length = vector.Length();
        return length == 0 ? new System.Numerics.Vector3() : new System.Numerics.Vector3(vector.X / length, vector.Y / length, vector.Z / length);
    }
}