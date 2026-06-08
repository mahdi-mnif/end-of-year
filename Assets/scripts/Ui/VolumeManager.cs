using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    private static VolumeManager _instance;

    public static VolumeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("VolumeManager");
                _instance = go.AddComponent<VolumeManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public float masterVolume = 1f;
    public Slider volumeSlider;   // Optional: assign in scenes that have a slider

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolume();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMasterVolume()
    {
        // This version doesn't take any parameter ? more reliable
        if (volumeSlider != null)
        {
            masterVolume = volumeSlider.value;
            AudioListener.volume = masterVolume;
            SaveVolume();
        }
    }

    private void LoadVolume()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = masterVolume;
    }

    private void SaveVolume()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
    }
}