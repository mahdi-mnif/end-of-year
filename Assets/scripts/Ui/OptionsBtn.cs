using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsBtn : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject Canvas2;
    public GameObject Decor;
    public void ShowOptions()
    {
        Canvas.SetActive(false);
        Canvas2.SetActive(true);
        Decor.SetActive(false);
    }
    public void HideOptions() 
    {
        Canvas.SetActive(true);
        Canvas2.SetActive(false);
        Decor.SetActive(true);
    }

}
