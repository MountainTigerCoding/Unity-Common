using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Editors.Shared
{
    public static class EditorGUIUtils
    {
    #region Fields
        public static void PaintField (SerializedObject serializedObject, string propertyPath, string inspectorName, string tooltip = "")
        {
            if (serializedObject == null) return;
            if (serializedObject.targetObject == null) return;

            SerializedProperty property = serializedObject.FindProperty(propertyPath);
            if (property == null) {
                Debug.LogError("Could not find property '" + propertyPath + "'.");
                return;
            }

            // Extract tooltip, if present
            if (tooltip == "") ExtractTooltip();
            PaintField(property, inspectorName, tooltip);

            string ExtractTooltip ()
            {
                FieldInfo fieldInfo = serializedObject.targetObject.GetType().GetField(propertyPath);
                if (fieldInfo == null) return "";
                return GetTooltip(fieldInfo).tooltip;
            }
        }

        public static void PaintField (SerializedProperty property, string inspectorName, string tooltip = "")
        {
            EditorGUILayout.PropertyField(property, new GUIContent(inspectorName, tooltip), true);
        }

        /// <summary>
        /// Paints 2 float fields to control a range
        /// </summary>
        /// <param name="name">The inspector name of the range</param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="minLimit">The minimium allowed value</param>
        /// <param name="maxLimit">The maximium value allowed</param>
        /// <returns>true if a value was modified</returns>
        [Obsolete("Should use MinMax attaribute instead")]
        public static void PaintMinMaxFloatField (string name, ref float minValue, ref float maxValue, float minLimit, float maxLimit)
        {
            EditorGUILayout.MinMaxSlider(name, ref minValue, ref maxValue, minLimit, maxLimit);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.FloatField("Min", minValue);
            EditorGUILayout.FloatField("Max", maxValue);
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(10f);
        }

        public static void DrawDisabledObjectArray <T>(T[] collection, ref Vector2 scrollPosition, string header = "Count") where T : UnityEngine.Object
        {
            if (collection == null) return;

            int count = collection.Length;
            GUILayout.Label(header + ": " + count);
            if (count == 0) return;

            GUILayout.ExpandHeight(false);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);
            EditorGUI.BeginDisabledGroup(true);

            foreach (T element in collection)
            {
                EditorGUILayout.ObjectField(element, typeof(T), false);
            }

            EditorGUI.EndDisabledGroup();
            GUILayout.EndScrollView();
        }

        public static void DrawDisabledObjectQueue <T>(Queue<T> queue, ref Vector2 scrollPosition, string header = "Count") where T : UnityEngine.Object
        {
            if (queue == null) return;

            int count = queue.Count;
            GUILayout.Label(header + ": " + count);
            if (count == 0) return;

            GUILayout.ExpandHeight(false);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false);
            EditorGUI.BeginDisabledGroup(true);

            foreach (T element in queue.ToArray())
            {
                EditorGUILayout.ObjectField(element, typeof(T), false);
            }

            EditorGUI.EndDisabledGroup();
            GUILayout.EndScrollView();
        }

        private static TooltipAttribute GetTooltip (FieldInfo field, bool inherit = true)
        {
            TooltipAttribute[] attributes = field.GetCustomAttributes(typeof(TooltipAttribute), inherit) as TooltipAttribute[];
            return attributes.Length > 0 ? attributes[0] : null;
        }
    #endregion Fields

    #region Foldouts
        // If nesting is not required, then use FoldoutHeaderGroup() instead 
        [Obsolete("Use the FoldOut() overload with 'paintFunc' argument for dependancy injection")]
        public static bool FoldOut (ref bool open, string name)
        {
            open = EditorGUILayout.Foldout(open, name);
            return open;
        }

        public static void FoldOut (string name, ref bool open, Action paintFunc)
        {
            open = EditorGUILayout.Foldout(open, name);
            if (open) {
                EditorGUI.indentLevel++;
                paintFunc.Invoke();
                EditorGUI.indentLevel--;
            }
        }

        public static void FoldoutHeaderGroup (string header, ref bool isOpen, Action paintFunc)
        {
            isOpen = EditorGUILayout.BeginFoldoutHeaderGroup(isOpen, header);
            if (isOpen) paintFunc.Invoke();
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    #endregion Foldouts

    #region Decorations
        public static void Header (string value, float height = 10)
        {
            GUIStyle style = new()
            {
                fontStyle = FontStyle.Bold,
                fontSize = 15
            };
            style.normal.textColor = Color.white;

            GUILayout.Space(height);
            GUILayout.Label(value,style);
        }
    #endregion Decorations
    }
}