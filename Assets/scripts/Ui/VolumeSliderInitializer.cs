using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderSync : MonoBehaviour
{
    public Slider volumeSlider;

    // This runs automatically every time the Pause Menu becomes active
    private void OnEnable()
    {
        if (VolumeManager.Instance != null && volumeSlider != null)
        {
            volumeSlider.value = VolumeManager.Instance.masterVolume;
        }
    }
}