using System;
using UnityEngine;

internal class GuiBackground : IDisposable
{
    internal Color oldBackgroundColor;
    internal Color newBackgroundColor;

    public GuiBackground (Color newBackgroundColor)
    {
        this.newBackgroundColor = newBackgroundColor;
        PaintBackground();
    }
    public void PaintBackground()
    {
        oldBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = newBackgroundColor;
    }
    public void Dispose()
    {
        GUI.backgroundColor = oldBackgroundColor;
    }
}
