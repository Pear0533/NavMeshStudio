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

    public static void OpenNavMesh(this NavMeshStudio studio)
    {
        if (!FileIO.OpenNvmHktBndFile()) return;
        SetWindowTitleFilePath(studio, Cache.NvmHktBnd?.Path!);
    }

    public static void RegisterDefaultEvents(this NavMeshStudio studio)
    {
        studio.openToolStripMenuItem.Click += (_, _) => OpenNavMesh(studio);
        studio.KeyDown += (_, e) =>
        {
            switch (e.Control)
            {
                case true when e.KeyCode == Keys.O:
                    OpenNavMesh(studio);
                    break;
            }
        };
    }
}