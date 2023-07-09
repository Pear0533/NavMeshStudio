﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NavMeshStudio;

public static class StudioUtils
{
    private const string AppName = "NavMesh Studio";
    private const string Version = "1.0";

    public static void SetVersionString(this NavMeshStudio studio)
    {
        studio.versionLabel.Text += ' ' + $@"{Version}";
    }

    private static void SetWindowTitleFilePath(this NavMeshStudio studio, string filePath)
    {
        studio.Text = $@"{AppName} - {filePath}";
    }

    private static void ResetStatus(this NavMeshStudio studio)
    {
        studio.statusLabel.Text = @"Ready";
    }

    public static void UpdateStatus(this NavMeshStudio studio, string message)
    {
        studio.statusLabel.Text = message;
    }

    public static void ToggleStudioControls(this NavMeshStudio studio, bool enabled = false)
    {
        studio.saveAsToolStripMenuItem.Enabled = enabled;
        studio.saveAsToolStripButton.Enabled = enabled;
    }

    private static void Open(this NavMeshStudio studio)
    {
        if (!FileIO.OpenMsbFile()) return;
        if (!FileIO.OpenNvmHktBndFile()) return;
        SetWindowTitleFilePath(studio, Cache.Msb?.Path!);
        ToggleStudioControls(studio, true);
    }

    private static async Task ReadNavMeshGeometry(this NavMeshStudio studio)
    {
        UpdateStatus(studio, "Reading navmesh geometry...");
        if (!await NavMeshUtils.GenerateNvmJson()) ResetStatus(studio);
    }

    private static async Task SaveAs(this NavMeshStudio studio)
    {
        UpdateStatus(studio, "Waiting for user...");
        SaveFileDialog dialog = new()
        {
            FileName = Utils.RemoveFileExtensions(Cache.Msb?.FileName),
            Filter = FileIO.GetSaveDialogFilter()
        };
        if (dialog.ShowDialog() != DialogResult.OK)
        {
            ResetStatus(studio);
            return;
        }
        if (dialog.FilterIndex == 2) await ReadNavMeshGeometry(studio);
        JObject? rootJson = NavMeshUtils.GetNavMeshJson(dialog.FilterIndex);
        string rootJsonString = JsonConvert.SerializeObject(rootJson, Formatting.Indented);
        UpdateStatus(studio, "Saving navmesh JSON...");
        await File.WriteAllTextAsync(dialog.FileName, rootJsonString);
        ResetStatus(studio);
    }

    public static void RegisterFormEvents(this NavMeshStudio studio)
    {
        studio.openToolStripMenuItem.Click += (_, _) => Open(studio);
        studio.saveAsToolStripMenuItem.Click += async (_, _) => await SaveAs(studio);
        studio.KeyDown += async (_, e) =>
        {
            switch (e.Control)
            {
                case true when e.KeyCode == Keys.O:
                    Open(studio);
                    break;
                case true when e.KeyCode == Keys.S:
                    await SaveAs(studio);
                    break;
            }
        };
        studio.openToolStripButton.Click += (_, _) => Open(studio);
        studio.saveAsToolStripButton.Click += async (_, _) => await SaveAs(studio);
    }
}