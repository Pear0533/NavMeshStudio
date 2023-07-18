using Microsoft.Xna.Framework.Graphics;
using SoulsFormats;
using Color = Microsoft.Xna.Framework.Color;

namespace NavMeshStudio;

public sealed class MPNode : GeoNode
{
    private readonly string MapPieceBndFilePath;

    public MPNode(string name, string mapPieceBndFilePath)
    {
        Name = name;
        MapPieceBndFilePath = mapPieceBndFilePath;
        Process();
    }

    // TODO: Cleanup this method

    protected override void Process()
    {
        List<VertexPositionColor> vertices = new();
        List<VertexPositionColor> facesets = new();
        Color facesetColor = Color.Gray;
        FLVER2 mapPiece = MapUtils.ReadFLVERFromBND(MapPieceBndFilePath);
        mapPiece.Meshes.ForEach(i =>
        {
            List<FLVER.Vertex[]> faces = i.GetFaces();
            faces.ForEach(x =>
            {
                Vector3 vert1Position = new(x[0].Position.X, x[0].Position.Z, x[0].Position.Y);
                Vector3 vert2Position = new(x[1].Position.X, x[1].Position.Z, x[1].Position.Y);
                Vector3 vert3Position = new(x[2].Position.X, x[2].Position.Z, x[2].Position.Y);
                vertices.AddRange(new[]
                {
                    new VertexPositionColor(vert1Position, facesetColor),
                    new VertexPositionColor(vert2Position, facesetColor),
                    new VertexPositionColor(vert1Position, facesetColor),
                    new VertexPositionColor(vert3Position, facesetColor),
                    new VertexPositionColor(vert2Position, facesetColor),
                    new VertexPositionColor(vert3Position, facesetColor)
                });
                facesets.AddRange(new[]
                {
                    new VertexPositionColor(vert1Position, facesetColor),
                    new VertexPositionColor(vert3Position, facesetColor),
                    new VertexPositionColor(vert2Position, facesetColor),
                    new VertexPositionColor(vert1Position, facesetColor),
                    new VertexPositionColor(vert3Position, facesetColor),
                    new VertexPositionColor(vert2Position, facesetColor),
                    new VertexPositionColor(vert1Position, facesetColor),
                    new VertexPositionColor(vert2Position, facesetColor),
                    new VertexPositionColor(vert3Position, facesetColor)
                });
            });
        });
        Vertices = vertices.ToList();
        Facesets = facesets.ToList();
    }
}