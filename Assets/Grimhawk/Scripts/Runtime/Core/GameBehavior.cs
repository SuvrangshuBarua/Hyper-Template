using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using grimhawk.core;
using grimhawk.managers;
using grimhawk.tools.gameevent;

public abstract class GameBehavior : GameEventListener
{
    #region Public Variables
    public bool IsRegistered { get; private set; }

    #endregion
    #region Protected Variables
    protected SysemManager _gameManager { get; private set; }

    #endregion

    #region Monobehavior

    protected virtual void OnEnable()
    {
        if(!IsRegistered)
        {
            IsRegistered = true;
            _gameManager = SysemManager.Instance;
            _gameManager.OnSceneLoadBeginEvent.RegisterEvent(this, OnSceneLoadBegin);
            _gameManager.OnSceneLoadedEvent.RegisterEvent(this, OnSceneLoaded);
            _gameManager.OnDataLoadedEvent.RegisterEvent(this, OnDataLoaded);
            _gameManager.OnLevelStartedEvent.RegisterEvent(this, OnLevelStarted);
            _gameManager.OnStepOneCompleteEvent.RegisterEvent(this, OnStepOneComplete);
            _gameManager.OnStepTwoCompleteEvent.RegisterEvent(this, OnStepTwoComplete);
            _gameManager.OnLevelCompleteEvent.RegisterEvent(this, OnLevelComplete);
            _gameManager.OnLevelFailedEvent.RegisterEvent(this, OnLevelFailed);
        }
    }
    protected virtual void OnDisable()
    {
        if(IsRegistered)
        {
            _gameManager.OnSceneLoadBeginEvent.UnregisterEvent(this);
            _gameManager.OnSceneLoadedEvent.UnregisterEvent(this);
            _gameManager.OnDataLoadedEvent.UnregisterEvent(this);
            _gameManager.OnLevelStartedEvent.UnregisterEvent(this);
            _gameManager.OnStepOneCompleteEvent.UnregisterEvent(this);
            _gameManager.OnStepTwoCompleteEvent.UnregisterEvent(this);
            _gameManager.OnLevelCompleteEvent.UnregisterEvent(this);
            _gameManager.OnLevelFailedEvent.UnregisterEvent(this);
        }
    }
    #endregion

    #region Virtual Method

    protected virtual void OnSceneLoadBegin() { }
    protected virtual void OnSceneLoaded() { }
    protected virtual void OnDataLoaded() { }
    protected virtual void OnLevelStarted() { }
    protected virtual void OnStepOneComplete() { }
    protected virtual void OnStepTwoComplete() { }
    protected virtual void OnLevelComplete() { }
    protected virtual void OnLevelFailed() { }
    #endregion
}
