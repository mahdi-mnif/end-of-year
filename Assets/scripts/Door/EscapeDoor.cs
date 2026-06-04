using UnityEngine;

public class EscapeDoor : MonoBehaviour
{
    [Header("References")]
    public GameObject keyObject;      // The key the player picks up
    public GameObject exitObject;     // The object that starts DISABLED

    private bool exitActivated = false;
    private void Start()
    {
        exitObject.SetActive(false);
    }

    void Update()
    {
        if (exitActivated) return;

        bool keyPickedUp = false;

        // Check if player is holding the key
        if (keyObject != null && PlayerHand.currentHeldObject == keyObject)
            keyPickedUp = true;

        // Also support if key gets deactivated after pickup
        if (keyObject != null && !keyObject.activeInHierarchy)
            keyPickedUp = true;

        if (keyPickedUp && exitObject != null)
        {
            exitObject.SetActive(true);
            Debug.Log("Key picked up ? Exit object enabled!");
            exitActivated = true;
        }
    }
}