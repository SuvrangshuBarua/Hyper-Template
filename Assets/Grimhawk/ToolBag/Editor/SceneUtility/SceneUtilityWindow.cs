using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using System;
using SceneManager = UnityEngine.SceneManagement.SceneManager;
using System.Linq;
/// <summary>
/// Scene Utility window, an editor window for managing scene
/// </summary>
public class SceneUtilityWindow : EditorWindow
{
    public enum ScenesSource
    {
        BuildSettings,
        Project,
        Manual
    }
    public enum BuildSceneOperation
    {
        Singular = 0,
        Append = 1
    }
    private Vector2 _scrollPosition;
    private Vector2 _scenesTabScrollPosition;
    private ScenesSource _scenesSource = ScenesSource.BuildSettings;
    private NewSceneMode _newSceneMode = NewSceneMode.Single;
    private NewSceneSetup _newSceneSetup = NewSceneSetup.DefaultGameObjects;
    private OpenSceneMode _openSceneMode = OpenSceneMode.Single;
    private BuildSceneOperation _buildSceneOperation = BuildSceneOperation.Singular;
    private bool _showPath = false;
    private bool _showAddToBuild = true;
    private bool _askBeforeDelete = true;
    private bool[] _selectedScenes;
    private string[] _guids;
    private int _selectedTab = 0;
    private string _lastScene;
    private string[] _tabs = new string[] { "Scenes", "Settings" };
    private string _searchFolder = "Assets";

