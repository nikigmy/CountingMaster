using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BuildScripts : MonoBehaviour
{
    [MenuItem("Build/Build Mac")]
    public static void BuildMac()
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
    public static void BuildDesktop()
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

    [MenuItem("Build/Build iOS (release)")]
    private static void Build_iOS_Release()
    {
        IncrementBuildNumber(BuildTarget.iOS);
        Build_iOS(BuildOptions.None);
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
