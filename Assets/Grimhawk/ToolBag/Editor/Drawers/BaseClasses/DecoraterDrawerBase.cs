using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class DecoraterDrawerBase<T> : DecoratorDrawer where T: PropertyAttribute
{
    public sealed override float GetHeight()
    {
        return IsAttributeValid(attribute) ? GetHeightSafe(attribute as T) : base.GetHeight();
    }
    protected virtual float GetHeightSafe(T localAttribute)
    {
        return base.GetHeight();
    }

    public sealed override void OnGUI(Rect position)
    {
        if(IsAttributeValid(attribute))
        {
            OnGUISafe(position, attribute as T);
            return;
        }
        
    }

    protected virtual void OnGUISafe(Rect position, T localAttribute)
    {
        
    }

    public virtual bool IsAttributeValid(PropertyAttribute attribute)
    {
        return true;
    }
}
