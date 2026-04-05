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
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, DistanceOpen))
            {
                // Support both normal Door and LockedDoor
                var normalDoor = hit.transform.GetComponent<Door>();
                var lockedDoor = hit.transform.GetComponent<LockedDoor>();

                if (normalDoor != null || lockedDoor != null)
                {
                    text.SetActive(true);

                    // Determine what text to show
                    if (lockedDoor != null && lockedDoor.isLocked)
                    {
                        textUI.text = "[E] Unlock Door";
                    }
                    else if (normalDoor != null && normalDoor.open ||
                             lockedDoor != null && lockedDoor.open)
                    {
                        textUI.text = "[E] Close Door";
                    }
                    else
                    {
                        textUI.text = "[E] Open Door";
                    }

                    // Handle input
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (lockedDoor != null)
                        {
                            lockedDoor.TryInteract();      // Locked door uses TryInteract
                        }
                        else if (normalDoor != null)
                        {
                            normalDoor.OpenDoor();         // Normal door uses old OpenDoor
                        }
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