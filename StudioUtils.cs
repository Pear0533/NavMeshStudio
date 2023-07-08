using Newtonsoft.Json;

namespace NavMeshStudio;

public static class StudioUtils
{
    private const string AppName = "NavMesh Studio";
    private const string Version = "1.0";

    public static void SetVersionString(this NavMeshStudio studio)
    {
        studio.versionLabel.Text += ' ' + $@"{Version}";
    }

    public static void SetWindowTitleFilePath(this NavMeshStudio studio, string filePath)
    {
        studio.Text = $@"{AppName} - {filePath}";
    }

    public static void ResetStatus(this NavMeshStudio studio)
    {
        studio.statusLabel.Text = @"Ready";
    }

    public static void UpdateStatus(this NavMeshStudio studio, string message)
    {
        studio.statusLabel.Text = message;
    }

    public static void OpenMap(this NavMeshStudio studio)
    {
        if (!FileIO.OpenNvmHktBndFile()) return;
        SetWindowTitleFilePath(studio, Cache.NvmHktBnd?.Path!);
        // TODO: Function
        studio.saveNVMJSONToolStripMenuItem.Enabled = true;
        studio.saveNVMJSONIconButton.Enabled = true;
    }

    public static async Task SaveNvmJson(this NavMeshStudio studio)
    {
        UpdateStatus(studio, "Reading navmesh geometry...");
        if (!await NavMeshUtils.ReadNavMeshGeometry())
        {
            ResetStatus(studio);
            return;
        }
        UpdateStatus(studio, "Waiting for user...");
        string rootJsonString = JsonConvert.SerializeObject(Cache.NavMeshJson, Formatting.Indented);
        SaveFileDialog dialog = new() { FileName = $"{Cache.NvmHktBnd?.FileName}.json", Filter = @"JSON File (*.json)|*.json" };
        if (dialog.ShowDialog() != DialogResult.OK)
        {
            ResetStatus(studio);
            return;
        }
        UpdateStatus(studio, "Saving NVMJSON to disk...");
        await File.WriteAllTextAsync(dialog.FileName, rootJsonString);
        ResetStatus(studio);
    }

    public static void RegisterDefaultEvents(this NavMeshStudio studio)
    {
        studio.openMapToolStripMenuItem.Click += (_, _) => OpenMap(studio);
        studio.saveNVMJSONToolStripMenuItem.Click += async (_, _) => await SaveNvmJson(studio);
        studio.saveNVMJSONToolStripMenuItem.Enabled = false;
        studio.KeyDown += async (_, e) =>
        {
            switch (e.Control)
            {
                case true when e.KeyCode == Keys.O:
                    OpenMap(studio);
                    break;
                case true when e.KeyCode == Keys.S:
                    await SaveNvmJson(studio);
                    break;
            }
        };
        studio.openMapIconButton.Click += (_, _) => OpenMap(studio);
        studio.saveNVMJSONIconButton.Click += async (_, _) => await SaveNvmJson(studio);
        studio.saveNVMJSONIconButton.Enabled = false;
    }
}