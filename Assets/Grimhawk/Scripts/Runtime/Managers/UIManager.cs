using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using grimhawk.core;
using grimhawk.ui;
using System;

namespace grimhawk.managers
{
    public class UIManager : GameBehavior
    {
        public UIScreen ActiveScreen { get; private set; }

        #region Private Variables
        [SerializeField] private InitialScreen initial;
        [SerializeField] private HUDScreen HUD;
        [SerializeField] private SuccessScreen success;
        [SerializeField] private FailScreen failure;
        #endregion

        private IEnumerator Start()
        {
            yield return null;
            
        }
        public IEnumerator ChangeUI(UIScreen uiScreen)
        {
            
            if (ActiveScreen)
                if(ActiveScreen.isActiveAndEnabled)
                {
                    ActiveScreen.Reset();
                    yield return StartCoroutine(ActiveScreen.PlayOutAnimation());
                }                 

            ActiveScreen = uiScreen;
            if(ActiveScreen)
                yield return StartCoroutine(ActiveScreen.PlayInAnimation());
        }
        public IEnumerator ChangeUI(MainScreens screen)
        {
            UIScreen uiScreen = screen switch
            {
                MainScreens.Initial => initial,
                MainScreens.HUD => HUD,
                MainScreens.Success => success,
                MainScreens.Fail => failure,
                _ => throw new ArgumentOutOfRangeException(nameof(screen), screen, null)
            };

            return ChangeUI(uiScreen);
        }
        protected override void OnSceneLoaded()
        {
            base.OnSceneLoaded();
            StartCoroutine(ChangeUI(MainScreens.Initial));
        }
        protected override void OnLevelStarted()
        {
            base.OnLevelStarted();
            StartCoroutine(_gameManager._uiManager.ChangeUI(UIManager.MainScreens.HUD));
        }
        public enum MainScreens
        {
            Initial,
            HUD,
            Success,
            Fail
        }
    }
}

