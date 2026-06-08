using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private Vector3 closedRotation = Vector3.zero;
    [SerializeField] private Vector3 openRotation = new Vector3(0f, 90f, 0f);
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Input")]
    [SerializeField] private InputActionReference toggleAction;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    private bool isOpen;
    private Quaternion targetRotation;

    private void OnEnable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.started += OnTogglePressed;
            toggleAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.started -= OnTogglePressed;
            toggleAction.action.Disable();
        }
    }

    private void Start()
    {
        targetRotation = Quaternion.Euler(closedRotation);
        transform.localRotation = targetRotation;
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    private void OnTogglePressed(InputAction.CallbackContext context)
    {
        ToggleDoor();
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        targetRotation = Quaternion.Euler(
            isOpen ? openRotation : closedRotation
        );

        if (audioSource != null)
        {
            AudioClip clip = isOpen ? openClip : closeClip;

            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}