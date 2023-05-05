using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class HeaderAttribute : PropertyAttribute
{
    public readonly string header;
    public readonly string colorString;
    public readonly Color color;
    public readonly float textHeightIncrease;
    public HeaderAttribute(string header, string colorString) : this(header, colorString, 1) { }
    public HeaderAttribute(string header, string colorString = "lightblue", float textHeightIncrease = 1)
    {
        this.header = header;
        this.colorString = colorString;

        this.textHeightIncrease = Mathf.Max(1f, textHeightIncrease);
        if (ColorUtility.TryParseHtmlString(colorString, out this.color)) return;

        this.color = new Color(173, 216, 230);
        this.colorString = "lightblue";
    }
}
