using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DoorScript
{
    public class PickUp : MonoBehaviour
    {
        [SerializeField] float pickUpRange = 5f;
        [SerializeField] Transform holdPoint;

        private Rigidbody rb;
        private bool isHolding = false;
        private bool canPick = false;

        public bool isKey = false;
        public int keyID;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            // NEW: Check if we are looking at a cobweb first
            bool lookingAtCobweb = false;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpRange))
            {
                if (hit.collider.CompareTag("Cobweb"))
                {
                    lookingAtCobweb = true;
                }
            }

            if (PlayerCasting.isInteractable && PlayerCasting.distanceFromTarget <= pickUpRange && !lookingAtCobweb)
            {
                canPick = true;
                UiDynamics.actionText = isHolding ? "Drop" : "Pick Up";
                UiDynamics.uiActive = true;
            }
            else if (!lookingAtCobweb) // Only hide UI if we aren't looking at a cobweb
            {
                canPick = false;
                UiDynamics.uiActive = false;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                // ONLY pick up or drop if we are NOT looking at a cobweb
                if (!lookingAtCobweb)
                {
                    if (isHolding)
                        DropObject();
                    else if (canPick)
                        PickObject();
                }
            }
        }

        void PickObject()
        {
            isHolding = true;
            rb.isKinematic = true;
            rb.useGravity = false;
            transform.SetParent(holdPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            GetComponent<BoxCollider>().enabled = false;

            // Tell the door system we are holding this key
            if (isKey)
            {
                PlayerHand.currentHeldObject = gameObject;
            }
        }

        void DropObject()
        {
            isHolding = false;

            if (isKey)
                PlayerHand.currentHeldObject = null;

            transform.SetParent(null);
            rb.isKinematic = false;
            rb.useGravity = true;
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}