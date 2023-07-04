using HKLib.hk2018;
using HKLib.Serialization.hk2018.Binary;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoulsFormats;
using static NavMeshStudio.Utils;

namespace NavMeshStudio;

public class NavMeshUtils
{
    private static hkReferencedObject? GetReferencedObject(hkRootLevelContainer container, int index)
    {
        return container.m_namedVariants.ElementAtOrDefault(index)?.m_variant;
    }

    // TODO: Button
    public static void ExportNavMeshJson()
    {
        ReadNavMeshGeometry();
        string rootJsonString = JsonConvert.SerializeObject(Cache.NavMeshJson, Formatting.Indented);
        SaveFileDialog dialog = new() { Filter = @"JSON File (*.json)|*.json" };
        if (dialog.ShowDialog() != DialogResult.OK) return;
        File.WriteAllText(dialog.FileName, rootJsonString);
    }

    private static void ReadNavMeshGeometry()
    {
        if (Cache.NvmHktBnd == null) return;
        HavokBinarySerializer serializer = new();
        for (int i = 0; i < Cache.NvmHktBnd.Data.Files.Count; i++)
        {
            BinderFile file = Cache.NvmHktBnd.Data.Files[i];
            hkRootLevelContainer rootLevelContainer = (hkRootLevelContainer)serializer.Read(new MemoryStream(file.Bytes));
            hkReferencedObject? navMesh = GetReferencedObject(rootLevelContainer, 0);
            hkReferencedObject? queryMediator = GetReferencedObject(rootLevelContainer, 1);
            hkReferencedObject? userEdgeSetup = GetReferencedObject(rootLevelContainer, 2);
            JObject? navMeshJson = ToJson(navMesh);
            JObject? queryMediatorJson = ToJson(queryMediator);
            JObject? userEdgeSetupJson = ToJson(userEdgeSetup);
            Cache.NavMeshJson[(i + 1).ToString()] = new JObject
            {
                { "NavMesh", navMeshJson },
                { "QueryMediator", queryMediatorJson },
                { "UserEdgeSetup", userEdgeSetupJson }
            };
        }
    }

    // TODO: Button
    public static void LoadEditor()
    {
        NavMeshEditor editor = new();
        editor.ConfigureGeometry();
        editor.Viewer.ShowDialog();
    }
}