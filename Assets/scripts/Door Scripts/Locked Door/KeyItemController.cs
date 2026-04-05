using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KeySystem
{
    public class KeyItemController : MonoBehaviour
    {
        [SerializeField] private bool redDoor = false;
        [SerializeField] private bool redKey = false;

        [SerializeField] private KeyInvertory _keyInventory = null;

        private KeyDoorController doorObject;

        private void Start()
        {
            if (redDoor)
            {
                doorObject = GetComponent<KeyDoorController>();
            }
            
      
        }

        public void objectInerraction()
        {
            if (redDoor)
            {
                doorObject.PlayAnimation();
            }

            else if(redKey)
            {
                _keyInventory.hasRedkey = true;
                gameObject.SetActive(false);
            }
        }

    }
}



