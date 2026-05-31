using DoorScript;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CameraDoorScript
{
    public class CameraOpenDoor : MonoBehaviour
    {
        public float DistanceOpen = 3f;
        public GameObject text;
        public TextMeshProUGUI textUI;

        void Update()
        {
            GameObject held = PlayerHand.currentHeldObject;

            // Respect Burn UI - Do NOT clear if Burn is active
            if (UiDynamics.actionText == "Burn")
                return;

            // Block door UI when holding Ice or Batteries
            if (held != null)
            {
                PickUp pick = held.GetComponent<PickUp>();
                if (pick != null && (pick.CompareTag("Key_Ice") || pick.CompareTag("Key_Batteries")))
                {
                    text.SetActive(false);
                    return;
                }
            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, DistanceOpen))
            {
                // Give absolute priority to cobweb
                BurnableCobweb cobweb = hit.collider.GetComponent<BurnableCobweb>();
                if (cobweb != null)
                {
                    return; // Let BurnableCobweb control the UI
                }

                // Normal doors / safe
                var normalDoor = hit.transform.GetComponentInParent<Door>();
                var lockedDoor = hit.transform.GetComponentInParent<LockedDoor>();
                var microwaveDoor = hit.transform.GetComponentInParent<MicrowaveDoor>();
                var safePIN = hit.transform.GetComponentInParent<SafePINSystem>();

                if (normalDoor != null || lockedDoor != null || microwaveDoor != null || safePIN != null)
                {
                    text.SetActive(true);

                    if (safePIN != null && !safePIN.IsUnlocked)
                        textUI.text = "[E] Enter Code";
                    else if (lockedDoor != null && lockedDoor.isLocked)
                        textUI.text = "[E] Unlock Door";
                    else if (normalDoor != null && normalDoor.open ||
                             lockedDoor != null && lockedDoor.open ||
                             microwaveDoor != null && microwaveDoor.open)
                        textUI.text = "[E] Close Door";
                    else
                        textUI.text = "[E] Open Door";

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (safePIN != null) safePIN.TryOpenSafe();
                        else if (lockedDoor != null) lockedDoor.TryInteract();
                        else if (normalDoor != null) normalDoor.OpenDoor();
                        else if (microwaveDoor != null) microwaveDoor.OpenDoor();
                    }
                }
                else
                {
                    text.SetActive(false);
                }
            }
            else
            {
                text.SetActive(false);
            }
        }
    }
}