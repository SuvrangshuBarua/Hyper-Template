using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;

public class SceneSwitcher : EditorWindow
{
    #region Private Variables
    private List<SceneAsset> _scenes = null;
    private List<Color> _buttonColors = null;

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
    private void UpdateTextColorAccordingToButtonColor(Color color, GUIStyle style)
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
    #endregion

    #region PresistantData
    ///<summary>
    /// Save scenes with index and button color
    /// Also consider if it is in editmode
    /// </summary>

    ///<summary>
    /// Load and perse preferences from saved data for the scenes
    /// </summary>
    #endregion
}
