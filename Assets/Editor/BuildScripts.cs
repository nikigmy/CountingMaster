using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildScripts : MonoBehaviour
{
    [MenuItem("Build/Build Mac")]
    public static void Build_Mac()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "mac/" + Application.productName + ".app";
        buildPlayerOptions.target = BuildTarget.StandaloneOSX;
        buildPlayerOptions.options = BuildOptions.None;
        buildPlayerOptions.scenes = GetScenes();

        Debug.Log("Building StandaloneOSX");
        Build(buildPlayerOptions);
        Debug.Log("Built StandaloneOSX");
    }
    
    [MenuItem("Build/Build Desktop")]
    public static void Build_Desktop()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "desktop/" + Application.productName;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows;
        buildPlayerOptions.options = BuildOptions.None;
        buildPlayerOptions.scenes = GetScenes();

        Debug.Log("Building Desktop");
        Build(buildPlayerOptions);
        Debug.Log("Built Desktop");
    }

    [MenuItem("Build/Build Android")]
    private static void Build_Android()
    {
        PlayerSettings.Android.useCustomKeystore = true;
        EditorUserBuildSettings.buildAppBundle = false;
        
        SetupAndroidBuild();
        
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "android/" + Application.productName;
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;
        buildPlayerOptions.scenes = GetScenes();
        

        Debug.Log("Building Android");
        Build(buildPlayerOptions);
        Debug.Log("Built Android");
    }

    [MenuItem("Build/Build Android Bundle")]
    private static void Build_Android_Bundle()
    {
        PlayerSettings.Android.useCustomKeystore = true;
        EditorUserBuildSettings.buildAppBundle = true;
        
        SetupAndroidBuild();

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        
        buildPlayerOptions.locationPathName = "android/android.aab";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;
        buildPlayerOptions.scenes = GetScenes();
        EditorUserBuildSettings.buildAppBundle = false;

        Debug.Log("Building Android Bundle");
        Build(buildPlayerOptions);
        Debug.Log("Built Android Bundle");
    }

    private static void SetupAndroidBuild()
    {
        // Set bundle version. NEW_BUILD_NUMBER environment variable is set in the codemagic.yaml 
        bool versionIsSet = int.TryParse(Environment.GetEnvironmentVariable("NEW_BUILD_NUMBER"), out int version);

        if (versionIsSet)
        {
            Debug.Log($"Bundle version code set to {version}");
            PlayerSettings.Android.bundleVersionCode = version;
        }
        else
            Debug.Log("Bundle version not provided");

        string keystoreName = Environment.GetEnvironmentVariable("FCI_KEYSTORE_PATH");

        if (!string.IsNullOrEmpty(keystoreName))
        {
            Debug.Log($"Setting path to keystore: {keystoreName}");
            PlayerSettings.Android.keystoreName = keystoreName;
        }
        else
            Debug.Log("Keystore name not provided");

        // Set keystore password
        string keystorePass = Environment.GetEnvironmentVariable("FCI_KEYSTORE_PASSWORD");

        if (!string.IsNullOrEmpty(keystorePass))
        {
            Debug.Log("Setting keystore password");
            PlayerSettings.Android.keystorePass = keystorePass;
        }
        else
            Debug.Log("Keystore password not provided");

        // Set keystore alias name
        string keyaliasName = Environment.GetEnvironmentVariable("FCI_KEY_ALIAS");

        if (!string.IsNullOrEmpty(keyaliasName))
        {
            Debug.Log("Setting keystore alias");
            PlayerSettings.Android.keyaliasName = keyaliasName;
        }
        else
            Debug.Log("Keystore alias not provided");

        // Set keystore alias password
        string keyaliasPass = Environment.GetEnvironmentVariable("FCI_KEY_PASSWORD");

        if (!string.IsNullOrEmpty(keyaliasPass))
        {
            Debug.Log("Setting keystore alias password");
            PlayerSettings.Android.keyaliasPass = keyaliasPass;
        }
        else
            Debug.Log("Keystore alias password not provided");
    }

    [MenuItem("Build/Build iOS (development)")]
    private static void Build_iOS_Development()
    {
        Build_iOS(BuildOptions.Development | BuildOptions.AllowDebugging | BuildOptions.WaitForPlayerConnection);
    }

    [MenuItem("Build/Build iOS (profiling)")]
    private static void Build_iOS_Profile()
    {
        Build_iOS(BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging);
    }
    
    private static void Build_iOS(BuildOptions buildOptions)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.locationPathName = "ios/"  + Application.productName;
        buildPlayerOptions.target = BuildTarget.iOS;
        buildPlayerOptions.scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray();
        buildPlayerOptions.options = buildOptions;

        Debug.Log("Building IOS");
        Build(buildPlayerOptions);
        Debug.Log("Built IOS");
    }

    private static void Build(BuildPlayerOptions buildOptions)
    {
        BuildPipeline.BuildPlayer(buildOptions);
    }

    static void IncrementBuildNumber(BuildTarget target)
    {
        int buildNum = 0;
        int newBuildNum = 0;
        switch(target)
        {
            case BuildTarget.tvOS:
                buildNum = Int32.Parse(PlayerSettings.tvOS.buildNumber);
                newBuildNum = buildNum + 1;
                PlayerSettings.tvOS.buildNumber = newBuildNum.ToString();
                break;

            case BuildTarget.StandaloneOSX:
                buildNum = Int32.Parse(PlayerSettings.macOS.buildNumber);
                newBuildNum = buildNum + 1;
                PlayerSettings.macOS.buildNumber = newBuildNum.ToString();
                break;

            case BuildTarget.iOS:
                buildNum = Int32.Parse(PlayerSettings.iOS.buildNumber);
                newBuildNum = buildNum + 1;
                PlayerSettings.iOS.buildNumber = newBuildNum.ToString();
                break;

            default:
                EditorUtility.DisplayDialog("Build Number ERROR!",
                    "Build Number Increment ERROR! - Wrong build target",
                    "Ok");
                break;
        }

    }

    private static string[] GetScenes()
    {
        return (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();
    }
}
