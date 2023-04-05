namespace grimhawk.tools.build
{
    using System.IO;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using UnityEngine;

    [InitializeOnLoad]
    public static class OnInitialize
    {
        static OnInitialize()
        {
            if (!SessionState.GetBool("initialized", false))
            {
                var buildHelper = new BuildHelper();
                buildHelper.ApplyBuildSettings();
                SessionState.SetBool("initialized", true);
            }
        }
    }
#if UNITY_EDITOR
    public class BuildHelper : EditorWindow
    {
        [System.Serializable]
        public class BuildSetting
        {
            public string packageName;
            public string version;
            public int bundleVersionCode;
            public string keystorePath;
            public string keystorePassword;
            public string aliasName;
            public string aliasPassword;

            public BuildSetting()
            {
                packageName = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
                version = PlayerSettings.bundleVersion;
                bundleVersionCode = PlayerSettings.Android.bundleVersionCode;
                keystorePath = PlayerSettings.Android.keystoreName;
                keystorePassword = PlayerSettings.Android.keystorePass;
                aliasName = PlayerSettings.Android.keyaliasName;
                aliasPassword = PlayerSettings.Android.keyaliasPass;
            }
        }

        [MenuItem("Tools/Build Helper", false, -200)]
        private static void ShowWindow()
        {
            GetWindow(typeof(BuildHelper), true, "Build Helper").Show();
        }

        private const string buildSettingPath = "BuildSettings.json";
        public BuildSetting buildSetting;

        private void Awake()
        {
            buildSetting ??= new BuildSetting();

            if (File.Exists(buildSettingPath)) LoadBuildSettings();
        }

        private void OnGUI()
        {
            var serializedObject = new SerializedObject(this);
            var serializedProperty = serializedObject.FindProperty(("buildSetting"));

            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            ApplyBuildSettings();

            GUILayout.Space(10f);
            GUILayout.BeginHorizontal();

            if (File.Exists(buildSettingPath))
            {
                if (GUILayout.Button("Load Build Settings")) LoadBuildSettings();
            }

            if (GUILayout.Button("Save Build Settings")) SaveBuildSettings();

            GUILayout.EndHorizontal();
        }

        private void LoadBuildSettings()
        {
            var dataAsJson = File.ReadAllText(buildSettingPath);
            buildSetting = JsonUtility.FromJson<BuildSetting>(dataAsJson);
        }

        private void SaveBuildSettings()
        {
            var jsonData = JsonUtility.ToJson(buildSetting, true);
            File.WriteAllText(buildSettingPath, jsonData);
        }

        public void ApplyBuildSettings()
        {
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, buildSetting.packageName);
            PlayerSettings.bundleVersion = buildSetting.version;
            PlayerSettings.Android.bundleVersionCode = buildSetting.bundleVersionCode;
            PlayerSettings.Android.keystoreName = buildSetting.keystorePath;
            PlayerSettings.Android.keystorePass = buildSetting.keystorePassword;
            PlayerSettings.Android.keyaliasName = buildSetting.aliasName;
            PlayerSettings.Android.keyaliasPass = buildSetting.aliasPassword;
        }
    }
#endif
}

