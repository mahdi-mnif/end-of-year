using UnityEngine;
using System.Collections;
using DoorScript;

public class BurnableCobweb : MonoBehaviour
{
    [SerializeField] float burnRange = 3.5f;
    [SerializeField] GameObject flameEffectPrefab;
    [SerializeField] float burnDuration = 2.5f;

    private bool isBurning = false;

    void Update()
    {
        if (isBurning || PlayerHand.currentHeldObject == null) return;

        PickUp held = PlayerHand.currentHeldObject.GetComponent<PickUp>();
        if (held == null || !held.isCandle) return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, burnRange))
        {
            if (hit.collider.gameObject == gameObject)
            {
                UiDynamics.actionText = "Burn";
                UiDynamics.uiActive = true;

                if (Input.GetKeyDown(KeyCode.E))
                    StartCoroutine(BurnCobweb());

                return;
            }
        }

        if (UiDynamics.actionText == "Burn")
            UiDynamics.uiActive = false;
    }

    IEnumerator BurnCobweb()
    {
        isBurning = true;
        UiDynamics.uiActive = false;

        if (flameEffectPrefab != null)
        {
            GameObject flame = Instantiate(flameEffectPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            Destroy(flame, burnDuration + 0.5f);
        }

        float elapsed = 0f;
        Vector3 startScale = transform.localScale;

        while (elapsed < burnDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, elapsed / burnDuration);
            yield return null;
        }

        Destroy(gameObject);
    }
}