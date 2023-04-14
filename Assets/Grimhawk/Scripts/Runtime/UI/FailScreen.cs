using grimhawk.ui;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FailScreen : UIScreen
{
    [SerializeField] private AnimatableUI<Image> _background;
    [SerializeField] private AnimatableUI<Image> _sadEmoji;
    [SerializeField] private AnimatableUI<Button> _retryButton;
    [SerializeField] private AnimatableUI<TextMeshProUGUI> _reactionText;

    [SerializeField] private Sprite[] _sadEmojis;

    protected override void OnEnable()
    {
        base.OnEnable();
        _retryButton.UIComponent.onClick.AddListener(OnRetry);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _retryButton.UIComponent.onClick.RemoveListener(OnRetry);
    }
    public override IEnumerator PlayInAnimation()
    {
        _sadEmoji.Image.sprite = _sadEmojis.GetRandom();
        gameObject.SetActive(true);
        yield return StartCoroutine(_background.PlayInAnimation(1f));
        StartCoroutine(_reactionText.PlayInAnimation(0.2f));
        yield return StartCoroutine(_sadEmoji.PlayInAnimation(.2f));
        yield return StartCoroutine(_retryButton.PlayInAnimation(.1f));
    }

    public override IEnumerator PlayOutAnimation()
    {
        gameObject.SetActive(false);
        yield break;
    }
    public override void Reset()
    {
        base.Reset();
        _background.Reset();
        _sadEmoji.Reset();
        _retryButton.Reset();
        _reactionText.Reset();
    }
    public void OnRetry()
    {
        //Handle LevelManager Code to Reload Same Level
        _gameManager.levelManager.LoadLevel();
    }
    protected override void OnSceneLoaded()
    {
        base.OnSceneLoaded();
        Reset();
    }
}
