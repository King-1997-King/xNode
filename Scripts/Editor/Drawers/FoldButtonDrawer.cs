using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using XNode;
namespace XNodeEditor
{
    [CustomPropertyDrawer(typeof(FoldButtonAttribute))]
    public class FoldButtonDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // Throw error on wrong type
            if (property.propertyType != SerializedPropertyType.Boolean) {
                throw new ArgumentException("Parameter selected must be of type bool");
            }
            
            EditorGUI.BeginProperty(position, label, property);

            if ((bool)fieldInfo.GetValue(property.serializedObject.targetObject)) {
                if (GUI.Button(position, "▼")) {
                    fieldInfo.SetValue(property.serializedObject.targetObject, false);
                }
            } else {
                if (GUI.Button(position, "▲")) {
                    fieldInfo.SetValue(property.serializedObject.targetObject, true);
                }
            }
            EditorGUI.EndProperty();
        }
    }
}