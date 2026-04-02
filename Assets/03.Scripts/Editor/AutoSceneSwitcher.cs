#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class AutoSceneSwitcher
{
    const int START_SCENE_INDEX = 0;
    const string MENU_PATH = "Tools/Always Start From The First Scene";
    const string EXITED_SCENE_SAVE_KEY = "Exited_Scene_Path";

    static bool IsUse
    {
        get => Menu.GetChecked(MENU_PATH);
        set => Menu.SetChecked(MENU_PATH, value);
    }

    static AutoSceneSwitcher()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    [MenuItem(MENU_PATH)]
    static void Toggle() => IsUse = !IsUse;

    [MenuItem(MENU_PATH, true)]
    static bool Validate()
    {
        Menu.SetChecked(MENU_PATH, IsUse);
        return true;
    }

    static void OnPlayModeChanged(PlayModeStateChange state)
    {
        if (!IsUse)
            return;

        if (state == PlayModeStateChange.ExitingEditMode)
        {
            var StartScenePath = SceneUtility.GetScenePathByBuildIndex(START_SCENE_INDEX);
            var exitingScenePath = EditorSceneManager.GetActiveScene().path;

            EditorPrefs.SetString(EXITED_SCENE_SAVE_KEY, exitingScenePath);
            
            if (exitingScenePath != StartScenePath)
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    EditorSceneManager.OpenScene(StartScenePath);
                else
                    EditorApplication.isPlaying = false;
            }
        }
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            var exitedScenePath = EditorPrefs.GetString(EXITED_SCENE_SAVE_KEY);

            EditorSceneManager.OpenScene(exitedScenePath);
        }
    }
}
#endif