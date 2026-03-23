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

            if (hit.collider.CompareTag("InteractiveObject"))
                isInteractable = true;
            else
                isInteractable = false;
        }
        else
        {
            isInteractable = false;
        }
    }
}