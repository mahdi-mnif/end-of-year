using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    // Name of the scene you want to load
    public string playSceneName;

    public void PlayGame()
    {
        SceneManager.LoadScene(playSceneName);
    }
}
