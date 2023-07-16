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

    public static string? RemoveFileExtensions(string? input)
    {
        return input?[..input.IndexOf('.')];
    }

    public static Color GetRandomColor()
    {
        int r = Random.Shared.Next(256);
        int g = Random.Shared.Next(256);
        int b = Random.Shared.Next(256);
        return Color.FromNonPremultiplied(r, g, b, 256);
    }

    public static JObject? ToJson(object? obj)
    {
        return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
    }
}