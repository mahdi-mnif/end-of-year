using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiDynamics : MonoBehaviour
{
    public static string actionText;
    public static bool uiActive;

    [SerializeField] GameObject actionBox;
    [SerializeField] GameObject interactPoint;

    void Update()
    {
        //Debug.Log("UI State -> Text: " + actionText + " | Active: " + uiActive);

        if (uiActive)
        {
            actionBox.SetActive(true);
            interactPoint.SetActive(true);

            TMP_Text textComponent = actionBox.GetComponent<TMP_Text>();

            if (textComponent != null)
            {
                textComponent.text = "[Y] " + actionText;
            }
        }
        else
        {
            actionBox.SetActive(false);
            interactPoint.SetActive(false);
        }
    }
}