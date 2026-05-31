using UnityEngine;
using System.Collections;
using DoorScript;

public class MicrowaveController : MonoBehaviour
{
    [Header("References")]
    public MicrowaveDoor microwaveDoor;
    public GameObject onOffButton;
    public GameObject keyObject;
    public GameObject keyStand;
    public AudioClip microwaveHumSound;
    public AudioClip insertBatteriesSound;
    public AudioSource audioSource;

    [Header("Settings")]
    public float cookingTime = 15f;
    public float postCookDelay = 2f;

    private bool hasIceInside = false;
    private bool hasBatteries = false;
    private bool isRunning = false;
    private bool isLocked = false;

    private GameObject currentIceInMicrowave = null;
    private Collider microwaveCollider;

    void Start()
    {
        microwaveCollider = GetComponent<Collider>();

        if (keyObject != null) keyObject.SetActive(false);
        if (keyStand != null) keyStand.SetActive(false);
    }

    void Update()
    {
        if (isRunning || isLocked)
        {
            UiDynamics.uiActive = false;
            return;
        }

        RaycastHit hit;
        if (!Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 4f))
        {
            UiDynamics.uiActive = false;
            return;
        }

        GameObject held = PlayerHand.currentHeldObject;
        GameObject hitObject = hit.collider.gameObject;
        bool lookingAtMicrowave = IsPartOfMicrowave(hitObject);

        // === PUT ICE ===
        if (!hasIceInside && microwaveDoor != null && microwaveDoor.open && held != null && held.CompareTag("Key_Ice") && lookingAtMicrowave)
        {
            UiDynamics.actionText = "Put Inside";
            UiDynamics.uiActive = true;

            if (Input.GetKeyDown(KeyCode.E))
                PutIceInside(held);
            return;
        }

        // === INSERT BATTERIES ===
        if (hasIceInside && !hasBatteries && held != null && held.CompareTag("Key_Batteries") && lookingAtMicrowave)
        {
            UiDynamics.actionText = "Insert Batteries";
            UiDynamics.uiActive = true;

            if (Input.GetKeyDown(KeyCode.E))
                InsertBatteries(held);
            return;
        }

        // === TURN ON (Fixed - works even when not holding anything) ===
        if (hasIceInside && hasBatteries && !microwaveDoor.open && hitObject == onOffButton)
        {
            UiDynamics.actionText = "Turn On";
            UiDynamics.uiActive = true;

            if (Input.GetKeyDown(KeyCode.E))
                StartCooking();
            return;
        }

        UiDynamics.uiActive = false;
    }

    private bool IsPartOfMicrowave(GameObject obj)
    {
        if (obj == null) return false;
        if (obj == gameObject || obj == onOffButton) return true;
        if (microwaveDoor != null && obj == microwaveDoor.gameObject) return true;

        Transform t = obj.transform;
        while (t != null)
        {
            if (t.gameObject == gameObject) return true;
            t = t.parent;
        }
        return false;
    }

    void PutIceInside(GameObject ice)
    {
        PickUp pick = ice.GetComponent<PickUp>();
        if (pick != null) pick.DropObject();

        ice.transform.position = new Vector3(-18.04211f, 0.943f, -55.77576f);
        ice.transform.rotation = Quaternion.identity;

        Rigidbody rb = ice.GetComponent<Rigidbody>();
        if (rb != null) { rb.isKinematic = true; rb.useGravity = false; }

        Collider col = ice.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        currentIceInMicrowave = ice;
        hasIceInside = true;
        UiDynamics.uiActive = false;
    }

    void InsertBatteries(GameObject batteries)
    {
        PickUp pick = batteries.GetComponent<PickUp>();
        if (pick != null) pick.DropObject();

        batteries.SetActive(false);
        if (insertBatteriesSound && audioSource) audioSource.PlayOneShot(insertBatteriesSound);

        hasBatteries = true;
        UiDynamics.uiActive = false;
    }

    void StartCooking()
    {
        if (!hasIceInside || !hasBatteries || microwaveDoor.open) return;

        isRunning = true;
        isLocked = true;
        if (microwaveDoor != null) microwaveDoor.enabled = false;

        if (microwaveHumSound && audioSource)
        {
            audioSource.clip = microwaveHumSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        StartCoroutine(CookingCoroutine());
    }

    IEnumerator CookingCoroutine()
    {
        yield return new WaitForSeconds(cookingTime);

        if (audioSource) audioSource.Stop();

        if (currentIceInMicrowave != null)
            currentIceInMicrowave.SetActive(false);

        if (microwaveCollider != null)
            microwaveCollider.enabled = false;

        if (keyStand != null)
        {
            keyStand.SetActive(true);
            keyStand.transform.position = new Vector3(-18.0382f, 0.8734f, -55.78358f);
        }

        if (keyObject != null)
        {
            keyObject.SetActive(true);
            keyObject.transform.position = new Vector3(-18.04211f, 0.924f, -55.8172f);
        }

        yield return new WaitForSeconds(postCookDelay);

        isRunning = false;
        isLocked = false;
        if (microwaveDoor != null) microwaveDoor.enabled = true;
    }
}