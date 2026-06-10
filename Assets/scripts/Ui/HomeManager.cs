
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string playSceneName;

    public void PlayGame()
    {

        CleanSceneSwitch("KEY", false, CursorLockMode.Locked);
    }

    public void GoBackToLobby()
    {

        CleanSceneSwitch("UI Lobby", true, CursorLockMode.Locked);
    }

    private void CleanSceneSwitch(string sceneName, bool showCursor, CursorLockMode lockMode)
    {
        // 1. Reset time scale in case the game scene was paused
        Time.timeScale = 1f;

        // 2. Clear out unused assets and garbage memory from the previous scene
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        // 3. Update cursor settings safely
        Cursor.visible = showCursor;
        Cursor.lockState = lockMode;

        // 4. Load the scene normally (Single mode completely unloads the current scene)
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}