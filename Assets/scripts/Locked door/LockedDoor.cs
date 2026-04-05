using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class LockedDoor : MonoBehaviour   // ? We changed the class name to LockedDoor
    {
        public GameObject Frame;
        public bool open = false;
        public bool isLocked = true;
        public string requiredKeyTag = "Key_Door1";

        public float smooth = 1.0f;
        float DoorOpenAngle = -90.0f;
        float DoorCloseAngle = 0.0f;

        public AudioSource asource;
        public AudioClip openDoor, closeDoor, lockedSound;

        void Start()
        {
            asource = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (open)
            {
                var target = Quaternion.Euler(0, DoorOpenAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);
                if (Frame != null) Frame.GetComponent<MeshCollider>().enabled = false;
            }
            else
            {
                var target1 = Quaternion.Euler(0, DoorCloseAngle, 0);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, target1, Time.deltaTime * 5 * smooth);
                if (Frame != null) Frame.GetComponent<MeshCollider>().enabled = true;
            }
        }

        public void TryInteract()
        {
            if (isLocked)
            {
                if (HasRequiredKeyInHand())
                {
                    UnlockDoor();
                    ToggleDoor();
                }
                else
                {
                    if (lockedSound != null)
                    {
                        asource.clip = lockedSound;
                        asource.Play();
                    }
                    Debug.Log("Door is locked!");
                }
            }
            else
            {
                ToggleDoor();
            }
        }

        private void ToggleDoor()
        {
            open = !open;
            asource.clip = open ? openDoor : closeDoor;
            asource.Play();
        }

        private void UnlockDoor()
        {
            isLocked = false;
            Debug.Log("Door unlocked permanently!");
        }

        private bool HasRequiredKeyInHand()
        {
            if (PlayerHand.currentHeldObject == null) return false;
            return PlayerHand.currentHeldObject.CompareTag(requiredKeyTag);
        }
    }
}