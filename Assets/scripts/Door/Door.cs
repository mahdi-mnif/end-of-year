using UnityEngine;

namespace DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Door : MonoBehaviour
    {
        public GameObject Frame;

        public bool open;
        public float smooth = 1.0f;

        float DoorOpenAngle = -90.0f;
        float DoorCloseAngle = 0.0f;

        public AudioSource asource;
        public AudioClip openDoor;
        public AudioClip closeDoor;

        public NPCFollow npc;

        void Start()
        {
            asource = GetComponent<AudioSource>();
        }

        void Update()
        {
            if (open)
            {
                Quaternion target =
                    Quaternion.Euler(0, DoorOpenAngle, 0);

                transform.localRotation =
                    Quaternion.Slerp(
                        transform.localRotation,
                        target,
                        Time.deltaTime * 5 * smooth);

                Frame.GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                Quaternion target =
                    Quaternion.Euler(0, DoorCloseAngle, 0);

                transform.localRotation =
                    Quaternion.Slerp(
                        transform.localRotation,
                        target,
                        Time.deltaTime * 5 * smooth);

                Frame.GetComponent<BoxCollider>().enabled = true;
            }
        }

        public void OpenDoor()
        {
            open = !open;

            asource.clip = open ? openDoor : closeDoor;
            asource.Play();

            if (open && npc != null)
            {
                npc.DoorOpened(this);
            }
        }
    }
}