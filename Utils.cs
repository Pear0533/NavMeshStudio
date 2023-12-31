﻿using System.Diagnostics;
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
        Cache.Viewer.IsFocused = false;
        MessageBox.Show(message, @"Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Cache.Viewer.IsFocused = true;
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
        return control.Focused && control.ClientRectangle.Contains(control.PointToClient(Cursor.Position));
    }

    public static JObject? ToJson(object? obj)
    {
        return JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
    }

    public static string[] TryDirectoryGetFolders(string folderPath, string searchPattern, SearchOption searchOption)
    {
        try
        {
            return Directory.GetDirectories(folderPath, searchPattern, searchOption);
        }
        catch
        {
            return Array.Empty<string>();
        }
    }

    public static bool IsMainWindowFocused()
    {
        IntPtr activatedHandle = GetForegroundWindow();
        if (activatedHandle == IntPtr.Zero) return false;
        int procId = Process.GetCurrentProcess().Id;
        GetWindowThreadProcessId(activatedHandle, out int activeProcId);
        return activeProcId == procId;
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
}