using System;
using UnityEditor;
using UnityEngine;

public class InputAccessTokenDialog : EditorWindow
{
    private string variableNeeded;
    private string targetRepo;
    private string currentText;
    private Action<string> onConfirm; 
    static InputAccessTokenDialog Init(string targetRepo, string variableNeeded)
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
        EditorGUILayout.LabelField($"Missing environment variable {variableNeeded} for repository {targetRepo}", EditorStyles.wordWrappedLabel);
        GUILayout.Space(70);
        currentText = GUILayout.TextField(currentText);
        if (GUILayout.Button("Confirm"))
        {
            onConfirm?.Invoke(currentText);
            this.Close();
        }
    }
}