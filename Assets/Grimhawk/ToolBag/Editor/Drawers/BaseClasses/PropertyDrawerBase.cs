using System;
using UnityEditor;
using UnityEngine;

public abstract class PropertyDrawerBase : PropertyDrawer
{
    public sealed override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return IsPropertyValid(property) ? GetPropertyHeightSafe(property, label) : base.GetPropertyHeight(property, label);
    }

    protected virtual float GetPropertyHeightSafe(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label);
    }

    public sealed override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if(IsPropertyValid(property))
        {
            OnGUISafe(position, property, label);
            return;
        }
    }
    protected virtual void OnGUISafe(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label);
    }

    public virtual bool IsPropertyValid(SerializedProperty property)
    {
        return true;
    }
}
