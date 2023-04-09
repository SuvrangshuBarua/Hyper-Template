using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using grimhawk.core;
using NaughtyAttributes;
using System;
#if SUPERSONIC_WISDOM_SDK_INSTALLED
//using SupersonicWisdomSDK;
#endif
namespace grimhawk.managers
{
    public class LevelManager : GameBehavior
    {
        [SerializeField]
        private PersistantData<int> _level;
        private const int defaultValue = 0;
        private const string saveFileName = "_LevelNumber";
        [SerializeField]
        private int totalLevelNumber = 5;
        [SerializeField]
        private SceneTransitionMode _levelLoadStyle = SceneTransitionMode.None;
        public int Level
        {
            private set
            {
                if (_level == null)
                {
                    _level = new PersistantData<int>(saveFileName, defaultValue);
                }
                _level.value = value;

            }

            get
            {
                if (_level == null)
                {
                    _level = new PersistantData<int>(saveFileName, defaultValue);
                }
                return _level;
            }
        }

        public void IncrementLevel() => Level++;

        private void Awake()
        {
            Input.backButtonLeavesApp = true;
        }
        private int GetSceneIndex()
        {
            return (Level % totalLevelNumber) + 1;
        }
        public int GetLevel()
        {
            return Level + 1;
        }
        private void OnWishdomReady()
        {
            _gameManager._sceneManager.LoadScene(GetSceneIndex(), _levelLoadStyle);
        }
        public void Initialize(Action OnLevelDataLoaded)
        {
            //TODO: DebugLog Colorizer class 
#if UNITY_EDITOR
            Debug.Log($"<color=white>--- You Are Playing Level : {GetSceneIndex()} ---</color>");
#endif
            OnLevelDataLoaded?.Invoke();
        }
        protected override void OnDataLoaded()
        {
            base.OnDataLoaded();
            OnWishdomReady();
        }
        protected override void OnSceneLoadBegin()
        {
            base.OnSceneLoadBegin();
        }
        
    }
}


