using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class GithubPackageManager 
{
    
    // [MenuItem("Build/DependencyTest")]
    // private static void DependencyTest()
    // {
    //     HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://api.github.com/repos/nikigmy/LibratyTest/releases/latest");
    //     request.Method = "GET";
    //     
    //     // Content type is JSON.
    //     request.Headers = new WebHeaderCollection() { "Authorization: token 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"};
    //     
    //     try
    //     {
    //         using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
    //         {
    //             Debug.Log("Publish Response: " + (int)response.StatusCode + ", " + response.StatusDescription);
    //             if((int)response.StatusCode == 200)
    //             {
    //             }
    //         }
    //     }
    //     catch(Exception e)
    //     {
    //         Debug.LogError(e.ToString());
    //     }
    //     
    // }
    
    
    //
    // [MenuItem("Build/DependencyTest")]
    // private static void DependencyTest()
    // {
    //     try
    //     {
    //         UnityWebRequest webRequest =
    //             UnityWebRequest.Get("https://api.github.com/repos/nikigmy/LibratyTest/releases/latest");
    //         while (!webRequest.isDone)
    //         {
    //             Debug.LogError(webRequest.result);
    //         }
    //         Debug.LogError(webRequest.result);
    //         string[] pages = "https://api.github.com/repos/nikigmy/LibratyTest/releases/latest".Split('/');
    //         int page = pages.Length - 1;
    //
    //         switch (webRequest.result)
    //         {
    //             case UnityWebRequest.Result.ConnectionError:
    //             case UnityWebRequest.Result.DataProcessingError:
    //                 Debug.LogError(pages[page] + ": Error: " + webRequest.error);
    //                 break;
    //             case UnityWebRequest.Result.ProtocolError:
    //                 Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
    //                 break;
    //             case UnityWebRequest.Result.Success:
    //                 Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
    //                 break;
    //         }
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.LogError(e.ToString());
    //     }
    // }
    
    // Lets poke Maps for the distance between two cities.
    static string url = "https://api.github.com/repos/nikigmy/LibratyTest/releases/latest";
    static UnityWebRequest www;
 
    static void Request()
    {
        www = UnityWebRequest.Get(url);
        www.Send();
    }
 
    static void EditorUpdate()
    {
        if (!www.isDone)
            return;
        
        if (www.isNetworkError)
            Debug.Log(www.error);
        else
            Debug.Log(www.downloadHandler.text);
        
        EditorApplication.update -= EditorUpdate;
    }
 
    [MenuItem ("Tools/Poke Google Web Service")]
    static void UpdateFromSpreadsheet()
    {
        Request();
        EditorApplication.update += EditorUpdate;
    }
    // private bool isDone;
    // private IEnumerator SetJsonData()
    // {
    //     // Dictionary<string, string> headers = new Dictionary<string, string>();
    //     // headers.Add("Authorization", "token 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");
    //     //
    //     // WWW www = new WWW("https://api.github.com/repos/nikigmy/LibratyTest/releases/latest", null, headers);
    //     // www
    //     // while (www.isDone)
    //     // {
    //     //     
    //     // }
    //     // yield return www;
    //     //
    //     // Debug.Log(www.text);
    //     yield return null;
    // } 
}