using grimhawk.ui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitialScreen : UIScreen
{
    [SerializeField] private AnimatableUI<TextMeshProUGUI> _levelCounter;
    [SerializeField] private AnimatableUI<Button> _playButton;

    private void Start()
    {
        UpdateLevelCounter();
    }
    private void UpdateLevelCounter()
    {
        _levelCounter.UIComponent.text = "Level " + _gameManager._levelManager.GetLevel();// + (SaveLoadManager.Getlevel() + 1);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        _playButton.UIComponent.onClick.AddListener(OnPlayButtonPressed);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _playButton.UIComponent.onClick.RemoveListener(OnPlayButtonPressed);
    }
    public override IEnumerator PlayInAnimation()
    {
        gameObject.SetActive(true);
        StartCoroutine(_levelCounter.PlayInAnimation(1f));
        yield return StartCoroutine(_playButton.PlayInAnimation(1f));
        
    }

    public override IEnumerator PlayOutAnimation()
    {    
        StartCoroutine(_levelCounter.PlayOutAnimation(1f));
        yield return StartCoroutine(_playButton.PlayOutAnimation(1f));
        
        gameObject.SetActive(false);
    }
    private void OnPlayButtonPressed()
    {
        StartCoroutine(this.PlayOutAnimation());
    }
}
