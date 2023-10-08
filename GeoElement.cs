using Microsoft.Xna.Framework.Graphics;

namespace NavMeshStudio;

public class GeoElement
{
    public VertexPositionColor BaseColorData;
    public VertexPositionColor Data;

    public GeoElement(VertexPositionColor data)
    {
        BaseColorData = data;
        Data = data;
    }
}