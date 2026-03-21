using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public static string actionText;
    [SerializeField] bool canPick;
    [SerializeField] GameObject tableKey;
    [SerializeField] GameObject handKey;


    // Update is called once per frame
    void Update()
    {
        if (canPick == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                this.GetComponent<BoxCollider>().enabled = false;
                tableKey.SetActive(false);
                handKey.SetActive(true);

            }
        }
    }
    void OnMouseOver()
    {
        if (PlayerCasting.distanceFromTarget < 5)
        {
            canPick = true;
            UiDynamics.uiActive = true;
            UiDynamics.actionText = "Pick Up";
        }
        
        else
        {
            canPick = false;
            UiDynamics.uiActive = false;
            UiDynamics.actionText = "";
        }
    }
     void OnMouseExit()
    {
        canPick = false;
        UiDynamics.uiActive = false;
        UiDynamics.actionText = "";
    }
}
