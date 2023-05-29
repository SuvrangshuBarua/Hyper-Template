using UnityEngine;
using UnityEditor;
using System;
/// <summary>
/// Bookmark class to store actual bookmark data
/// </summary>
[System.Serializable]
public class SVBookmark
{
    #region Private Variables
    [SerializeField]
    private string name;
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private Vector3 rotation;
    [SerializeField]
    private bool isOrtho;
    [SerializeField]
    private bool is2D;
    [SerializeField]
    private float size;
    [SerializeField]
    private Texture2D thumbnail;
    #endregion

    #region Properties
    public string Name => name;
    public Vector3 Position => position;    
    public Vector3 Rotation => rotation;    
    public bool IsOrthographic => isOrtho;
    public bool Is2D => is2D;
    public float Size => size;
    public bool IsSet => !string.IsNullOrEmpty(Name);
    public Texture2D Thumbnail { get { return thumbnail; } internal set { thumbnail = value; } }
    #endregion

    /// <summary>
    /// Constructor to set fields of an instance
    /// </summary>
    /// <param name="name">Name of Bookmark</param>
    /// <param name="position">World Space position of camera</param>
    /// <param name="rotation">Rotaion in eular angles of camera</param>
    /// <param name="size">Size of camera</param>
    /// <param name="isOrthographic">Bookmark is orthographic</param>
    /// <param name="is2D">Bookmark using 2D mode</param>
    internal SVBookmark(string name, Vector3 position, Vector3 rotation, float size, bool isOrthographic,  bool is2D)
    {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
        this.size = size;
        this.isOrtho = isOrthographic;
        this.is2D = is2D;
    }
    /// <summary>
    /// Sets value for a given scene view as the value of bookmark
    /// </summary>
    /// <param name="sceneView">Target scene view</param>
    public void SetSceneViewAsBookmark(SceneView sceneView)
    {
        sceneView.pivot = position;
        sceneView.in2DMode = is2D;
        if(!sceneView.in2DMode)
        {
            sceneView.rotation = Quaternion.Euler(rotation);
        }
        sceneView.orthographic = isOrtho;
        sceneView.size = size;
    }
    /// <summary>
    /// Create a new bookmark object with a given name from the current camera view of a given scene view
    /// </summary>
    /// <param name="name">Name of the bookmark</param>
    /// <param name="sceneView">SceneView from which to generate the bookmark</param>
    /// <returns></returns>
    public static SVBookmark CreateFromSceneView(string name, SceneView sceneView)
    {
        return new SVBookmark(name, sceneView.pivot, sceneView.rotation.eulerAngles, sceneView.size, sceneView.orthographic, sceneView.in2DMode);
    }
}