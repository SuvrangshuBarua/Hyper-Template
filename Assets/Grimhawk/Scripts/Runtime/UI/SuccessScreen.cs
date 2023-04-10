using grimhawk.ui;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuccessScreen : UIScreen
{
    [SerializeField] private AnimatableUI<Image> _background;
    [SerializeField] private AnimatableUI<Image> _happyEmoji;
    [SerializeField] private AnimatableUI<Button> _nextLevelButton;
    [SerializeField] private AnimatableUI<TextMeshProUGUI> _reactionText;

    [SerializeField] private Sprite[] _happyEmojis;

    protected override void OnEnable()
    {
        base.OnEnable();
        _nextLevelButton.UIComponent.onClick.AddListener(OnNextLevel);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _nextLevelButton.UIComponent.onClick.RemoveListener(OnNextLevel);
    }
    public override IEnumerator PlayInAnimation()
    {
        _happyEmoji.Image.sprite = _happyEmojis.GetRandom();
        gameObject.SetActive(true);
        yield return StartCoroutine(_background.PlayInAnimation(1f));
        StartCoroutine(_reactionText.PlayInAnimation(0.2f));
        yield return StartCoroutine(_happyEmoji.PlayInAnimation(0.2f));
        yield return StartCoroutine(_nextLevelButton.PlayInAnimation(0.1f));
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
        _happyEmoji.Reset();
        _nextLevelButton.Reset();
        _reactionText.Reset();
    }
    public void OnNextLevel()
    {
        //Report Level should be iterated to next
        //Handle LevelManager Load Next Level Logic
        _gameManager._levelManager.LoadLevel();
    }
    protected override void OnSceneLoaded()
    {
        base.OnSceneLoaded();
        Reset();
    }
}
