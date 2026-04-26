using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCasting : MonoBehaviour
{
    public static float distanceFromTarget;
    public static bool isInteractable;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            distanceFromTarget = hit.distance;

            // Changed: now it can pick up "PickableObject" OR any "Key_..." tag
            if (hit.collider.CompareTag("PickableObject") ||
                hit.collider.tag.StartsWith("Key_") ||
                hit.collider.CompareTag("Cobweb"))
            {
                isInteractable = true;
            }
            else
            {
                isInteractable = false;
            }
        }
        else
        {
            isInteractable = false;
        }
    }
}