using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public static GameObject currentHeldObject; // static so Door can easily access it

    // Call this from your pickup script when you attach the key to the hand
    public void HoldItem(GameObject item)
    {
        currentHeldObject = item;
        // parent it to hand, set position/rotation, etc.
    }

    // Call this on drop
    public void DropItem()
    {
        currentHeldObject = null;
    }
}