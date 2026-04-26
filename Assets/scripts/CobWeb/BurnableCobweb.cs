using UnityEngine;
using System.Collections;

public class BurnableCobweb : MonoBehaviour
{
    [SerializeField] float burnRange = 3f;
    [SerializeField] float burnDuration = 2.5f;
    [SerializeField] GameObject flameEffectPrefab;

    void Update()
    {
        if (PlayerCasting.isInteractable && PlayerCasting.distanceFromTarget <= burnRange)
        {
            if (IsLookingAtMe())
            {
                if (PlayerHand.currentHeldObject != null && PlayerHand.currentHeldObject.name.Contains("Candle"))
                {
                    UiDynamics.actionText = "Burn";
                    UiDynamics.uiActive = true; // This should now stay true

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        StartCoroutine(BurnSequence());
                    }
                }
            }
        }
    }

    bool IsLookingAtMe()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, burnRange))
        {
            return hit.collider.gameObject == gameObject;
        }
        return false;
    }

    IEnumerator BurnSequence()
    {
        // Disable the collider immediately so you can't click it twice
        GetComponent<Collider>().enabled = false;

        if (flameEffectPrefab != null)
        {
            GameObject fire = Instantiate(flameEffectPrefab, transform.position, Quaternion.identity);

            var ps = fire.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var shape = ps.shape;
                shape.shapeType = ParticleSystemShapeType.Box;
                shape.scale = transform.localScale;
            }
            Destroy(fire, burnDuration + 1f);
        }

        float elapsed = 0;
        Vector3 startScale = transform.localScale;

        while (elapsed < burnDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, elapsed / burnDuration);
            yield return null;
        }

        Destroy(gameObject);
        // Reset UI after destruction
        UiDynamics.uiActive = false;
    }
}
