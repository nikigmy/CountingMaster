using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "GithubPackageManager", menuName = "GithubPackageManager/CreateScriptableObject", order = 1)]
public class GithubPackageManager : ScriptableObject
{
    private static GithubPackageManager instance;
    
    public static GithubPackageManager Instance
    {
        get
        {
            if (instance == null)
            {
                var assetGuid = AssetDatabase.FindAssets("t:GithubPackageManager").FirstOrDefault();
                if (string.IsNullOrEmpty(assetGuid))
                {
                    Debug.LogError("Cant find GithubPackageManager asset, please create one");
                }
                else
                {
                    var path = AssetDatabase.GUIDToAssetPath(assetGuid);
                    instance = AssetDatabase.LoadAssetAtPath<GithubPackageManager>(path);
                    if (instance == null)
                    {
                        Debug.LogError("Could not load GithubPackageManager asset");
                    }
                }
            }
            return instance;
        }
    }

    [SerializeField] private List<RepositoryDependency> dependencies;    
    
    static string latestReleaseUrlFormat = "https://api.github.com/repos/{0}/{1}/releases/latest";
    static string targetReleaseUrlFormat = "https://api.github.com/repos/{0}/{1}/releases/{2}";
    static UnityWebRequest currentRequest;
    private bool isUpdating;

    void SendRequest(string url, Dictionary<string ,string> headers)
    {
        currentRequest = UnityWebRequest.Get(url);
        foreach (var header in headers)
        {
            currentRequest.SetRequestHeader(header.Key, header.Value);
        }
        currentRequest.Send();
    }

    [MenuItem ("Tools/GithubPackageManager/UpdateDependencies")]
    static void UpdateDependencies()
    {
        
        foreach (var repositoryDependency in Instance.dependencies)
        {
            Instance.HandleDependency(repositoryDependency);
        }
    }

    private void HandleDependency(RepositoryDependency repositoryDependency)
    {
        Log($"Begin Loading {repositoryDependency.RepositoryName} dependency");
        string accessToken = "";
        string releaseTag;
        if (!string.IsNullOrEmpty(repositoryDependency.AccessTokenEnvironmentVariable))
        {
            accessToken = Environment.GetEnvironmentVariable(repositoryDependency.AccessTokenEnvironmentVariable);
            if (string.IsNullOrEmpty(accessToken))
            {
                if (!Application.isBatchMode)
                {
                    var window = InputAccessTokenDialog.Init(repositoryDependency.RepositoryName,
                        repositoryDependency.AccessTokenEnvironmentVariable);
                    window.ShowModalUtility();
                    accessToken = window.currentText;
                }
                else
                {
                    Debug.LogWarning($"No access token found for dependency '{repositoryDependency.RepositoryName}' skipping");
                }
            }
        }
        
        //for private repos
        var headers = new Dictionary<string, string>();
        if (!string.IsNullOrEmpty(accessToken))
        {
            headers.Add("Authorization", "token " + accessToken);
        }

        //get release data
        string releaseData;
        if (repositoryDependency.ReleaseTag == "latest")
        {
            SendRequest(string.Format(latestReleaseUrlFormat, repositoryDependency.OwnerName, repositoryDependency.RepositoryName), headers);
            WaitUntilReady();
            releaseData = currentRequest.downloadHandler.text;
        }
        else
        {
            SendRequest(string.Format(targetReleaseUrlFormat, repositoryDependency.OwnerName, repositoryDependency.RepositoryName, repositoryDependency.ReleaseTag), headers);
            WaitUntilReady();
            releaseData = currentRequest.downloadHandler.text;
        }

        if (!string.IsNullOrEmpty(releaseData))
        {
            
            var releaseNamePattern = "(\"tag_name\":\\s+\".+\")[.\\w\\W]+?(\"name\":\\s+\".+\")(?=[.\\w\\W]+?\\],)";
            var assetsPattern = "(\"url\":\\s+\"http.+releases\\/assets.+\")[.\\w\\W]+?(\"name\":\\s+\".+\")(?=[.\\w\\W]+?\\],)";
            var releaseNameMatch = Regex.Match(releaseData,releaseNamePattern);
            var assetMatches = Regex.Matches(releaseData,assetsPattern);
            
            Log($"Found {assetMatches.Count} assets for Release '{releaseNameMatch.Groups[1].Value.Split('"')[1]}' with tag '{releaseNameMatch.Groups[0].Value.Split('"')[1]}'");
            headers.Add("Accept", "application/octet-stream");
            foreach (Match match in assetMatches)
            {
                var requestPath = match.Groups[0].Value.Split('"')[3];
                var fileName = match.Groups[1].Value.Split('"')[3];
                
                Log($"Loading asset {fileName}");
                
                SendRequest(requestPath, headers);
                WaitUntilReady();
                var directoryPath =  Path.Combine(Path.Combine(Directory.GetParent(Application.dataPath).FullName ,  "Assets"), repositoryDependency.TargetDirectory);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                File.WriteAllBytes(Path.Combine(directoryPath, fileName), currentRequest.downloadHandler.data);
                
                Log($"Loaded asset {fileName}");
            }
        }
        else
        {
            Debug.LogError($"Could not load dependency '{repositoryDependency.RepositoryName}'");
        }
    }

    private void Log(string log)
    {
        Debug.Log(log);
    }

    private void WaitUntilReady()
    {
        while (!currentRequest.isDone)
        {
        }
    }

    [Serializable]
    public class RepositoryDependency
    {
        public string RepositoryName;
        public string OwnerName;
        public string ReleaseTag;
        public string AccessTokenEnvironmentVariable;
        public string TargetDirectory;
    }
}