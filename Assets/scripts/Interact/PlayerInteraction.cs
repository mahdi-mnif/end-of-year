using UnityEngine;
using DoorScript;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 5f;

    void Update()
    {
        GameObject held = PlayerHand.currentHeldObject;

        // Drop logic
        if (held != null &&  (VRInputManager.WasInteractPressed))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4f))
            {
                if (hit.collider.GetComponent<BurnableCobweb>() != null)
                    return;
            }

            PickUp p = held.GetComponent<PickUp>();
            if (p != null) p.DropObject();
        }

        // Normal pickup
        if (held == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange))
            {
                PickUp pick = hit.collider.GetComponent<PickUp>();
                if (pick != null && !pick.IsHolding)
                {
                    UiDynamics.actionText = "Pick Up";
                    UiDynamics.uiActive = true;

                    if (VRInputManager.WasInteractPressed)
                        pick.TryPickUp();
                }
            }
        }
    }
}