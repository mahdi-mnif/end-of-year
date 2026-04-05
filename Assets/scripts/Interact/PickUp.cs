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
            if (PlayerCasting.isInteractable && PlayerCasting.distanceFromTarget <= pickUpRange)
            {
                canPick = true;
                UiDynamics.actionText = isHolding ? "Drop" : "Pick Up";
                UiDynamics.uiActive = true;
            }
            else
            {
                canPick = false;
                UiDynamics.uiActive = false;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isHolding)
                    DropObject();
                else if (canPick)
                    PickObject();
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