    private GUIStyle _style;
    public GUIStyle Style { 
        get 
        {
            _style = new GUIStyle(GUI.skin.label) { fontSize = 12 };
            _style.normal.textColor = Color.cyan;
            return _style;
        }
        set
        {

        }
    }
    [MenuItem("Window/Scene Utility")]
    public static void Init()
    {
        var window = EditorWindow.GetWindow<SceneUtilityWindow>("Scene Utility");
        window.minSize = new Vector2(400f, 200f);
        window.Show();
    }
    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += PlayModeStateChanged;
        _scenesSource = (ScenesSource)EditorPrefs.GetInt("SceneUtility.sceneSource", (int)ScenesSource.BuildSettings);
        _searchFolder = EditorPrefs.GetString("SceneUtility.searchFolder", "Assets");
        _newSceneSetup = (NewSceneSetup)EditorPrefs.GetInt("SceneUtility.newSceneSetup", (int)NewSceneSetup.DefaultGameObjects);
        _newSceneMode = (NewSceneMode)EditorPrefs.GetInt("SceneUtility.newSceneMode", (int)NewSceneMode.Single);
        _openSceneMode = (OpenSceneMode)EditorPrefs.GetInt("SceneUtility.openSceneMode", (int)OpenSceneMode.Single);
        _buildSceneOperation = (BuildSceneOperation) EditorPrefs.GetInt("SceneUtility.buildSceneOperation", (int)BuildSceneOperation.Singular);
        _showPath = EditorPrefs.GetBool("SceneUtility.showPath", false);
        _showAddToBuild = EditorPrefs.GetBool("SceneUtility.showAddToBuild", true);
        _askBeforeDelete = EditorPrefs.GetBool("SceneUtility.askBeforeDelete", true);

        
    }
    private void PlayModeStateChanged(PlayModeStateChange state)
    {
        Debug.Log($"Current State : {state} , Last Scene : {_lastScene}");
    }
    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= PlayModeStateChanged;
        EditorPrefs.SetInt("SceneUtility.sceneSource", (int)_scenesSource);
        EditorPrefs.SetString("SceneUtility.searchFolder", _searchFolder);
        EditorPrefs.SetInt("SceneUtility.newSceneSetup", (int)_newSceneSetup);
        EditorPrefs.SetInt("SceneUtility.newSceneMode", (int)_newSceneMode);
        EditorPrefs.SetInt("SceneUtility.openSceneMode", (int)_openSceneMode);
        EditorPrefs.SetInt("SceneUtility.buildSceneOperation", (int)_buildSceneOperation);
        EditorPrefs.SetBool("SceneUtility.showPath", _showPath);
        EditorPrefs.SetBool("SceneUtility.showAddToBuild", _showAddToBuild);
        EditorPrefs.SetBool("SceneUtility.askBeforeDelete", _askBeforeDelete);
    }
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        _selectedTab = GUILayout.Toolbar(_selectedTab, _tabs, EditorStyles.toolbarButton);
        EditorGUILayout.EndHorizontal();
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        EditorGUILayout.BeginVertical();
        switch (_selectedTab)
        {
            case 0:
                ScenesTabGUI();
                break;
            case 1:
                SettingsTabGUI();
                break;
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
        GUILayout.Label("Made by ♠Saiyajinn♠", EditorStyles.centeredGreyMiniLabel);
    }
    private void SettingsTabGUI()
    {
        _scenesSource = (ScenesSource)EditorGUILayout.EnumPopup("Scenes Source",_scenesSource);
        if(_scenesSource == ScenesSource.Manual)
        {
            _searchFolder = EditorGUILayout.TextField("Search Folder", _searchFolder);
        }
        //_newSceneSetup = (NewSceneSetup)EditorGUILayout.EnumPopup("New Scene Setup", _newSceneSetup); ;
        //_newSceneMode = (NewSceneMode)EditorGUILayout.EnumPopup("New Scene Mode", _newSceneMode);
        _buildSceneOperation = (BuildSceneOperation)EditorGUILayout.EnumPopup("Build Scene Operation", _buildSceneOperation);
        _openSceneMode = (OpenSceneMode)EditorGUILayout.EnumPopup("Open Scene Mode", _openSceneMode);
        _showPath = EditorGUILayout.Toggle("Show Path", _showPath);
        _showAddToBuild = EditorGUILayout.Toggle("Show Add To Build", _showAddToBuild);
        _askBeforeDelete = EditorGUILayout.Toggle("Ask Before Delete", _askBeforeDelete);
    }
    private void ScenesTabGUI()
    {
        List<EditorBuildSettingsScene> buildScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        _guids = AssetDatabase.FindAssets("t:Scene");
        if (_selectedScenes == null || _selectedScenes.Length != _guids.Length)
            _selectedScenes = new bool[_guids.Length];
        _scenesTabScrollPosition = EditorGUILayout.BeginScrollView(_scenesTabScrollPosition);
        EditorGUILayout.BeginVertical();
        if (_guids.Length == 0)
        {
            GUILayout.Label("No Scenes Can Be Found", EditorStyles.centeredGreyMiniLabel);
        }
        for (int i = 0; i < _guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(_guids[i]);
            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
            EditorBuildSettingsScene buildScene = buildScenes.Find((editorBuildScene) => editorBuildScene.path == path);
            Scene scene = SceneManager.GetSceneByPath(path);
            bool isOpen = scene.IsValid() && scene.isLoaded;
            var asset = UnityEditor.PackageManager.PackageInfo.FindForAssetPath(path);
            if (asset != null)
            {
                switch (asset.source)
                {
                    case PackageSource.Unknown:                        
                    case PackageSource.Registry:                       
                    case PackageSource.BuiltIn:                       
                    case PackageSource.Git:                       
                    case PackageSource.LocalTarball:
                        continue;
                    case PackageSource.Embedded:
                        break;
                    case PackageSource.Local:
                        break;
                    
                }
            }
            switch (_scenesSource)
            {
                case ScenesSource.BuildSettings:
                    if( buildScene == null )
                    {
                        continue;
                    }
                    break;
                case ScenesSource.Manual:
                    if(!path.Contains(_searchFolder))
                    {
                        continue;
                    }
                    break;
            }
            EditorGUILayout.BeginHorizontal();
            _selectedScenes[i] = EditorGUILayout.Toggle(_selectedScenes[i], GUILayout.Width(15));
            if(isOpen)
            {
                GUILayout.Label( 
                    sceneAsset.name, Style ) ;
            }
            else
            {
                GUILayout.Label(sceneAsset.name, EditorStyles.wordWrappedLabel);
            }

            if(_showPath)
            {
                GUILayout.Label(path, EditorStyles.wordWrappedLabel);
            }

            if(_showAddToBuild)
            {
                if(GUILayout.Button(buildScene == null ? "+ Build" : "- Build", GUILayout.Width(60)))
                {
                    if(buildScene == null)
                        AddToBuildSettings(path);
                    else
                        RemoveFromBuildSettings(path);
                }
            }
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            if(GUILayout.Button("Play", GUILayout.Width(50)))
            {
                _lastScene = EditorSceneManager.GetActiveScene().path;
                Open(path);
                EditorApplication.isPlaying = true;
            }
            EditorGUI.EndDisabledGroup();

            if(GUILayout.Button(isOpen ? "Close" : "Open", GUILayout.Width(50)))
            {
                if(isOpen)
                {
                    EditorSceneManager.CloseScene(scene, true);
                }
                else
                    Open(path);
            }
            if (GUILayout.Button("Ping", GUILayout.Width(50)))
                EditorGUIUtility.PingObject(sceneAsset);
            if (GUILayout.Button("Delete", GUILayout.Width(50)))
                Delete(path);

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Group Action →", EditorStyles.boldLabel);  
        bool anySelected = false;
        for (int i = 0; i < _selectedScenes.Length; i++)
        {
            anySelected |= _selectedScenes[i];
        }
        EditorGUI.BeginDisabledGroup(!anySelected);
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Delete"))
        {
            for (int i = 0; i < _selectedScenes.Length; i++)
            {
                if (_selectedScenes[i])
                    Delete(AssetDatabase.GUIDToAssetPath(_guids[i]));   
            }
        }
        if(GUILayout.Button("Open As Additive"))
        {
            OpenSceneMode openMode = _openSceneMode;
            _openSceneMode = OpenSceneMode.Additive;
            for (int i = 0; i < _selectedScenes.Length; i++)
            {
                if (_selectedScenes[i])
                {
                    Open(AssetDatabase.GUIDToAssetPath(_guids[i]));
                }
            }
            _openSceneMode= openMode;   
        }
        /*if(GUILayout.Button("Push To Build"))
        {
            BuildSceneOperation operation = _buildSceneOperation;
            _buildSceneOperation = BuildSceneOperation.Append;
            
            for (int i = 0; i < _selectedScenes.Length; i++)
            {
                if (_selectedScenes[i])
                    AddToBuildSettings(AssetDatabase.GUIDToAssetPath(_guids[i]));
            }
            _buildSceneOperation = operation;
        }*/
        EditorGUILayout.EndHorizontal();
        EditorGUI.EndDisabledGroup();
        GUILayout.Label("General Actions →", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Modified Scenes"))
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        if (GUILayout.Button("Save Open Scenes"))
            EditorSceneManager.SaveOpenScenes();
        EditorGUILayout.EndHorizontal();
    }
    private void Open(string path)
    {
        if (EditorSceneManager.EnsureUntitledSceneHasBeenSaved("You haven't saved the Untitled Scene, Do you want to leave"))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(path, _openSceneMode);
        }
    }
    private void Delete(string path)
    {
        if (!_askBeforeDelete || EditorUtility.DisplayDialog(
            "Delete Scene ?",
            String.Format("Are you sure you want to delete {0}", path),
            "Yes",
            "Cancel"
            ))
        {
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.Refresh();
        }
    }
    private void AddToBuildSettings(string path)
    {
        List<EditorBuildSettingsScene> scenes;
        switch ((int)_buildSceneOperation)
        {
            case 0:
                scenes = new List<EditorBuildSettingsScene>();
                scenes.Add(new EditorBuildSettingsScene(path, true));
                EditorBuildSettings.scenes = scenes.ToArray();
                break;
            case 1:
                scenes = EditorBuildSettings.scenes.ToList();
                scenes.Add(new EditorBuildSettingsScene(path, true));
                EditorBuildSettings.scenes = scenes.ToArray();
                break;
        }
        
        
        
    }
    private void RemoveFromBuildSettings(string path)
    {
        List<EditorBuildSettingsScene> scenes;
        switch ((int)_buildSceneOperation)
        {
            case 0:
                scenes = new List<EditorBuildSettingsScene>();
                scenes.RemoveAll(scene =>
                {
                    return scene.path == path;
                });
                EditorBuildSettings.scenes = scenes.ToArray();
                break;
            case 1:
                scenes= EditorBuildSettings.scenes.ToList();
                scenes.Remove(scenes.Find((editorBuildScene) => editorBuildScene.path == path));
                EditorBuildSettings.scenes = scenes.ToArray();
                break;
        }
        
    }
}





