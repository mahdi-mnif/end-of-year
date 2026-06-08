using UnityEngine;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class MicrowaveDoor : MonoBehaviour
    {
        

        [Header("Door Settings")]
        public bool open = false;
        public float smooth = 1.0f;

        [Header("Rotation Settings (Editable)")]
        public Vector3 openRotationAxis = new Vector3(0, 0, 90f);   // Change this per door
        public float openAngle = 90f;                               // Degrees to open

        private Quaternion closedRotation;
        private Quaternion openRotation;

        public AudioSource asource;
        public AudioClip openDoor, closeDoor;

        void Start()
        {

            closedRotation = transform.localRotation;
            openRotation = closedRotation * Quaternion.Euler(openRotationAxis.normalized * openAngle);
        }

        void Update()
        {
            Quaternion target = open ? openRotation : closedRotation;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * 5 * smooth);


        }

        public void OpenDoor()
        {
            open = !open;

            if (asource != null)
            {
                asource.clip = open ? openDoor : closeDoor;
                asource.Play();
            }
        }
    }
}