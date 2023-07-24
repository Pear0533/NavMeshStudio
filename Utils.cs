using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Color = Microsoft.Xna.Framework.Color;

namespace NavMeshStudio;

public static class Utils
{
    public static string AppRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "";
    public static string ResourcesPath = $"{AppRootPath}\\Resources";

    public static void RegisterCharacterEncodings()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public static void ShowInformationDialog(string message)
    {
        MessageBox.Show(message, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public static string ShowOpenFileDialog(string filter = "")
    {
        OpenFileDialog dialog = new() { Filter = filter };
        return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : "";
    }

    public static string RemoveFileExtensions(string input)
    {
        return input[..input.IndexOf('.')];
    }

    public static string MoveUpDirectory(string path, int steps)
    {
        for (int i = 0; i < steps; i++)
            path = Path.GetDirectoryName(path) ?? path;
        return path;
    }

    public static Color GetRandomColor()
    {
        int r = (byte)(Random.Shared.Next(128) + 127);
        int g = (byte)(Random.Shared.Next(128) + 127);
        int b = (byte)(Random.Shared.Next(128) + 127);
        return Color.FromNonPremultiplied(r, g, b, 255);
    }

    public static bool IsMouseOverControl(Control control)
    {
        return control.ClientRectangle.Contains(control.PointToClient(Cursor.Position));
    }

    public static JObject? ToJson(object? obj)
    {
        return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
    }
}