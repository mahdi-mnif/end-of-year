using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] float pickUpRange = 5f;
    [SerializeField] Transform holdPoint;

    private Rigidbody rb;
    private bool isHolding = false;
    private bool canPick = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Detect if player is looking at object (using your raycast system)
        if (PlayerCasting.isInteractable && PlayerCasting.distanceFromTarget <= pickUpRange)
        {
            canPick = true;

            if (!isHolding)
                UiDynamics.actionText = "Pick Up";
            else
                UiDynamics.actionText = "Drop";

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
            {
                DropObject(); // always allow dropping
            }
            else if (canPick)
            {
                PickObject(); // only pick if allowed
            }
        }
    }

    void PickObject()
    {
        isHolding = true;

        // Disable physics
        rb.isKinematic = true;
        rb.useGravity = false;

        // Attach to hand
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    void DropObject()
    {
        isHolding = false;

        // Detach
        transform.SetParent(null);

        // Enable physics
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}