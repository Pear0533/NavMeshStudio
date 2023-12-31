﻿using HKLib.hk2018;
using HKLib.Serialization.hk2018.Binary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoulsFormats;

namespace NavMeshStudio;

public static class StudioUtils
{
    private const string AppName = "NavMesh Studio";
    private const string Version = "1.3";

    public static void SetVersionString(this NavMeshStudio studio)
    {
        studio.versionLabel.Text += ' ' + $@"{Version}";
    }

    private static void SetWindowTitleFilePath(this NavMeshStudio studio, string filePath)
    {
        studio.Text = $@"{AppName} - {filePath}";
    }

    private static void ActivateWaitingStatus(this NavMeshStudio studio)
    {
        studio.statusLabel.Text = @"Waiting for user...";
    }

    public static void ResetStatus(this NavMeshStudio studio)
    {
        studio.statusLabel.Text = @"Ready";
    }

    public static async Task UpdateStatus(this NavMeshStudio studio, string message, int delay)
    {
        studio.statusLabel.Text = message;
        await Task.Delay(delay);
    }

    public static void UpdateStatus(this NavMeshStudio studio, string message)
    {
        studio.statusLabel.Text = message;
    }

    public static void ToggleOpenFileMenuOption(this NavMeshStudio studio, bool enabled = false)
    {
        studio.openToolStripMenuItem.Enabled = enabled;
        studio.openToolStripButton.Enabled = enabled;
    }

    public static void ToggleSaveAsFileMenuOption(this NavMeshStudio studio, bool enabled = false)
    {
        studio.saveAsToolStripMenuItem.Enabled = enabled;
        studio.saveAsToolStripButton.Enabled = enabled;
    }

    public static void RegisterConsole(this NavMeshStudio studio)
    {
        Cache.Console = new Console(studio);
    }

    private static void RunViewer(this NavMeshStudio studio)
    {
        new Thread(() =>
        {
            Thread.CurrentThread.IsBackground = true;
            Cache.SceneGraph = new SceneGraph(studio);
            if (!Cache.Viewer.IsInitialized)
            {
                Cache.Viewer = new Viewer(studio);
                Cache.Viewer.RefreshGeometry(true);
                Cache.Viewer.Run();
            }
            else Cache.Viewer.RefreshGeometry(true);
        }).Start();
        if (!Cache.Viewer.IsInitialized)
            Cache.Console.Write("Started main thread for viewer");
    }

    // TODO: Allow for modded map files to be opened

    private static async Task Open(this NavMeshStudio studio)
    {
        ActivateWaitingStatus(studio);
        if (!FileIO.OpenMsbFile())
        {
            ResetStatus(studio);
            return;
        }
        Cache.Console.Write($"Read {Cache.Msb?.Path}");
        if (!studio.SetMapDependenciesPath()) return;
        Cache.Clear();
        // TODO: Determine a way to automatically invoke UI operations
        studio.Invoke(() => ToggleOpenFileMenuOption(studio));
        studio.Invoke(() => ToggleSaveAsFileMenuOption(studio));
        SetWindowTitleFilePath(studio, Cache.Msb?.Path!);
        await NavMeshUtils.ReadNavMeshGeometry(studio);
        await CollisionUtils.ReadCollisionGeometry(studio);
        MapUtils.ReadMapPieceGeometry();
        ResetStatus(studio);
        RunViewer(studio);
    }

    private static async Task<JObject> GetNvmJson(this NavMeshStudio studio)
    {
        UpdateStatus(studio, "Generating nvmhktbnd JSON...");
        return await NavMeshUtils.GenerateNvmJson();
    }

    // TODO: Cleanup

    private static async Task SaveNvmHktBnd(this NavMeshStudio studio, FileDialog dialog)
    {
        UpdateStatus(studio, "Saving nvmhktbnd to file...");
        HavokBinarySerializer serializer = new();
        await Task.Run(() =>
        {
            BND4 bnd = new();
            for (int i = 0; i < Cache.NavMeshes.Count; i++)
            {
                hkaiNavMesh mesh = Cache.NavMeshes[i];
                BinderFile file = new();
                hkRootLevelContainer container = new();
                hkRootLevelContainer.NamedVariant namedVariant = new()
                {
                    m_name = "hkaiNavMesh",
                    m_className = "hkaiNavMesh",
                    m_variant = mesh
                };
                container.m_namedVariants.Add(namedVariant);
                MemoryStream stream = new();
                serializer.Write(container, stream);
                file.Name = $"{i + 1}.hkx";
                file.Bytes = stream.ToArray();
                bnd.Files.Add(file);
            }
            bnd.Write(dialog.FileName);
        });
    }

    private static async Task SaveNavMeshJson(this NavMeshStudio studio, FileDialog dialog)
    {
        JObject? rootJson = dialog.FilterIndex == 1 ? Utils.ToJson(Cache.Nva?.Data) : await GetNvmJson(studio);
        string rootJsonString = JsonConvert.SerializeObject(rootJson, Formatting.Indented);
        UpdateStatus(studio, "Saving navmesh JSON...");
        await File.WriteAllTextAsync(dialog.FileName, rootJsonString);
    }

    private static async Task SaveAs(this NavMeshStudio studio)
    {
        ActivateWaitingStatus(studio);
        SaveFileDialog dialog = new()
        {
            FileName = Utils.RemoveFileExtensions(Cache.Msb?.FileName ?? ""),
            Filter = FileIO.GetSaveDialogFilter()
        };
        if (dialog.ShowDialog() != DialogResult.OK)
        {
            ResetStatus(studio);
            return;
        }
        if (dialog.FilterIndex == 3) await SaveNvmHktBnd(studio, dialog);
        else await studio.SaveNavMeshJson(dialog);
        Cache.Console.Write($"Saved {dialog.FileName} to file");
        ResetStatus(studio);
    }

    private static void RegisterViewerEvents(KeyEventArgs e)
    {
        if (!new[] { Keys.W, Keys.A, Keys.S, Keys.D }.Contains(e.KeyCode)) return;
        e.SuppressKeyPress = true;
        if (Cache.Viewer.IsInitialized) Cache.Viewer.ViewerWindow.Invoke(Cache.Viewer.ViewerWindow.Focus);
    }

    public static void RegisterFormEvents(this NavMeshStudio studio)
    {
        studio.openToolStripMenuItem.Click += async (_, _) => await Open(studio);
        studio.saveAsToolStripMenuItem.Click += async (_, _) => await SaveAs(studio);
        studio.KeyDown += async (_, e) =>
        {
            switch (e.Control)
            {
                case true when e.KeyCode == Keys.O:
                    await Open(studio);
                    break;
                case true when e.KeyCode == Keys.S:
                    await SaveAs(studio);
                    break;
            }
            RegisterViewerEvents(e);
        };
        studio.openToolStripButton.Click += async (_, _) => await Open(studio);
        studio.saveAsToolStripButton.Click += async (_, _) => await SaveAs(studio);
    }
}