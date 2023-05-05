using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

namespace grimhawk.toolbag.editor
{
    public class TagManagerPostProcessor : AssetPostprocessor
    {
        private const string tagManagerPath = "ProjectSettings/TagManager.asset";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var file in importedAssets)
            {
                if (file == tagManagerPath)
                {
                    TagsAndLayersTool.UpdateAllScripts();
                    break;
                }
            }
        }
    }

    //	Generates a type safe representation of the tags and layers you can set up in the project settings.
    //
    //	How To Install:
    //		Put this file somewhere in an 'Editor' folder.
    //
    //	How To Use:
    //		When you make changes to the tags and layers in the project settings the tool will generate two scripts 
    //		in the project folder which contain two static classes for Tags and Layers.
    //		
    //		You can change the path where the scripts are created in the Preferences window(Edit/Preferences).
    //	
    //	Scripting:
    //		if(collider.tag == Tags.Player) {
    //			//	do something
    //		}
    //	
    //		if(gameObject.layer == Layers.Enemy) {
    //			//	do something
    //		}
    public static class TagsAndLayersTool
    {
        private const string ClassFormat = @"using UnityEngine;
public static class {0}
{{
{1}
}}
";
        private static string EditorPrefsExportFolderKey
        {
            get
            {
                return string.Format("{0}.{1}.TagsAndLayers.ExportFolder", PlayerSettings.companyName, PlayerSettings.productName);
            }
        }

        private static string ExportFolder
        {
            get { return EditorPrefs.GetString(EditorPrefsExportFolderKey, "/Plugins/"); }
            set { EditorPrefs.SetString(EditorPrefsExportFolderKey, value); }
        }

        [PreferenceItem("Grimhawk")]
        private static void OnPreferencesGUI()
        {
            EditorGUILayout.LabelField("Tags And Layers", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            GUILayout.Space(4.0f);
            EditorGUILayout.TextField("Export Folder", ExportFolder);
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("...", GUILayout.Width(50.0f), GUILayout.Height(18.0f)))
            {
                string oldExportFolder = ExportFolder;
                string exportFolder = EditorUtility.OpenFolderPanel("Select export folder", Application.dataPath, "");
                if (!string.IsNullOrEmpty(exportFolder))
                {
                    int index = exportFolder.IndexOf("Assets");
                    if (index >= 0 && index + 6 < exportFolder.Length)
                    {
                        exportFolder = exportFolder.Substring(index + 6);
                        exportFolder = (exportFolder[exportFolder.Length - 1] == '/') ? exportFolder : exportFolder + '/';
                        ExportFolder = exportFolder;
                        MoveScriptFiles(oldExportFolder, exportFolder);
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        [MenuItem("Window/Tools/Tags and Layers/Generate Tags")]
        public static void UpdateTagsScript()
        {
            CreateScriptFile("Tags.cs", GenerateTagsClass());
        }

        [MenuItem("Window/Tools/Tags and Layers/Generate Layers")]
        public static void UpdateLayersScript()
        {
            CreateScriptFile("Layers.cs", GenerateLayersClass());
        }

        [MenuItem("Window/Tools/Tags and Layers/Generate All")]
        public static void UpdateAllScripts()
        {
            CreateScriptFile("Tags.cs", GenerateTagsClass());
            CreateScriptFile("Layers.cs", GenerateLayersClass());
        }

        private static void CreateScriptFile(string filename, string content)
        {
            try
            {
                string exportFolder = Application.dataPath + ExportFolder;
                if (!Directory.Exists(exportFolder))
                {
                    Directory.CreateDirectory(exportFolder);
                }

                using (var writer = File.CreateText(exportFolder + filename))
                {
                    writer.Write(content);
                }
                AssetDatabase.Refresh();
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private static void MoveScriptFiles(string oldExportFolder, string newExportFolder)
        {
            try
            {
                oldExportFolder = Application.dataPath + oldExportFolder;
                newExportFolder = Application.dataPath + newExportFolder;

                if (!Directory.Exists(newExportFolder))
                {
                    Directory.CreateDirectory(newExportFolder);
                }

                if (File.Exists(oldExportFolder + " Tags.cs"))
                {
                    File.Move(oldExportFolder + "Tags.cs", newExportFolder + "Tags.cs");
                }
                else
                {
                    GenerateTagsClass();
                }

                if (File.Exists(oldExportFolder + " Layers.cs"))
                {
                    File.Move(oldExportFolder + "Layers.cs", newExportFolder + "Layers.cs");
                }
                else
                {
                    GenerateLayersClass();
                }

                AssetDatabase.Refresh();
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private static string GenerateTagsClass()
        {
            StringBuilder builder = new StringBuilder();
            string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

            for (int i = 0; i < tags.Length; i++)
            {
                string tagName = tags[i].Replace(' ', '_');
                builder.AppendFormat("\tpublic const string {0} = \"{0}\";\r\n", tagName);
            }

            return string.Format(ClassFormat, "Tags", builder.ToString());
        }

        private static string GenerateLayersClass()
        {
            StringBuilder builder = new StringBuilder();
            string[] layers = UnityEditorInternal.InternalEditorUtility.layers;

            for (int i = 0; i < layers.Length; i++)
            {
                string layerName = layers[i].Replace(' ', '_');
                int layerValue = LayerMask.NameToLayer(layers[i]);

                builder.AppendFormat("\tpublic const int {0} = {1};\r\n", layerName, layerValue);
            }

            return string.Format(ClassFormat, "Layers", builder.ToString());
        }
    }
}