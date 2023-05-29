using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Has a few responsibilities 
/// 1. Publishes events reporting bookmark opened, saved and removed 
/// 2. A getter for Scene View Bookmark Icon
/// 3. A getter for Bookmark Icon
/// 4. Open Bookmark Method that takes bookmark index as args
/// 5. Switch To Previous Method to jump to previous bookmark, takes no args but previousBookmarkIndex field
/// 6. Save Bookmark Method to save bookmark, takes SVBookmark instance, index, previewTexture
/// 7. 
/// </summary>
static class SVBookmarkManager 
{
    public static event Action<int> MoveToBookmarkEvent;
    public static event Action<int, SVBookmark, Texture2D> SetBookmarkEvent;
    public static event Action ClearAllBookmarkEvent;

    private const string SVBookmarkIconBlackGUID = "";
    private const string SVBookmarkIconWhiteGUID = "";
    private const string SVEmptyIconGUID = "";
}
