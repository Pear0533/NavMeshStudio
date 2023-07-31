using SoulsFormats;
using Color = Microsoft.Xna.Framework.Color;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace NavMeshStudio;

public sealed class MPNode : GeoNode
{
    private readonly string MapPieceBndFilePath;

    public MPNode(string mapPieceBndFilePath)
    {
        MapPieceBndFilePath = mapPieceBndFilePath;
        Name = Utils.RemoveFileExtensions(Path.GetFileName(MapPieceBndFilePath));
        Process();
    }

    protected override void Process()
    {
        FLVER2 mapPiece = MapUtils.ReadFLVERFromBND(MapPieceBndFilePath);
        mapPiece.Meshes.ForEach(i =>
        {
            List<FLVER.Vertex[]> faces = i.GetFaces();
            faces.ForEach(x =>
            {
                AddVertices(new Vector3[]
                {
                    x[0].Position, x[1].Position, x[2].Position
                }, Color.Gray);
            });
        });
    }
}