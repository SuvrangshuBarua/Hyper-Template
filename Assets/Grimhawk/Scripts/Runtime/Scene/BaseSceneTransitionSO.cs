using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseSceneTransitionSO : ScriptableObject
{
    [CurveRange(EColor.Violet)]
    public AnimationCurve lerpCurve;
    public float animationTime = 0.25f;
    protected Image animatedObject;

    public abstract IEnumerator SceneEntryAnimation(Canvas parent);
    public abstract IEnumerator SceneExitAnimation(Canvas parent);

    protected virtual Image CreateImage(Canvas parent)
    {
        GameObject child = new GameObject("Transition Image");
        child.transform.SetParent(parent.transform, false);

        return child.GetComponent<Image>();
    }
}
