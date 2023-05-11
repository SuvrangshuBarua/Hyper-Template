using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using UnityEditor.SceneManagement;

public class SceneSwitcher : EditorWindow
{
    #region Private Variables
    private List<SceneAsset> _scenes = null;
    private List<Color> _buttonColors = null;
    private bool _editMode = false;

    private GUILayoutOption _heightLayout;
    private Color _windowBackgroundColor = new Color(0.25f, 0.25f, 0.25f);
    private string PREFIX;
    #endregion
    private void OnEnable()
    {
        PREFIX = $"SceneSwitcher/{Application.companyName}/{Application.productName}";

        _scenes = new List<SceneAsset>();
        _buttonColors = new List<Color>();
        

        // AddScene();

        ///<summary>
        /// Define styles here for GUILayoutOption to describe how you want the window to look
        /// </summary>
        _heightLayout = GUILayout.Height(22);

        //Load scene data from EditorPrefs when window is opened
        //LoadPrefs();

    }
    [MenuItem("Window/SceneSwitcher")]
    static void Init()
    {
        SceneSwitcher sceneSwitcherWindow = (SceneSwitcher)GetWindow(typeof(SceneSwitcher));
        //sceneSwitcherWindow.show();
    }
    private void OnGUI()
    {
        
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
            SaveInPrefs();
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
        SaveInPrefs();  
    }

    #endregion

    #region PresistantData
    ///<summary>
    /// Save total scene count, scene with index and button color
    /// Also saves a boolean value that indicates if SceneSwitcher is in edit mode
    /// </summary>
    private void SaveInPrefs()
    {
        //Scene Count
        EditorPrefs.SetInt(PREFIX + "ScenesCount", _scenes.Count);
        for (int i = 0; i < _scenes.Count; i++)
        {
            //Scenes
            EditorPrefs.SetString(PREFIX + $"Scene_{i}", AssetDatabase.GetAssetPath(_scenes[i]));

            //Button Colors
            string sceneColor = ColorUtility.ToHtmlStringRGBA(_buttonColors[i]);
            EditorPrefs.SetString(PREFIX + $"SceneColor_{i}", $"#{sceneColor}");
        }

        //Edit Mode
        EditorPrefs.SetBool(PREFIX + "EditMode", _editMode);
    }
    ///<summary>
    /// Load and perse preferences from saved data for the scenes
    /// </summary>
    private void LoadFromPrefs()
    {
        int sceneCount = EditorPrefs.GetInt(PREFIX + "SceneCount");

        _scenes.Clear();
        _buttonColors.Clear();

        for (int i = 0; i < sceneCount; i++)
        {
            //Scenes
            //AddScene();

            //Scenes
            string scenePath = EditorPrefs.GetString(PREFIX + $"Scene_{i}");
            _scenes[i] = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

            //Parse Button Color from Scene Color
            string sceneColor = EditorPrefs.GetString(PREFIX + $"SceneColor_{i}");
            ColorUtility.TryParseHtmlString(sceneColor, out Color buttonColor);
            _buttonColors[i] = buttonColor; 
        }
    }
    #endregion
}
