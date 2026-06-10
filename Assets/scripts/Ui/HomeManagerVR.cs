using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class HomeManagerVR : MonoBehaviour
{

    [Header("VR Input (B Button)")]
    [SerializeField] private InputActionReference goBackAction; // Drag your B button action here

    [Header("Pause Menu Reference")]
    [SerializeField] private PauseMenu pauseMenu; // Drag your PauseMenu GameObject here

  
    public void GoBackToLobby()
    {
        CleanSceneSwitch("UI Lobby", true, CursorLockMode.None);
    }

    private void OnEnable()
    {
        if (goBackAction != null && goBackAction.action != null)
        {
            goBackAction.action.Enable();
            goBackAction.action.performed += OnGoBackPerformed;
        }
    }

    private void OnDisable()
    {
        if (goBackAction != null && goBackAction.action != null)
        {
            goBackAction.action.performed -= OnGoBackPerformed;
            goBackAction.action.Disable();
        }
    }

    private void OnGoBackPerformed(InputAction.CallbackContext context)
    {
        // Only allow going back to lobby if the game is paused
        if (pauseMenu != null && pauseMenu.GamePaused)
        {
            GoBackToLobby();
        }
    }

    // NEW Update method - checks B button only when paused
    private void Update()
    {
        if (pauseMenu != null && pauseMenu.GamePaused && goBackAction != null && goBackAction.action.WasPressedThisFrame())
        {
            GoBackToLobby();
        }
    }

    private void CleanSceneSwitch(string sceneName, bool showCursor, CursorLockMode lockMode)
    {
        Time.timeScale = 1f;
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        Cursor.visible = showCursor;
        Cursor.lockState = lockMode;

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}