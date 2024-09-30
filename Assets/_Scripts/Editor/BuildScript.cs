#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Security.AccessControl;
using UnityEditor.SceneManagement;
using System.Text.RegularExpressions;
using Keylid.Footballop;
using static UnityEngine.GraphicsBuffer;
using System.Diagnostics;
using System.IO;

namespace Keylid.Footballop.Editor
{
    public class BuildScript : MonoBehaviour
    {
        // Keys for saving the build path and build name in EditorPrefs
        public const string buildPathKey = "AndroidBuildPath";
        public const string buildNameKey = "AndroidBuildName";
        public const string appVersionKey = "AppVersion";
        public const string bundleVersionKey = "BundleVersion";
        public static string defaultAppVersion = "1.0.0";
        public static int defaultBundleVersion = 1;
        // Default values
        public static string defaultBuildPath = "Builds";
        public static string defaultBuildName = "AndroidBuild.apk";
        [MenuItem("Build/Configure Build Settings")]
        public static void OpenBuildPathWindow()
        {
            BuildPathWindow.ShowWindow();
        }
        [MenuItem("Build/Build Android APK")]
        public static void BuildAndroid()
        {
            string buildPath = EditorPrefs.GetString(buildPathKey, defaultBuildPath);
            string buildName = EditorPrefs.GetString(buildNameKey, defaultBuildName);
            string appVersion = EditorPrefs.GetString(appVersionKey, defaultAppVersion);
            string fullBuildPath = $"{buildPath}/{buildName}";

            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            }
            string suffix = " Sadad";
            suffix += " " + appVersion;
            suffix += ".apk";
            fullBuildPath += suffix;
            string[] scenes = GetEnabledScenes();
            BuildPipeline.BuildPlayer(scenes, fullBuildPath, BuildTarget.Android, BuildOptions.None);
            UnityEngine.Debug.Log($"Android build completed. If Successful, The APK should be located at: {fullBuildPath}");
        }
        private static string[] GetEnabledScenes()
        {
            var scenes = EditorBuildSettings.scenes;
            string[] enabledScenes = new string[scenes.Length];
            int index = 0;

            foreach (EditorBuildSettingsScene scene in scenes)
            {
                if (scene.enabled)
                {
                    enabledScenes[index] = scene.path;
                    index++;
                }
            }
            System.Array.Resize(ref enabledScenes, index);

            return enabledScenes;
        }
    }

    public class BuildPathWindow : EditorWindow
    {
        private string buildPath;
        private string buildName;
        private string appVersion;
        private int bundleVersion;
        public static void ShowWindow()
        {
            GetWindow<BuildPathWindow>("Android Build Settings");
        }
        private void OnEnable()
        {
            buildPath = EditorPrefs.GetString(BuildScript.buildPathKey, BuildScript.defaultBuildPath);
            buildName = EditorPrefs.GetString(BuildScript.buildNameKey, BuildScript.defaultBuildName);
            bundleVersion = EditorPrefs.GetInt(BuildScript.bundleVersionKey, BuildScript.defaultBundleVersion);
            appVersion = EditorPrefs.GetString(BuildScript.appVersionKey, BuildScript.defaultAppVersion);
        }
        private void OnGUI()
        {
            GUILayout.Label("Configure Android Build Settings", EditorStyles.boldLabel);
            buildPath = EditorGUILayout.TextField("Build Path", buildPath);
            buildName = EditorGUILayout.TextField("Build Name", buildName);
            appVersion = EditorGUILayout.TextField("App Version", appVersion);
            bundleVersion = EditorGUILayout.IntField("Bundle Version", bundleVersion);
            if (GUILayout.Button("Save Settings"))
            {
                PlayerSettings.bundleVersion = appVersion;
                PlayerSettings.Android.bundleVersionCode = bundleVersion;
                EditorPrefs.SetString(BuildScript.buildPathKey, buildPath);
                EditorPrefs.SetString(BuildScript.buildNameKey, buildName);
                EditorPrefs.SetString(BuildScript.appVersionKey, appVersion);
                EditorPrefs.SetInt(BuildScript.bundleVersionKey, bundleVersion);
                UnityEngine.Debug.Log($"Build settings saved: {buildPath}/{buildName} and the app version is " + appVersion +
                   " and the bundle version is " + bundleVersion);
            }
        }
    }
}
#endif