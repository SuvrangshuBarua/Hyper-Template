namespace grimhawk.tools.gameevent
{
    using grimhawk.core;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    [CreateAssetMenu(fileName = "GameEvent", menuName = "Grim/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        #region Custom Classes
        private class GameEventResponse
        {
            #region Public Variables
            public GameEventListener GameEventListenerReference { get; private set; }
            public Action GameEventResponseReference { get; private set; }
            #endregion

            #region Public Callbacks
            public GameEventResponse(GameEventListener gameEventListener, Action gameEventResponse)
            { 
                GameEventListenerReference = gameEventListener;
                GameEventResponseReference = gameEventResponse;
            }
            #endregion

        }
        #endregion

        #region Private Variables
        private List<GameEventResponse> _allGameEventResponseList = new List<GameEventResponse>();
        private Dictionary<GameEventListener, GameEventResponse> _gameEventListenerCollection = new Dictionary<GameEventListener, GameEventResponse>();
        #endregion

        #region Public Callbacks
        public void RegisterEvent(GameEventListener gameEventListener, Action gameEventResponse)
        {
            if(!_gameEventListenerCollection.ContainsKey(gameEventListener))
            {
                GameEventResponse newGameEventResponse = new GameEventResponse(gameEventListener, gameEventResponse);

                _allGameEventResponseList.Add(newGameEventResponse);
                _gameEventListenerCollection.Add(gameEventListener, newGameEventResponse);
            }
        }
        public void UnregisterEvent(GameEventListener gameEventListener)
        {
            if(_gameEventListenerCollection.ContainsKey(gameEventListener))
            {
                GameEventResponse removeableGameEventResponse;
                if(_gameEventListenerCollection.TryGetValue(gameEventListener, out removeableGameEventResponse))
                {
                    _allGameEventResponseList.Remove(removeableGameEventResponse);
                    _gameEventListenerCollection.Remove(gameEventListener);
                    
                }
            }
        }

        public void Raise()
        {
            int numberOfResponse = _allGameEventResponseList.Count;
            Debug.Log($"GameEvent: {name} -- Response Count = {numberOfResponse}");
            for (int i = numberOfResponse - 1; i >= 0; i--)
            {
                _allGameEventResponseList[i].GameEventResponseReference?.Invoke();
            }
        }
        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor:Editor
    {
        #region Private Variables

        private GameEvent _reference;

        #endregion

        #region Editor

        private void OnEnable()
        {
            _reference = (GameEvent)target;

            if (_reference == null)
                return;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            if (GUILayout.Button("Raise"))
            {
                _reference.Raise();
            }

            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
#endif
}