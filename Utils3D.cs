namespace NavMeshStudio;

public class Utils3D
{
    public static Vector3 RotatePoint(Vector3 point, float pitch, float roll, float yaw)
    {
        Vector3 rotatedPoint = new(0, 0, 0);
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

    public static Vector3 RotateLine(Vector3 point, Vector3 org, Vector3 direction, double theta)
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
        Vector3 rotatedLine = new((float)rP[0], (float)rP[1], (float)rP[2]);
        return rotatedLine;
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

    public static Vector3 Normalize(Vector3 vector)
    {
        float length = vector.Length();
        return length == 0 ? new Vector3() : new Vector3(vector.X / length, vector.Y / length, vector.Z / length);
    }
}