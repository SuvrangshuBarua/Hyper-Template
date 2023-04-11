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
        UpdateLevelCounter();
        StartCoroutine(_levelCounter.PlayInAnimation(0.5f));
        yield return StartCoroutine(_playButton.PlayInAnimation(0.5f));
        
    }

    public override IEnumerator PlayOutAnimation()
    {    
        StartCoroutine(_levelCounter.PlayOutAnimation(0.5f));
        yield return StartCoroutine(_playButton.PlayOutAnimation(0.5f));
        
        gameObject.SetActive(false);
    }
    private void OnPlayButtonPressed()
    {
        StartCoroutine(_gameManager._uiManager.ChangeUI(this));
    }
}
