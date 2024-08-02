using System.IO;
using UnityEngine;
using UnityEditor;

namespace Editors.Shared
{
    [InitializeOnLoad]
    public static class ScriptRefreshControl
    {

    #region Fields
        private static bool _projectChanged;
        private static readonly FileSystemWatcher _fileSystemWatcher;

        private const string _menuItemPath = "Tools/Scripts/Auto Refresh";
    #endregion Fields


        static ScriptRefreshControl ()
        {
            EditorApplication.playModeStateChanged += Refresh;
            EditorApplication.projectChanged += ProjectChanged;

            _fileSystemWatcher = new FileSystemWatcher
            {
                Path = Application.dataPath,
                IncludeSubdirectories = true,
                Filter = "*.cs;*.asmdef;*.inputactions",
                NotifyFilter = NotifyFilters.LastWrite
            };

            _fileSystemWatcher.Changed += FileSystemChanged;

            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void Refresh(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode) {
                RefreshAssets();
            }
        }

        private static void ProjectChanged ()
        {
            _projectChanged = true;
        }

        private static void FileSystemChanged (object sender, FileSystemEventArgs e)
        {
            _projectChanged = true;
        }

        [MenuItem(_menuItemPath)]
        private static void AutoRefreshToggle ()
        {
            int status = EditorPrefs.GetInt("kAutoRefresh");
            int newStatus = status == 1 ? 0 : 1;

            EditorPrefs.SetInt("kAutoRefresh", newStatus);

            if (newStatus == 1) EditorApplication.UnlockReloadAssemblies();
            else EditorApplication.LockReloadAssemblies();

            CheckAutoRefresh();
        }

        [MenuItem(_menuItemPath, true)]
        private static bool AutoRefreshToggleValidation ()
        {
            int status = EditorPrefs.GetInt("kAutoRefresh");
            Menu.SetChecked(_menuItemPath, status == 1);
            return true;
        }

        [MenuItem(_menuItemPath + " %r")]
        private static void Refresh ()
        {
            RefreshAssets();
        }

        // This will executed after refresh
        [InitializeOnLoadMethod]
        private static void Initialize ()
        {
            if (EditorPrefs.HasKey("kAutoRefresh") == false) {
                EditorPrefs.SetInt("kAutoRefresh", 1);
                Menu.SetChecked(_menuItemPath, true);
            }

            int status = EditorPrefs.GetInt("kAutoRefresh");
            if (status == 1) EditorApplication.UnlockReloadAssemblies();
            else EditorApplication.LockReloadAssemblies();
            CheckAutoRefresh();
        }

        private static void RefreshAssets ()
        {
            Debug.Log($"Request refresh assets. (Project changed: {_projectChanged})");
            if (!_projectChanged) return;

            EditorApplication.UnlockReloadAssemblies();
            AssetDatabase.Refresh();
        }
    
        private static void CheckAutoRefresh ()
        {
            int status = EditorPrefs.GetInt("kAutoRefresh");
            _projectChanged = status == 1; // If auto refresh is on, project changed is always true
        }
    }
}