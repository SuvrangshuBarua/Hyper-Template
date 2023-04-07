using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using grimhawk.core;
using grimhawk.tools.gameevent;
using NaughtyAttributes;
using MyBox;

namespace grimhawk.managers
{
    public class GameManager : GameEventListener
    {
        #region Public Variables
        public static GameManager Instance { get; private set; }
        public bool IsThisLevelActive { get; private set; } = false;

        public GameEvent OnDataLoadedEvent { get => _OnDataLoadedEvent; }
        public GameEvent OnSceneLoadBeginEvent { get => _OnSceneLoadBeginEvent; }
        public GameEvent OnSceneLoadedEvent { get => _OnSceneLoadedEvent; }
        public GameEvent OnLevelStartedEvent { get => _OnLevelStartedEvent; }
        public GameEvent OnStepOneCompleteEvent { get => _OnStepOneCompleteEvent; }
        public GameEvent OnStepTwoCompleteEvent { get => _OnStepTwoCompleteEvent; }
        public GameEvent OnLevelCompleteEvent { get => _OnLevelCompleteEvent; }
        public GameEvent OnLevelFailedEvent { get => _OnLevelFailedEvent; }
        #endregion

        #region Private Variables 
        [SerializeField, NaughtyAttributes.Foldout("State Events")] private GameEvent _OnDataLoadedEvent;
        [SerializeField, NaughtyAttributes.Foldout("State Events")] private GameEvent _OnSceneLoadBeginEvent;
        [SerializeField, NaughtyAttributes.Foldout("State Events")] private GameEvent _OnSceneLoadedEvent;
        [SerializeField, NaughtyAttributes.Foldout("State Events")] private GameEvent _OnLevelStartedEvent;
        [SerializeField, NaughtyAttributes.Foldout("State Events")] private GameEvent _OnStepOneCompleteEvent;
        [SerializeField, NaughtyAttributes.Foldout("State Events")] private GameEvent _OnStepTwoCompleteEvent;
        [SerializeField, NaughtyAttributes.Foldout("State Events")] private GameEvent _OnLevelCompleteEvent;
        [SerializeField, NaughtyAttributes.Foldout("State Events")] private GameEvent _OnLevelFailedEvent;

        [Separator("Managers")]
        [Label("UI Manager")]
        public UIManager _uiManager;
        public SaveManager _saveManager;
        public SceneManager _sceneManager;

        #endregion

        #region Monobehavior
        private void Awake()
        {
            Application.targetFrameRate = 120;

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

            }
            else
            {
                Destroy(gameObject);
            }
        }
        private IEnumerator Start()
        {
            yield return null;

            
        }

        private void OnEnable()
        {
            
        }
        private void OnDisable()
        {
            
        }
        #endregion

        #region Dependency Loader
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoadRuntimeMethod() {
            GameManager gameManager = Instantiate(Resources.Load<GameManager>("GameManager"));

            // TODO : Also initialize any other scripts that shall be useful
        }
        
        #endregion

    }
}

