using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using UnityEditor.SceneManagement;
using UnityEditorInternal;

public class SceneSwitcher : EditorWindow
{
    #region Private Variables
    private List<SceneAsset> _scenes = null;
    private List<Color> _buttonColors = null;
    private bool _editMode = false;

    private GUILayoutOption _heightLayout;
    private Color _windowBackgroundColor = new Color(0.25f, 0.25f, 0.25f);
    private string PREFIX;
    private Vector2 _scrollPosition = Vector2.zero;
    private const float _fixedSpace = 5f;
    private Font _globalFont;
    #endregion
    private void OnEnable()
    {
        PREFIX = $"SceneSwitcher/{Application.companyName}/{Application.productName}";

        _scenes = new List<SceneAsset>();
        _buttonColors = new List<Color>();
        

         AddScene();

        ///<summary>
        /// Define styles here for GUILayoutOption to describe how you want the window to look
        /// </summary>
        _heightLayout = GUILayout.Height(22);
        _globalFont = Resources.Load<Font>("Fonts/Roboto-Thin");
        //Load scene data from EditorPrefs when window is opened
        LoadPrefs();

    }
    private void OnDisable()
    {
        Resources.UnloadAsset(_globalFont);
        SavePrefs();
    }
    [MenuItem("Window/SceneSwitcher")]
    static void Init()
    {
        GetWindow<SceneSwitcher>("Scene Switcher");
    }
    private void OnGUI()
    {
        //Button Style
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fixedHeight = 22,
            font = _globalFont,
            fontStyle = FontStyle.Bold
        };

