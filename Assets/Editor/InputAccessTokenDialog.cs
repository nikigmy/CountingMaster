using System;
using UnityEditor;
using UnityEngine;

public class InputAccessTokenDialog : EditorWindow
{
    private string variableNeeded;
    private string targetRepo;
    public string currentText;
    public bool isDone;
    
    public static InputAccessTokenDialog Init(string targetRepo, string variableNeeded)
    {
        InputAccessTokenDialog window = ScriptableObject.CreateInstance<InputAccessTokenDialog>();
        window.targetRepo = targetRepo;
        window.variableNeeded = variableNeeded;
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 150);
        window.ShowPopup();
        return window;
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField($"Missing environment variable '{variableNeeded}' for repository '{targetRepo}'", EditorStyles.wordWrappedLabel);
        GUILayout.Space(20);
        currentText = GUILayout.TextField(currentText);
        if (GUILayout.Button("Confirm"))
        {
            isDone = true;
            this.Close();
        }
    }

    private void OnDestroy()
    {
        isDone = true;
    }
}