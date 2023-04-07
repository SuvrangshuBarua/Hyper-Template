using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fade Transition", menuName = "Grim/SceneTransitionMode/Fade")]
public class FadeTransitionSO : BaseSceneTransitionSO
{
    public override IEnumerator SceneEntryAnimation(Canvas parent)
    {
        float time = 0;
        Color startColor = Color.black;
        Color endColor = new Color(0, 0, 0, 0);
        while (time < 1)
        {
            animatedObject.color = Color.Lerp(
                startColor,
                endColor,
                lerpCurve.Evaluate(time)
            );
            yield return null;
            time += Time.deltaTime / animationTime;
        }

        Destroy(animatedObject.gameObject);
    }

    public override IEnumerator SceneExitAnimation(Canvas parent)
    {
        animatedObject = CreateImage(parent);
        animatedObject.rectTransform.anchorMin = Vector2.zero;
        animatedObject.rectTransform.anchorMax = Vector2.one;
        animatedObject.rectTransform.sizeDelta = Vector2.zero;

        float time = 0;
        Color startColor =  new Color(0, 0, 0, 0);
        Color endColor = Color.black;
        while( time < 1 )
        {
            animatedObject.color = Color.Lerp(startColor, endColor,lerpCurve.Evaluate(time));
            yield return null;
            time += Time.deltaTime / animationTime;
        }
    }

    
}