        //Title Label Style
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            font = _globalFont,
            fontStyle = FontStyle.Bold,
            fixedHeight = 22,
            fontSize = 14
        };

        GUIStyle genericLabelStyle = new GUIStyle(GUI.skin.toggle)
        {
            font = _globalFont,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleRight

        };
        Color contentColor = GUI.contentColor;

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

        GUILayout.Space(_fixedSpace);
        // Content from here would stay at horizontal position
        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent(" Scene Switcher"), titleStyle);
        GUILayout.FlexibleSpace();

        //Toggle  'EditMode' button
        /*GUI.backgroundColor = _editMode ? GetButtonColorBasedOnEditorTheme(Color.white) : _darkGrey;
        ButtonTextColorCorrection(GUI.backgroundColor, buttonStyle);
        
        _editMode = GUILayout.Toggle(_editMode, _editMode ? "Exit Edit Mode" : "Edit Mode", buttonStyle);
        GUI.backgroundColor = contentColor;*/

        _editMode = GUILayout.Toggle(_editMode, "Edit", genericLabelStyle);


        GUILayout.EndHorizontal();
        GUILayout.Space(3);

        for (int i = 0; i < _scenes.Count; i++)
        {
            if (_editMode)
            {
                GUILayout.BeginHorizontal();

                //Remove Scene Button
                bool isSceneRemoved = false;
                if (GUILayout.Button("X", _heightLayout, GUILayout.MaxWidth(25)))
                {
                    RemoveScene(i);
                    isSceneRemoved = true;
                }

                if (!isSceneRemoved)
                {
                    //Move Scene Up And Down Buttons
                    if (GUILayout.Button("↑", _heightLayout, GUILayout.MaxWidth(25))) MoveScene(i, -1);
                    if (GUILayout.Button("↓", _heightLayout, GUILayout.MaxWidth(25))) MoveScene(i, 1);

                    //Scene Asset Field
                    _scenes[i] = EditorGUILayout.ObjectField(_scenes[i], typeof(SceneAsset), false, _heightLayout) as SceneAsset;
                    //Scene Button Color Field
                    _buttonColors[i] = EditorGUILayout.ColorField(_buttonColors[i], _heightLayout, GUILayout.MaxWidth(80));
                }

                GUILayout.EndHorizontal();
            }
            else //if NOT in 'Edit'
            {
                //Disable button if it corresponds to the currently open scene
                bool isCureentScene = IsCurrentScene(_scenes[i]);
                if (isCureentScene) EditorGUI.BeginDisabledGroup(true);

                //Setup button background and text colors
                GUI.backgroundColor = GetButtonColorBasedOnEditorTheme(_buttonColors[i]);
                ButtonTextColorCorrection(GUI.backgroundColor, buttonStyle);

                //Draw Scene Button
                if (_scenes[i] != null && GUILayout.Button($"{_scenes[i].name}{(isCureentScene ? "(current)" : "")}", buttonStyle))
                    OpenScene(_scenes[i]);

                GUI.backgroundColor = contentColor;
                EditorGUI.EndDisabledGroup();
            }

            

            
        }
        if (_editMode) //'Edit' bottom options
        {
            GUILayout.Space(5);

            GUI.backgroundColor = _windowBackgroundColor;
            ButtonTextColorCorrection(GUI.backgroundColor, buttonStyle);
            float previousHeight = buttonStyle.fixedHeight;
            buttonStyle.fixedHeight = 25;

            if (GUILayout.Button("Add Scene", buttonStyle))
                AddScene();

            GUILayout.Space(1);

            //'Load From BuildSettings' Button Style
            buttonStyle.fixedHeight = previousHeight;
            GUI.backgroundColor = GetButtonColorBasedOnEditorTheme(contentColor);
            ButtonTextColorCorrection(GUI.backgroundColor, buttonStyle);

            if (GUILayout.Button("Load From BuildSettings", buttonStyle)) LoadScenesFromBuildSettings();
        }
        GUILayout.EndScrollView();

        if (GUI.changed) SavePrefs();
    }

    #region Custom Methods
    ///<summary>
    /// This method chooses black or white text color according to button color
    /// </summary>
    private void ButtonTextColorCorrection(Color color, GUIStyle style)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        bool shouldColorBeWhite = (v < 0.7f || (s > 0.4f && (h < 0.1f || h > 0.55f)));
        Color textColor = shouldColorBeWhite ? Color.white : Color.black;
        Color onHoverTextColor = textColor + (shouldColorBeWhite ? Color.black * -0.2f : Color.white * 0.2f);

        style.normal.textColor = textColor;
        style.hover.textColor = onHoverTextColor;
        style.focused.textColor = textColor;
        style.active.textColor = textColor;

        style.onNormal.textColor = textColor;
        style.onHover.textColor = onHoverTextColor;
        style.onFocused.textColor = textColor;
        style.onActive.textColor = textColor;
    }

    private Color GetButtonColorBasedOnEditorTheme(Color buttonColor)
    {
        Color editorColor = EditorGUIUtility.isProSkin ? (Color.white * 0.3f) : Color.black;
        return buttonColor * (EditorGUIUtility.isProSkin ? 2f : 1f) + editorColor;  
    }
    #endregion

    #region SceneManagement

    /// <summary>
    /// Opens <param name="scene"/> 
    /// </summary>
    private void OpenScene(SceneAsset scene)
    {
        string scenePath = AssetDatabase.GetAssetPath(scene);
        ///<summery>
        /// Checks if there are any unsaved changes, if any ? prompts if user wants to save the changes 
        /// If the use chooses to cancel, doesn't change scene
        /// </summery>
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
            SavePrefs();
        }
    }
    /// <summary>
    /// Adds a null scene slot 
    /// </summary>
    private void AddScene()
    {
        _scenes.Add(null);
        _buttonColors.Add(Color.white); 
    }
    /// <summary>
    /// Removes scene at specified  <param name="index"></param>
    /// </summary>
    private void RemoveScene(int index)
    {
        _scenes.RemoveAt(index);
        _buttonColors.RemoveAt(index);
        SavePrefs();  
    }
    private void MoveScene(int index, int value)
    {
        int newIndex = Mathf.Clamp(index + value, 0, _scenes.Count - 1);
        if(newIndex != index)
        {
            SceneAsset scene = _scenes[index];
            Color btnColor = _buttonColors[index];
            RemoveScene(index);

            _scenes.Insert(newIndex, scene);
            _buttonColors.Insert(newIndex, btnColor);
        }
        SavePrefs();
    }

    private bool IsCurrentScene(SceneAsset scene) => EditorSceneManager.GetActiveScene().path == AssetDatabase.GetAssetPath(scene);
        
    private void LoadScenesFromBuildSettings()
    {
        foreach (var scene in EditorBuildSettings.scenes)
        {
            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);

            if(!_scenes.Contains(sceneAsset))
            {
                int index;
                if (_scenes.Contains(null)) index = _scenes.IndexOf(null);
                else
                {
                    AddScene();
                    index = _scenes.Count - 1;
                }

                _scenes[index] = sceneAsset;
            }
        }
    }
    #endregion

    #region PersistentData

    // Save current preferences to persistent data
    private void SavePrefs()
    {
        // Scenes Count
        EditorPrefs.SetInt(PREFIX + "ScenesCount", _scenes.Count);

        for (int i = 0; i < _scenes.Count; i++)
        {
            // Scenes
            EditorPrefs.SetString(PREFIX + $"Scene_{i}", AssetDatabase.GetAssetPath(_scenes[i]));

            // Button Colors
            string sceneColor = ColorUtility.ToHtmlStringRGBA(_buttonColors[i]);
            EditorPrefs.SetString(PREFIX + $"SceneColor_{i}", $"#{sceneColor}");
        }

        // Edit Mode
        EditorPrefs.SetBool(PREFIX + $"EditMode", _editMode);
    }

    // Load and parse preferences from persistent saved data
    private void LoadPrefs()
    {
        // Scenes Count
        int scenesCount = EditorPrefs.GetInt(PREFIX + "ScenesCount");

        _scenes.Clear();
        _buttonColors.Clear();

        for (int i = 0; i < scenesCount; i++)
        {
            // Scenes
            AddScene();
            string scenePath = EditorPrefs.GetString(PREFIX + $"Scene_{i}");
            _scenes[i] = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

            // Button Colors
            string sceneColorString = EditorPrefs.GetString(PREFIX + $"SceneColor_{i}");
            ColorUtility.TryParseHtmlString(sceneColorString, out Color sceneColor);
            _buttonColors[i] = sceneColor;
        }

        // Edit Mode
        _editMode = EditorPrefs.GetBool(PREFIX + $"EditMode");
    }

    #endregion
}
