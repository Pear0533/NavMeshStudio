using HKLib.hk2018;
using HKLib.Serialization.hk2018.Binary;
using SoulsFormats;

namespace NavMeshStudio;

public static class HavokUtils
{
    public static hkReferencedObject GetReferencedObject(this hkRootLevelContainer container, int index)
    {
        return container.m_namedVariants.ElementAtOrDefault(index)?.m_variant ?? new hkReferencedObject();
    }

    public static hknpBodyCinfo GetBodyCollisionInfo(this hkRootLevelContainer container, int index)
    {
        hkReferencedObject sceneData = GetReferencedObject(container, 0);
        if (sceneData is not hknpPhysicsSceneData data) return new hknpBodyCinfo();
        hknpPhysicsSystemData systemData = data.m_systemDatas.ElementAtOrDefault(0) ?? new hknpPhysicsSystemData();
        return systemData.m_bodyCinfos.ElementAtOrDefault(index) ?? new hknpBodyCinfo();
    }

    public static hkRootLevelContainer GetRootLevelContainer(this HavokBinarySerializer serializer, byte[] bytes)
    {
        return (hkRootLevelContainer)serializer.Read(new MemoryStream(bytes));
    }

    public static bool SetCompendium(this HavokBinarySerializer serializer, NavMeshStudio studio, BXF4File bxf4)
    {
        BinderFile? compendiumFile = bxf4.Data.Files.Find(i => i.Name.EndsWith(".compendium.dcx"));
        if (compendiumFile == null)
        {
            studio.UpdateStatus("Couldn't find compendium, skipping...");
            return false;
        }
        byte[] compendiumBytes = DCX.Decompress(compendiumFile.Bytes);
        serializer.LoadCompendium(new MemoryStream(compendiumBytes));
        return true;
    }
}