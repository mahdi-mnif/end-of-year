using UnityEngine;

namespace DoorScript
{
    public class PickUp : MonoBehaviour
    {
        [SerializeField] Transform holdPoint;

        private Rigidbody rb;
        private bool isHolding = false;

        public bool isKey = false;
        public bool isCandle = false;

        public bool IsHolding => isHolding;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void TryPickUp()
        {
            if (isHolding) return;

            isHolding = true;
            rb.isKinematic = true;
            rb.useGravity = false;

            transform.SetParent(holdPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            if (isCandle)
            {
                transform.localPosition = new Vector3(0.25f, -0.2f, 0.45f);
                transform.localRotation = Quaternion.Euler(-35f, 40f, 10f);
            }

            GetComponent<BoxCollider>().enabled = false;
            PlayerHand.currentHeldObject = gameObject;
        }

        public void DropObject()
        {
            isHolding = false;
            PlayerHand.currentHeldObject = null;

            transform.SetParent(null);
            rb.isKinematic = false;
            rb.useGravity = true;
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}