using UnityEngine;
using System.Collections;
using TMPro;
using DoorScript;

public class BurnableCobweb : MonoBehaviour
{
    [SerializeField] float burnRange = 3.5f;
    [SerializeField] GameObject flameEffectPrefab;
    [SerializeField] float burnDuration = 2.5f;

    [Header("Burn UI")]
    [SerializeField] TextMeshProUGUI burnText;

    [Header("Burn Sound")]
    [SerializeField] AudioClip burnSound;
    [SerializeField] AudioSource audioSource;

    private bool isBurning = false;

    void Start()
    {
        if (burnText != null)
            burnText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isBurning)
            return;

        if (burnText != null)
            burnText.gameObject.SetActive(false);

        if (PlayerHand.currentHeldObject == null)
            return;

        PickUp held = PlayerHand.currentHeldObject.GetComponent<PickUp>();

        if (held == null || !held.isCandle)
            return;

        RaycastHit hit;

        if (Physics.Raycast(
            Camera.main.transform.position,
            Camera.main.transform.forward,
            out hit,
            burnRange))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (burnText != null)
                {
                    burnText.gameObject.SetActive(true);
                    burnText.text = "[Y] Burn";
                }

                if (VRInputManager.WasInteractPressed)
                {
                    if (audioSource != null && burnSound != null)
                        audioSource.PlayOneShot(burnSound);

                    StartCoroutine(BurnCobweb());
                }

                return;
            }
        }
    }

    IEnumerator BurnCobweb()
    {
        isBurning = true;

        if (burnText != null)
            burnText.gameObject.SetActive(false);

        if (flameEffectPrefab != null)
        {
            GameObject flame = Instantiate(
                flameEffectPrefab,
                transform.position + Vector3.up * 0.5f,
                Quaternion.identity);

            Destroy(flame, burnDuration + 0.5f);
        }

        float elapsed = 0f;
        Vector3 startScale = transform.localScale;

        while (elapsed < burnDuration)
        {
            elapsed += Time.deltaTime;

            transform.localScale = Vector3.Lerp(
                startScale,
                Vector3.zero,
                elapsed / burnDuration);

            yield return null;
        }

        Destroy(gameObject);
    }
}