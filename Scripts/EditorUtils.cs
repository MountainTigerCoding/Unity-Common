#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Editors.Shared
{
    public static class EditorUtils
    {
        /// <summary>
        /// Saves a Unity Object via a File Save Panel
        /// </summary>
        /// <param name="object"></param>
        /// <param name="name"></param>
        /// <param name="extention">File extention, without the period [.] before</param>
        /// <param name="path">Where the file will be saved relative to the project</param>
        /// <returns>Returns true if the object was sucessfully saved</returns>
        public static bool SaveViaDialogue (this ScriptableObject @object, string name, string extention, out string path)
        {
            return SaveObject_Dialogue(@object, name, extention, out path);
        }

        /// <summary>
        /// Saves a Unity Object via a File Save Panel
        /// </summary>
        /// <param name="object"></param>
        /// <param name="defaultName">The default name of the file</param>
        /// <param name="extention">File extention, without the period [.] before</param>
        /// <param name="path">Where the file will be saved relative to the project</param>
        /// <returns>Returns true if the object was sucessfully saved</returns>
        public static bool SaveObject_Dialogue (this Object @object, string defaultName, string extention, out string path)
        {
            path = EditorUtility.SaveFilePanel("Save " + @object.name, "", defaultName, extention);
            if (path == "") return false;

            AssetDatabase.CreateAsset(@object, AbsoluteToRelativePath(path));
            return true;
        }

        /// <summary>
        /// Converts an absolute path to one which begins with "Assets"
        /// </summary>
        /// <param name="absolutepath"></param>
        /// <returns></returns>
        public static string AbsoluteToRelativePath (string absolutepath)
        {
            if (absolutepath.StartsWith(Application.dataPath)) {
                return "Assets" + absolutepath[Application.dataPath.Length..];
            } else return absolutepath;
        }

        /// <summary>
        /// Finds all assets under the "Assets" folder using search filters.
        /// Works on only files compatible with Unity.Object 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nameFilter">Optional search filter</param>
        /// <returns>The objects found</returns>
        public static T[] FindAssetsByType <T>(string nameFilter) where T : UnityEngine.Object
        {
            string typeName =  typeof(T).Name;
            string filter = "t:" + typeName + " " + nameFilter;

            string[] guids = AssetDatabase.FindAssets(filter);
            T[] objects = new T[guids.Length];

            int i = 0;
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                objects[i] = AssetDatabase.LoadAssetAtPath<T>(path);
                i++;
            }

            Debug.Log("Searched Asset Database using filter '" + filter + "'. Found " + objects.Length +" objects of type '" + typeName +" '.");
            return objects;
        }

        /// <summary>
        /// Draws a debug arrow graphic
        /// </summary>
        /// <param name="pos">The origin of the arrow</param>
        /// <param name="direction">The direction the arrow is pointing</param>
        /// <param name="arrowHeadLength">The length of the arrow</param>
        /// <param name="arrowHeadAngle">The angle between the initial line and the head</param>
        public static void ArrowGizmo (Vector3 pos, Vector3 direction, float arrowHeadLength = 1.5f, float arrowHeadAngle = 20)
        {
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle,0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle,0) * new Vector3(0 , 0, 1);

            Gizmos.DrawRay(pos, direction);
            Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
            Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
        }
    }
}
#endif