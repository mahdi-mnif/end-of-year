using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("References")]
    public Animator spikeTrapAnim;
    public string openTrigger = "open";
    public string closeTrigger = "close";

    [Header("Timing")]
    public float spikeUpDuration = 2f;
    public float spikeDownDuration = 2f;

    [Header("Collision Settings")]
    public LayerMask triggerLayers;
    public LayerMask deadlyLayers;

    [Header("Backup Detection (Recommended for CharacterController)")]
    public float detectionRadius = 1.2f;   // Adjust based on your trap size
    public float checkInterval = 0.1f;

    private bool isActive = false;
    private Coroutine currentCycle;

    private void Awake()
    {
        if (spikeTrapAnim == null)
            spikeTrapAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        // Start backup detection (works even if OnTriggerEnter fails)
        StartCoroutine(BackupDetection());
    }

    // ====================== MAIN TRIGGER (OnTriggerEnter) ======================
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[SpikeTrap] OnTriggerEnter called by: {other.name} | Layer: {LayerMask.LayerToName(other.gameObject.layer)}");

        if (isActive) return;

        if (((1 << other.gameObject.layer) & triggerLayers) != 0)
        {
            TriggerTrap();
        }

        if (((1 << other.gameObject.layer) & deadlyLayers) != 0)
        {
            if (other.CompareTag("Player"))
            {
                KillPlayer(other.gameObject);
            }
        }
    }

    // ====================== BACKUP DETECTION (Very Reliable) ======================
    private IEnumerator BackupDetection()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (isActive) continue;

            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, triggerLayers);

            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    Debug.Log("[SpikeTrap] Backup detection triggered by Player!");
                    TriggerTrap();

                    if (((1 << col.gameObject.layer) & deadlyLayers) != 0)
                    {
                        KillPlayer(col.gameObject);
                    }
                    break;
                }
            }
        }
    }

    public void TriggerTrap()
    {
        if (isActive) return;

        isActive = true;

        if (currentCycle != null) StopCoroutine(currentCycle);
        currentCycle = StartCoroutine(SpikeCycle());
    }

    private IEnumerator SpikeCycle()
    {
        spikeTrapAnim.SetTrigger(openTrigger);
        yield return new WaitForSeconds(spikeUpDuration);

        spikeTrapAnim.SetTrigger(closeTrigger);
        yield return new WaitForSeconds(spikeDownDuration);

        isActive = false;
    }

    private void KillPlayer(GameObject player)
    {
        Debug.Log("Player killed by spikes!");
        Destroy(player);
    }

    // Fallback for non-trigger colliders
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KillPlayer(collision.gameObject);
        }
    }

    // Draw detection radius in Scene view (for easy adjustment)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
