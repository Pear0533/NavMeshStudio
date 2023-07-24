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
        return fileData == null ? null : new StudioFile<T>(filePath);
    }

    public static string GetSaveDialogFilter()
    {
        return @"NVA JSON (*.nvajson)|*.nvajson|NVMHKTBND JSON (*.nvmhktbndjson)|*.nvmhktbndjson|NVMHKTBND (*.nvmhktbnd)|*.nvmhktbnd";
    }

    public static bool OpenMsbFile()
    {
        StudioFile<MSBE>? msb = OpenFile<MSBE>(@"MSB File (*.msb, *.msb.dcx)|*.msb;*.msb.dcx");
        if (msb != null) Cache.Msb = msb;
        return msb != null;
    }
}