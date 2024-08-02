using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

namespace Editors.Addressables
{
    public static class AddressableEditorUtilities
    {
        public static void AddToAddressable (UnityEngine.Object asset, string groupName)
        {
            AddToAddressable(asset, groupName, "");
        }

        public static void AddToAddressable (UnityEngine.Object asset, string groupName, string label)
        {
            string assetpath = AssetDatabase.GetAssetPath(asset);
            string guid = AssetDatabase.AssetPathToGUID(assetpath);

            CreateAddressableGroup(groupName);
            AddAssetToAddressableGroup(assetpath, groupName);
            CreateAddressableLabel(label, AddressableAssetSettingsDefaultObject.Settings.FindAssetEntry(guid));
        }

        private static void CreateAddressableGroup (string name)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings.FindGroup(name) == null) settings.CreateGroup(name, false, false, false, null);
        }

        public static void AddAssetToAddressableGroup (string path, string groupName)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

            AddressableAssetGroup group = settings.FindGroup(groupName);
            if (group == null) throw new Exception($"Addressable : can't find group {groupName}");
            _ = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), group, false, true)
                ?? throw new Exception($"Addressable : can't add {path} to group {groupName}");
        }

        public static void CreateAddressableLabel (string name, AddressableAssetEntry entry)
        {
            entry.SetLabel(name, true, true, true);
        }
    }
}