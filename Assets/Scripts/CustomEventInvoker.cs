using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomEventInvoker : GameBehavior
{
    [SerializeField] private Button _failureButton;
    [SerializeField] private Button _successButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        _failureButton?.onClick.AddListener(delegate { _gameManager.OnLevelFailedEvent.Raise(); });
        _successButton?.onClick.AddListener(delegate { _gameManager.OnLevelCompleteEvent.Raise(); });
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _failureButton?.onClick.RemoveListener(delegate { _gameManager.OnLevelFailedEvent.Raise(); });
        _successButton?.onClick.RemoveListener(delegate { _gameManager.OnLevelCompleteEvent.Raise(); });
    }
}
