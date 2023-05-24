
using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(NotNullAttribute))]
public class NotNullAttributeDrawer : PropertyDrawerBase
{
    protected override float GetPropertyHeightSafe(SerializedProperty property, GUIContent label)
    {
        //return default height is there is object reference or return custom height 
        return property.objectReferenceValue ? base.GetPropertyHeightSafe(property, label)  
                                             : base.GetPropertyHeightSafe(property, label) + Style.boxHeight + Style.spacing * 2;
    }
    protected override void OnGUISafe(Rect position, SerializedProperty property, GUIContent label)
    {
        // Draw the default value if object reference is given or show a helpbox with errormessage
        position.height = EditorGUI.GetPropertyHeight(property);
        if(property.objectReferenceValue)
        {
            // Draw defult property
            EditorGUI.PropertyField(position, property, label, property.isExpanded);
        }
        else
        {
            //  Draw the helpbox with warning message
            var helpBox =  new Rect ( position.x, position.y, position.width, Style.boxHeight );
            EditorGUI.HelpBox(helpBox, Attribute.Label, MessageType.Error);
            // Change background color in Disposable scope
            using ( new GuiBackground(Style.errorBackgroundColor) )
            {
                EditorGUI.PropertyField(position, property, label, property.isExpanded);
            }
        }
        
    }
    public override bool IsPropertyValid(SerializedProperty property)
    {
        return property.propertyType == SerializedPropertyType.ObjectReference;
    }

    public NotNullAttribute Attribute => attribute as NotNullAttribute;
    private static class Style
    {
        internal static readonly float boxHeight = EditorGUIUtility.singleLineHeight * 2.1f;
        internal static readonly float spacing = EditorGUIUtility.standardVerticalSpacing;
        internal static readonly Color errorBackgroundColor = Color.red;
    }
}
