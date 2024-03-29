using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using MyBox;

[Serializable]
public class AnimatableUI<T> : ISerializationCallbackReceiver where T : UIBehaviour
{
    [field :SerializeField] public T UIComponent { get; private set; }

    [InfoBox("If true, GameObject would set false at the end of the out animation or vice verse")]
    [SerializeField] private bool changeActiveStatusWhileAnimating;
    [OnValueChanged("DebugAnimationType"), SerializeField] private AnimationType animationType;

    #region Properties
    public Image Image => image;
    public  RectTransform RectTransform => rectTransform;
    public GameObject GameObject => UIComponent.gameObject;
    #endregion

    #region Backing Field
    [SerializeField, HideInInspector] private RectTransform rectTransform;
    [SerializeField, HideInInspector] private Image image;
    
    #endregion

    #region Animation Params
    [SerializeField, ShowIf("AnimationTypeIsMove"), AllowNesting] private Vector2 inPosition;
    [SerializeField, ShowIf("AnimationTypeIsMove"),AllowNesting] private Vector2 outPosition;

    [SerializeField, ShowIf("AnimationTypeIsScale"), AllowNesting] private Vector3 inScale;
    [SerializeField, ShowIf("AnimationTypeIsScale"), AllowNesting] private Vector3 outScale;

    [SerializeField, ShowIf("AnimationTypeIsFade"), AllowNesting] private float inAlpha;
    [SerializeField, ShowIf("AnimationTypeIsFade"), AllowNesting] private float outAlpha;

    [SerializeField, ShowIf("AnimationTypeIsColor"), AllowNesting] private Color inColor;
    [SerializeField, ShowIf("AnimationTypeIsColor"), AllowNesting] private Color outColor;

    [SerializeField] private Ease inEase = Ease.Linear;
    [SerializeField] private Ease outEase = Ease.Linear;
    #endregion

    #region Type Checks
    private bool AnimationTypeIsMove => animationType.HasFlag(AnimationType.Move);
    private bool AnimationTypeIsScale => animationType.HasFlag(AnimationType.Scale);
    private bool AnimationTypeIsFade => animationType.HasFlag(AnimationType.Fade);
    private bool AnimationTypeIsColor => animationType.HasFlag(AnimationType.Color);
    #endregion
    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {
        if (UIComponent != null)
        {
            rectTransform = UIComponent.GetComponent<RectTransform>();
            Image _image = UIComponent.GetComponent<Image>();

            if (_image != null) image = _image;
            else image = null;
        }
        else
        {
            rectTransform = null;
            image = null;
        }
    }
    #region Animation
    public IEnumerator PlayInAnimation(float duration)
    {
        if (changeActiveStatusWhileAnimating) GameObject.SetActive(true);
        if (AnimationTypeIsMove) rectTransform.DOAnchorPos(inPosition, duration).SetEase(inEase);
        if (AnimationTypeIsScale) rectTransform.DOScale(inScale, duration).SetEase(inEase);
        if (AnimationTypeIsColor) Image.DOColor(inColor, duration).SetEase(inEase);  
        if (AnimationTypeIsFade) Image.DOFade(inAlpha, duration).SetEase(inEase);
        yield return new WaitForSeconds(duration);
    }
    public IEnumerator PlayOutAnimation(float duration)
    {
        
        if (AnimationTypeIsMove) rectTransform.DOAnchorPos(outPosition, duration).SetEase(outEase);
        if (AnimationTypeIsScale) rectTransform.DOScale(outScale, duration).SetEase(outEase);
        if (AnimationTypeIsColor) Image.DOColor(outColor, duration).SetEase(outEase);
        if (AnimationTypeIsFade) Image.DOFade(outAlpha, duration).SetEase(outEase);     
        yield return new WaitForSeconds(duration);
        if (changeActiveStatusWhileAnimating) GameObject.SetActive(false);
    } 
    public void Reset()
    {
        if (changeActiveStatusWhileAnimating) GameObject.SetActive(false);
        if (AnimationTypeIsMove) rectTransform.anchoredPosition = outPosition;
        if (AnimationTypeIsScale) rectTransform.localScale = outScale;
        if (AnimationTypeIsColor) Image.color = outColor;
        if (AnimationTypeIsFade) Image.SetAlpha(outAlpha);
    }
    #endregion
    [Flags]
    enum AnimationType
    {
        Move  = 1 << 0,
        Scale = 1 << 1,
        Fade  = 1 << 2,
        Color = 1 << 3,
    }
    private void DebugAnimationType()
    {
        if(animationType.HasFlag(AnimationType.Fade) && image == null)
        {
            Debug.LogError("Impossible to set animation type to Fade if object does not have Image component", UIComponent);
            animationType &= ~AnimationType.Fade;
        }
        if(animationType.HasFlag(AnimationType.Color) && image == null)
        {
            Debug.LogError("Impossible to set animation type to Color if object does not have Image component", UIComponent);
            animationType &= ~AnimationType.Color;
        }
    }
    
}
