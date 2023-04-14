using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomEventInvoker : GameBehavior
{
    [SerializeField] private Button _failureButton;
    [SerializeField] private Button _successButton;
    [SerializeField] private Button _add20KCoin;
    [SerializeField] private Button _deduct10KCoin;

    protected override void OnEnable()
    {
        base.OnEnable();
        _failureButton?.onClick.AddListener(delegate { _gameManager.OnLevelFailedEvent.Raise(); });
        _successButton?.onClick.AddListener(delegate { _gameManager.OnLevelCompleteEvent.Raise(); });
        _add20KCoin?.onClick.AddListener(delegate { _gameManager.dataManager.AddCurrency(20000); });
        _deduct10KCoin?.onClick.AddListener(delegate { _gameManager.dataManager.DeductCurrency(10000); });
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        _failureButton?.onClick.RemoveListener(delegate { _gameManager.OnLevelFailedEvent.Raise(); });
        _successButton?.onClick.RemoveListener(delegate { _gameManager.OnLevelCompleteEvent.Raise(); });
        _add20KCoin?.onClick.RemoveListener(delegate { _gameManager.dataManager.AddCurrency(20000); });
        _deduct10KCoin?.onClick.RemoveListener(delegate { _gameManager.dataManager.DeductCurrency(10000); });
    }
}
