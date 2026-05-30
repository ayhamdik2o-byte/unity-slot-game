using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

public class BuildAPK
{
    [MenuItem("Build/Build Queensati APK")]
    public static void BuildQueensatiAPK()
    {
        // تعيين الأيقونة والإعدادات
        PlayerSettings.companyName = "Queensati Games";
        PlayerSettings.productName = "Queensati";
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.queensati.slotgame");
        
        // إصدار التطبيق
        PlayerSettings.bundleVersion = "1.0";
        PlayerSettings.Android.bundleVersionCode = 1;
        
        // إعدادات Android
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel33;
        PlayerSettings.Android.useCustomKeystore = false;
        
        // الاتجاه
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
        
        // شاشات المستويات
        EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[2];
        scenes[0] = new EditorBuildSettingsScene("Assets/Scenes/LoginScene.unity", true);
        scenes[1] = new EditorBuildSettingsScene("Assets/Scenes/GameScene.unity", true);
        EditorBuildSettings.scenes = scenes;
        
        // مسار الحفظ
        string buildPath = "./Builds/Queensati.apk";
        
        // خيارات البناء
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions()
        {
            scenes = new[] { "Assets/Scenes/LoginScene.unity", "Assets/Scenes/GameScene.unity" },
            locationPathName = buildPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };
        
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;
        
        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"✅ تم بناء التطبيق بنجاح!\n📱 APK: {buildPath}\n📊 الحجم: {summary.totalSize / 1024 / 1024} MB");
            EditorUtility.RevealProjectFolder();
        }
        else if (summary.result == BuildResult.Failed)
        {
            Debug.LogError($"❌ فشل بناء التطبيق: {summary.totalErrors} أخطاء");
        }
    }
}
