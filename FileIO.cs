using SoulsFormats;
using static NavMeshStudio.Utils;

// ReSharper disable UnusedMember.Global

namespace NavMeshStudio;

public class FileIO
{
    public static T? GetFileData<T>(string filePath) where T : SoulsFile<T>, new()
    {
        if (string.IsNullOrEmpty(filePath)) return null;
        bool isFileTypeValid = SoulsFile<T>.IsRead(filePath, out T fileData);
        if (isFileTypeValid) return fileData;
        ShowInformationDialog($"{Path.GetFileName(filePath)} is not valid.");
        return null;
    }

    public static StudioFile<T>? OpenFile<T>(string dialogFilter) where T : SoulsFile<T>, new()
    {
        string filePath = ShowOpenFileDialog(dialogFilter);
        T? fileData = GetFileData<T>(filePath);
        return fileData == null ? null : new StudioFile<T>(filePath, Path.GetFileName(filePath), fileData);
    }

    public static string GetSaveDialogFilter()
    {
        return @"NVA JSON (*.nvajson)|*.nvajson|NVMHKTBND JSON (*.nvmhktbndjson)|*.nvmhktbndjson|NVMHKTBND (*.nvmhktbnd)|*.nvmhktbnd";
    }

    public static bool OpenNvaFile()
    {
        StudioFile<NVA>? nva = OpenFile<NVA>(@"NVA File (*.nva, *.nva.dcx)|*.nva;*.nva.dcx");
        if (nva != null) Cache.Nva = nva;
        return nva != null;
    }

    public static bool OpenNvmHktBndFile()
    {
        StudioFile<BND4>? nvmhktbnd = OpenFile<BND4>(@"NVMHKTBND File (*.nvmhktbnd, *.nvmhktbnd.dcx)|*.nvmhktbnd;*.nvmhktbnd.dcx");
        if (nvmhktbnd != null) Cache.NvmHktBnd = nvmhktbnd;
        return nvmhktbnd != null;
    }

    public static bool OpenMsbFile()
    {
        StudioFile<MSBE>? msb = OpenFile<MSBE>(@"MSB File (*.msb, *.msb.dcx)|*.msb;*.msb.dcx");
        if (msb != null) Cache.Msb = msb;
        return msb != null;
    }
}