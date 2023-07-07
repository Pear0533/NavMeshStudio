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

    public static void OpenNavMesh(this NavMeshStudio studio)
    {
        if (!FileIO.OpenNvmHktBndFile()) return;
        SetWindowTitleFilePath(studio, Cache.NvmHktBnd?.Path!);
        // TODO: Function
        studio.exportJSONToolStripMenuItem.Enabled = true;
        studio.exportJsonIconButton.Enabled = true;
    }

    public static async Task ExportNavMeshJson(this NavMeshStudio studio)
    {
        UpdateStatus(studio, "Reading navmesh geometry...");
        if (!await NavMeshUtils.ReadNavMeshGeometry())
        {
            ResetStatus(studio);
            return;
        }
        UpdateStatus(studio, "Waiting for export...");
        string rootJsonString = JsonConvert.SerializeObject(Cache.NavMeshJson, Formatting.Indented);
        SaveFileDialog dialog = new() { FileName = $"{Cache.NvmHktBnd?.FileName}.json", Filter = @"JSON File (*.json)|*.json" };
        if (dialog.ShowDialog() != DialogResult.OK)
        {
            ResetStatus(studio);
            return;
        }
        UpdateStatus(studio, "Exporting to JSON...");
        await File.WriteAllTextAsync(dialog.FileName, rootJsonString);
        ResetStatus(studio);
    }

    public static void RegisterDefaultEvents(this NavMeshStudio studio)
    {
        studio.openToolStripMenuItem.Click += (_, _) => OpenNavMesh(studio);
        studio.exportJSONToolStripMenuItem.Click += async (_, _) => await ExportNavMeshJson(studio);
        studio.exportJSONToolStripMenuItem.Enabled = false;
        studio.KeyDown += async (_, e) =>
        {
            switch (e.Control)
            {
                case true when e.KeyCode == Keys.O:
                    OpenNavMesh(studio);
                    break;
                case true when e.KeyCode == Keys.E:
                    await ExportNavMeshJson(studio);
                    break;
            }
        };
        studio.openIconButton.Click += (_, _) => OpenNavMesh(studio);
        studio.exportJsonIconButton.Click += async (_, _) => await ExportNavMeshJson(studio);
        studio.exportJsonIconButton.Enabled = false;
    }
}