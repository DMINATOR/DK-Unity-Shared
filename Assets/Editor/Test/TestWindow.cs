using UnityEditor;
using UnityEngine;

public class SharedTestWindow : EditorWindow
{
    // Add menu item named "My Window" to the Window menu
    [MenuItem("Project/Test/Shared Project")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(SharedTestWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Shared Project", EditorStyles.boldLabel);

        if (GUILayout.Button("Test"))
        {
            Debug.Log("Shared Project Editor loaded correctly!");
        }
    }
}
