using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HeaderAttribute))]
public class HeaderDrawer : DecoratorDrawer
{
    #region Overrides of DecoratorDrawer
    public override void OnGUI(Rect position)
    {
        if(!(attribute is HeaderAttribute headerAttribute)) return;

        
        position = EditorGUI.IndentedRect(position);
        position.yMin += EditorGUIUtility.singleLineHeight * (headerAttribute.textHeightIncrease - 0.5f);

        if(string.IsNullOrEmpty(headerAttribute.header))
        {
            position.height = headerAttribute.textHeightIncrease;
            EditorGUI.DrawRect(position, headerAttribute.color);
        }

        GUIStyle style = new GUIStyle(EditorStyles.label) {richText = true,
            font = Resources.Load<Font>("Fonts/Roboto-Thin"),
            fontSize = 10
        };
        GUIContent label = new GUIContent($"<color={headerAttribute.colorString}><size={style.fontSize + headerAttribute.textHeightIncrease}><b>{headerAttribute.header}</b></size></color>");

        Vector2 textSize = style.CalcSize(label);
        float separatorWidth = (position.width - (textSize.x + 20f)) / 2f;
        Rect prefixRect = new Rect(position.xMin - 5f, position.yMin + 3f, separatorWidth, headerAttribute.textHeightIncrease);
        Rect labelRect =  new Rect(position.xMin + separatorWidth, position.yMin - 3f, textSize.x, position.height);
        Rect postfixRect = new Rect(position.xMin + separatorWidth + 5f + textSize.x, position.yMin + 3f, separatorWidth, headerAttribute.textHeightIncrease);

        EditorGUI.DrawRect(prefixRect, headerAttribute.color);
        EditorGUI.LabelField(labelRect, label, style);
        EditorGUI.DrawRect(postfixRect, headerAttribute.color); 
    }

    public override float GetHeight()
    {
        HeaderAttribute headerAttribute = attribute as HeaderAttribute;
        return EditorGUIUtility.singleLineHeight + (headerAttribute?.textHeightIncrease + 2.5f ?? 0);
    }
    #endregion
}
