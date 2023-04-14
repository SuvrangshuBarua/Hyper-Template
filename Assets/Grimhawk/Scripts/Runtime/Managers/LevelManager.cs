
using UnityEngine;

#if SUPERSONIC_WISDOM_SDK_INSTALLED
using SupersonicWisdomSDK;
#endif

namespace grimhawk.managers
{
    public class LevelManager : GameBehavior
    {
        [SerializeField]
        private int totalLevelNumber = 5;
        [SerializeField]
        private SceneTransitionMode _levelLoadStyle = SceneTransitionMode.None;
        

        

        private void Awake()
        {
            Input.backButtonLeavesApp = true;
        }
        internal void IncrementLevel() => _gameManager.dataManager.IncrementLevel();
        private int GetSceneIndex()
        {
            return (_gameManager.dataManager.Level % totalLevelNumber) + 1;
        }
        public int GetLevel()
        {
            return _gameManager.dataManager.Level + 1;
        }
        private void OnSupersonicWisdomReady()
        {
            LoadLevel();
        }
        
        
        public void LoadLevel()
        {
            _gameManager.sceneManager.LoadScene(GetSceneIndex(), _levelLoadStyle);
        }
        protected override void OnDataLoaded()
        {
            base.OnDataLoaded();
#if SUPERSONIC_WISDOM_SDK_INSTALLED
             SupersonicWisdom.Api.AddOnReadyListener(OnSupersonicWisdomReady);
             SupersonicWisdom.Api.Initialize();
#else
            OnSupersonicWisdomReady();
#endif

            _gameManager.OnSceneLoadBeginEvent.Raise();
        }
        protected override void OnSceneLoadBegin()
        {
            base.OnSceneLoadBegin();
#if UNITY_EDITOR
            Debug.Log($"<color=white>--- You Are Playing Level : {GetLevel()} ---</color>");
#endif
        }

        protected override void OnLevelComplete()
        {
            base.OnLevelComplete();
            IncrementLevel();
            StartCoroutine(_gameManager._uiManager.ChangeUI(UIManager.MainScreens.Success));
        }

        protected override void OnLevelFailed()
        {
            base.OnLevelFailed();
            StartCoroutine(_gameManager._uiManager.ChangeUI(UIManager.MainScreens.Fail));
        }
    }
}


