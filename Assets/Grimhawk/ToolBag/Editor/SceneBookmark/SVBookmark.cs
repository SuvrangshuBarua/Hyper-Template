using UnityEngine;
using UnityEditor;
using System;

[Serializable]
struct SceneViewBookmark
{
    public Vector3 pivot;
    public Quaternion rotation;
    public float size;
    public bool in2DMode;

    public int svTextureSizeX;
    public int svTextureSizeY;

    public string svTextureBase64String;

    public SceneViewBookmark(SceneView sceneView, Texture2D sceneViewTexture)
    {
        this.pivot = sceneView.pivot;
        this.rotation = sceneView.rotation;
        this.size = sceneView.size;
        this.in2DMode = sceneView.in2DMode;

        svTextureSizeX = sceneViewTexture.width;
        svTextureSizeY= sceneViewTexture.height;
        svTextureBase64String = System.Convert.ToBase64String(sceneViewTexture.EncodeToPNG());
    }
    public Texture2D GetSVTexture()
    {
        if (string.IsNullOrEmpty(svTextureBase64String)) return null;
        byte[] textureBytes = System.Convert.FromBase64String(svTextureBase64String);
        var texture = new Texture2D(svTextureSizeX, svTextureSizeY);
        texture.LoadImage(textureBytes);

        return texture; 
    }
}
