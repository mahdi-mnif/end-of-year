using UnityEngine;
using UnityEngine.InputSystem;

public class Converter0trig : MonoBehaviour
{
    public static Converter0trig Instance;

    [Header("Input Action (Y button + E key)")]
    public InputActionReference interactAction;

    public static bool WasInteractPressed { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Enable the input action when this script is enabled
        if (interactAction != null && interactAction.action != null)
        {
            interactAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        // Disable the input action when this script is disabled
        if (interactAction != null && interactAction.action != null)
        {
            interactAction.action.Disable();
        }
    }

    private void Update()
    {
        WasInteractPressed = false;

        // Keyboard E
        if (Input.GetMouseButton(0)) 
        {
            WasInteractPressed = true;
        }

        // VR Controller (Y button)
        if (interactAction != null && interactAction.action != null)
        {
            if (interactAction.action.WasPressedThisFrame())
            {
                WasInteractPressed = true;
            }
        }
    }
}