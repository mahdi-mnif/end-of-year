using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HomeManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string mainSceneName = "KEY";        // ? Change to your exact main scene name
    public string lobbySceneName = "UI Lobby";  // ? Change to your exact lobby scene name

    public void PlayGame()
    {
        StartCoroutine(LoadSceneAsync(mainSceneName));
    }

    public void GoBackToLobby()
    {
        StartCoroutine(LoadSceneAsync(lobbySceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Set the new scene as the active scene (fixes dark lighting + many bugs)
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        // Cursor settings depending on which scene we loaded
        if (sceneName == mainSceneName)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (sceneName == lobbySceneName)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}