using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
static class SVBookmarkManager 
{
    public static event Action<int> MoveToBookmarkEvent;
    public static event Action<int, SceneViewBookmark, Texture2D> SetBookmarkEvent;
}
