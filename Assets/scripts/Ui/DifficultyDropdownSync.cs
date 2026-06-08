using UnityEngine;
using TMPro;

public class DifficultyDropdownSync : MonoBehaviour
{
    public TMP_Dropdown difficultyDropdown;

    void Start()
    {
        if (DifficultyManager.Instance != null && difficultyDropdown != null)
        {
            difficultyDropdown.value = DifficultyManager.Instance.CurrentDifficulty;
        }
    }
}
