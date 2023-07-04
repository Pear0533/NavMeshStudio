using SoulsFormats;
using static NavMeshStudio.Utils;

// ReSharper disable UnusedMember.Global

namespace NavMeshStudio;

public class FileIO
{
    public static T? GetFileData<T>(string filePath) where T : SoulsFile<T>, new()
    {
        if (string.IsNullOrWhiteSpace(filePath)) return null;
        bool isFileTypeValid = SoulsFile<T>.IsRead(filePath, out T fileData);
        if (isFileTypeValid) return fileData;
        ShowInformationDialog($"{Path.GetFileName(filePath)} is not valid.");
        return null;
    }

    public static StudioFile<T>? OpenFile<T>(string dialogFilter) where T : SoulsFile<T>, new()
    {
        string filePath = ShowOpenFileDialog(dialogFilter);
        T? fileData = GetFileData<T>(filePath);
        return fileData == null ? null : new StudioFile<T>(filePath, fileData);
    }

    public static bool OpenNvaFile()
    {
        Cache.Nva = OpenFile<NVA>(@"NVA File (*.nva, *.nva.dcx)|*.nva;*.nva.dcx");
        return Cache.Nva != null;
    }

    public static bool OpenNvmHktBndFile()
    {
        Cache.NvmHktBnd = OpenFile<BND4>(@"NVMHKTBND File (*.nvmhktbnd.dcx)|*.nvmhktbnd.dcx");
        return Cache.NvmHktBnd != null;
    }

    public static bool OpenMsbFile()
    {
        Cache.Msb = OpenFile<MSB3>(@"MSB File (*.msb, *.msb.dcx)|*.msb;*.msb.dcx");
        return Cache.Msb != null;
    }
}