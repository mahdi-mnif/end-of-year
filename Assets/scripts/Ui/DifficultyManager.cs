using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    private static DifficultyManager _instance;

    public static DifficultyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Create it automatically if it doesn't exist
                GameObject go = new GameObject("DifficultyManager");
                _instance = go.AddComponent<DifficultyManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    [Header("Easy Difficulty")]
    public float easyDetectionDistance = 8f;
    public float easyStopDistance = 3f;
    public int easyHitsToDisable = 4;

    [Header("Normal Difficulty")]
    public float normalDetectionDistance = 12f;
    public float normalStopDistance = 2.5f;
    public int normalHitsToDisable = 3;

    [Header("Hard Difficulty")]
    public float hardDetectionDistance = 15f;
    public float hardStopDistance = 2f;
    public int hardHitsToDisable = 2;

    public float CurrentDetectionDistance { get; private set; }
    public float CurrentStopDistance { get; private set; }
    public int CurrentHitsToDisable { get; private set; }

    private int _currentDifficulty = 1;
    public int CurrentDifficulty => _currentDifficulty;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Load saved difficulty
            _currentDifficulty = PlayerPrefs.GetInt("GameDifficulty", 1);
            ApplyDifficulty(_currentDifficulty);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetDifficulty(int difficultyIndex)
    {
        _currentDifficulty = difficultyIndex;
        ApplyDifficulty(difficultyIndex);

        PlayerPrefs.SetInt("GameDifficulty", difficultyIndex);
        PlayerPrefs.Save();

        Debug.Log("Difficulty changed to index: " + difficultyIndex);
    }

    private void ApplyDifficulty(int index)
    {
        switch (index)
        {
            case 0: // Easy
                CurrentDetectionDistance = easyDetectionDistance;
                CurrentStopDistance = easyStopDistance;
                CurrentHitsToDisable = easyHitsToDisable;
                break;

            case 1: // Normal
                CurrentDetectionDistance = normalDetectionDistance;
                CurrentStopDistance = normalStopDistance;
                CurrentHitsToDisable = normalHitsToDisable;
                break;

            case 2: // Hard
                CurrentDetectionDistance = hardDetectionDistance;
                CurrentStopDistance = hardStopDistance;
                CurrentHitsToDisable = hardHitsToDisable;
                break;
        }
    }
}