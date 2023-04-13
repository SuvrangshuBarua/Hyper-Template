
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
        internal void IncrementLevel() => _gameManager._dataManager.IncrementLevel();
        private int GetSceneIndex()
        {
            return (_gameManager._dataManager.Level % totalLevelNumber) + 1;
        }
        public int GetLevel()
        {
            return _gameManager._dataManager.Level + 1;
        }
        private void OnWishdomReady()
        {
            LoadLevel();
        }
        
        
        public void LoadLevel()
        {
            _gameManager._sceneManager.LoadScene(GetSceneIndex(), _levelLoadStyle);
        }
        protected override void OnDataLoaded()
        {
            base.OnDataLoaded();
            
            OnWishdomReady();
            _gameManager.OnSceneLoadBeginEvent.Raise();
        }
        protected override void OnSceneLoadBegin()
        {
            base.OnSceneLoadBegin();
            Debug.Log($"<color=white>--- You Are Playing Level : {GetLevel()} ---</color>");
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


