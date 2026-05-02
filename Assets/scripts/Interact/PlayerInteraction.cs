using UnityEngine;
using DoorScript;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 5f;

    void Update()
    {
        if (PlayerHand.currentHeldObject != null)
        {
            // Let BurnableCobweb handle burning if possible
            // Only drop if E is pressed and we're NOT looking at a cobweb
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Check if we're looking at a cobweb first
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4f))
                {
                    BurnableCobweb cobweb = hit.collider.GetComponent<BurnableCobweb>();
                    if (cobweb != null)
                    {
                        // Do nothing here - let BurnableCobweb handle it
                        return;
                    }
                }

                // If not looking at cobweb → Drop
                PickUp held = PlayerHand.currentHeldObject.GetComponent<PickUp>();
                if (held != null) held.DropObject();
            }

            if (UiDynamics.actionText == "Pick Up")
                UiDynamics.uiActive = false;

            return;
        }

        // Normal pickup when not holding anything
        RaycastHit hit2;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit2, interactionRange))
        {
            PickUp pick = hit2.collider.GetComponent<PickUp>();

            if (pick != null && !pick.IsHolding)
            {
                UiDynamics.actionText = "Pick Up";
                UiDynamics.uiActive = true;

                if (Input.GetKeyDown(KeyCode.E))
                    pick.TryPickUp();

                return;
            }
        }

        if (UiDynamics.actionText == "Pick Up")
            UiDynamics.uiActive = false;
    }
}