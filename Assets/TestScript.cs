using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    
    [ContextMenu("DependencyTest")]
    private void DependencyTest()
    {
        StartCoroutine(SetJsonData());
    }

    private bool isDone;
    private IEnumerator SetJsonData()
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Authorization", "token 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");

        WWW www = new WWW("https://api.github.com/repos/nikigmy/LibratyTest/releases/latest", null, headers);
        yield return www;

        Debug.Log(www.text);
    }    
}