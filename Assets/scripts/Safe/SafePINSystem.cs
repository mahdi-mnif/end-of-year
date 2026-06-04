using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace DoorScript
{
    public class SafePINSystem : MonoBehaviour
    {
        [Header("PIN Settings")]
        public string correctCode = "849867";
        public int codeLength = 6;

        [Header("UI References")]
        public GameObject pinPanel;
        public TMP_InputField inputField;
        public TextMeshProUGUI feedbackText;
        public Button submitButton;
        public Button closeButton;

        [Header("Safe References")]
        public GameObject safeBody;
        public MicrowaveDoor safeDoorScript;

        private bool isUnlocked = false;
        public bool IsUnlocked => isUnlocked;

        void Start()
        {
            if (pinPanel != null) pinPanel.SetActive(false);

            if (submitButton != null)
                submitButton.onClick.AddListener(SubmitCode);

            if (closeButton != null)
                closeButton.onClick.AddListener(ClosePanel);

            if (feedbackText != null)
                feedbackText.text = "";
        }

        void Update()
        {
            if (pinPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePanel();
            }
        }

        public void TryOpenSafe()
        {
            if (isUnlocked)
            {
                if (safeDoorScript != null) safeDoorScript.OpenDoor();
                return;
            }

            if (pinPanel != null)
            {
                pinPanel.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                inputField.ActivateInputField();
                inputField.text = "";
                feedbackText.text = "";
            }
        }

        public void SubmitCode()
        {
            if (isUnlocked) return;

            string enteredCode = inputField.text.Trim();

            if (enteredCode.Length != codeLength)
            {
                ShowFeedback("luck won't get you this one buddy", Color.red);
                return;
            }

            if (enteredCode == correctCode)
            {
                ShowFeedback("fair enough", Color.green);

                // === DISABLE COLLIDER IMMEDIATELY WHEN CORRECT CODE IS ENTERED ===
                if (safeBody != null)
                {
                    Collider col = safeBody.GetComponent<Collider>();
                    if (col != null)
                    {
                        col.enabled = false;
                    }
                }

                isUnlocked = true;

                if (safeDoorScript != null)
                    safeDoorScript.OpenDoor();

                Invoke("ClosePanel", 1.8f);
            }
            else
            {
                ShowFeedback("luck won't get you this one buddy", Color.red);
            }
        }

        void ShowFeedback(string message, Color color)
        {
            if (feedbackText != null)
            {
                feedbackText.text = message;
                feedbackText.color = color;
            }
        }

        public void ClosePanel()
        {
            if (pinPanel != null)
                pinPanel.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (feedbackText != null)
                feedbackText.text = "";
        }
    }
}