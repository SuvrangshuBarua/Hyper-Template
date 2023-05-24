using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HeaderAttribute))]
public class HeaderDrawer : DecoraterDrawerBase<HeaderAttribute>
{
    #region Overrides of DecoratorDrawer
    protected override void OnGUISafe(Rect position, HeaderAttribute headerAttribute)
    {       
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
        Rect labelRect =  new Rect(position.xMin + separatorWidth, position.yMin - 5f, textSize.x, position.height);
        Rect postfixRect = new Rect(position.xMin + separatorWidth + 5f + textSize.x, position.yMin + 3f, separatorWidth, headerAttribute.textHeightIncrease);

        EditorGUI.DrawRect(prefixRect, headerAttribute.color);
        EditorGUI.LabelField(labelRect, label, style);
        EditorGUI.DrawRect(postfixRect, headerAttribute.color); 
    }

    protected override float GetHeightSafe(HeaderAttribute headerAttribute)
    {
        return EditorGUIUtility.singleLineHeight + (headerAttribute?.textHeightIncrease + 5f ?? 0);
    }

    public override bool IsAttributeValid(PropertyAttribute attribute)
    {
        return attribute is HeaderAttribute;
    }
    #endregion
}
