using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiDynamics : MonoBehaviour
{
    public static string actionText;
    public static bool uiActive;
    [SerializeField] GameObject actionBox;
    [SerializeField] GameObject interactPoint;
    // Update is called once per frame
    void Update()
    {
        if (uiActive == true)
        {
            actionBox.SetActive(true);
            interactPoint.SetActive(true);
            actionBox.GetComponent<TMPro.TMP_Text>().text = "[E] " + actionText;

        }
        else
        {
            actionBox.SetActive(false);
            interactPoint.SetActive(false);
        }
    }
}
