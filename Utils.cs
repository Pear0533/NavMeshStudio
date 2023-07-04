using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NavMeshStudio;

public class Utils
{
    public static string? AppRootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    public static string? ResourcesPath = $"{AppRootPath}\\Resources";

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

    public static JObject? ToJson(object? obj)
    {
        return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
    }
}