#if UNITY_EDITOR
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GrimConfig : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        SetConditionalCompilation();
    }

    private static void SetConditionalCompilation()
    {
        var filePreviousLocation = Application.dataPath + "/csc.rsp";

        string[,] sdks = new string[,]
        {
            { "Adjust", "GRIM_ADJUST_SDK_INSTALLED"},
            { "FacebookSDK", "GRIM_FACEBOOK_SDK_INSTALLED"},
            { "GameAnalytics","GRIM_GAMEANALYTICS_SDK_INSTALLED"},
            {"SupersonicWisdom", "SUPERSONIC_WISDOM_SDK_INSTALLED" }
        };
        if (File.Exists(filePreviousLocation))
            File.WriteAllText(filePreviousLocation, "");

        FileStream fileStream= File.Open(filePreviousLocation, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamWriter sw_defines = new StreamWriter(fileStream);
        for (int i = 0; i < sdks.Length / 2; i++)
        {
            var sdkNames = sdks[i, 0];
            var sdkDefines = sdks[i, 1];
            if(Directory.Exists(Application.dataPath + "/" + sdkNames))
            {
                sw_defines.WriteLine("~define:" + sdkDefines);
            }
        }
        sw_defines.Close();
        fileStream.Close(); 
    }
}
#endif