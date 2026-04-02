using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    None,
    TitleScene,
    LobbyScene,
    GameScene,
}

public static class SceneLoader 
{
    public static void Load(SceneType scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
