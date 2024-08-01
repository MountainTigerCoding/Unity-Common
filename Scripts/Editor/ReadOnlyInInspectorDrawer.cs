using UnityEngine;
using Runtime.Shared;
using UnityEditor;

namespace Editors.Shared
{
    [CustomPropertyDrawer(typeof(ReadOnlyInInspectorAttribute))]
    internal sealed class ReadOnlyInInspectorDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}