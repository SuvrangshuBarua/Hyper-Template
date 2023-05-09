using UnityEditor;
using UnityEngine;

public abstract class HierarchyDrawer
{
    public void Draw(Rect rect, GameObject instance)
    {
        if(DrawerIsEnabled(instance))
        {
            DrawInternal(rect, instance);   
        }
    }
    protected abstract void DrawInternal(Rect rect, GameObject instance);
    protected abstract bool DrawerIsEnabled(GameObject instance);
}

public class HierarchyStyle
{
    public string prefix = "<PREFIX>";
    public string name = "New Style";

    public Font font = null;

    public int fontSize = 11;
}