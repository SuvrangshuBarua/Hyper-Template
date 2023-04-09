using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Circle Transition", menuName = "Grim/SceneTransitionMode/Circle")]
public class CircleTransitionSO : BaseSceneTransitionSO
{
    public Sprite circleSprite;
    public Color color;
    public override IEnumerator SceneEntryAnimation(Canvas parent)
    {
        float time = 0;
        float size = Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2));
        Vector2 initialSize = new Vector2(size, size);
        while (time < 1) 
        {
            animatedObject.rectTransform.sizeDelta = Vector2.Lerp(initialSize,
                                                                  Vector2.zero,
                                                                  lerpCurve.Evaluate(time));
            yield return null;
            time += Time.deltaTime / animationTime;
        }

        Destroy(animatedObject.gameObject);
    }

    public override IEnumerator SceneExitAnimation(Canvas parent)
    {
        animatedObject = CreateImage(parent);
        animatedObject.color = color;
        animatedObject.rectTransform.sizeDelta = Vector3.zero;
        animatedObject.sprite = circleSprite;

        float time = 0;
        float size = Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2));  
        Vector2 targetSize = new Vector2(size, size);
        while(time < 1)
        {
            animatedObject.rectTransform.sizeDelta = Vector2.Lerp(Vector2.zero,
                                                                  targetSize, 
                                                                  lerpCurve.Evaluate(time));
            yield return null;
            time += Time.deltaTime / animationTime;
        }
    }

    
}
