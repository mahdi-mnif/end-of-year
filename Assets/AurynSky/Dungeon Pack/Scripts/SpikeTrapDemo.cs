using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;   // ← Added for scene loading

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

    [Header("Backup Detection")]
    public float detectionRadius = 1.2f;
    public float checkInterval = 0.1f;

    [Header("Death Settings")]
    public string deathSceneName = "DeathScene";   // ← Set your scene name here in the Inspector

    private bool isActive = false;
    private Coroutine currentCycle;

    private void Awake()
    {
        if (spikeTrapAnim == null)
            spikeTrapAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(BackupDetection());
    }

    private void OnTriggerEnter(Collider other)
    {
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
                    TriggerTrap();
                    KillPlayer(col.gameObject);
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
        Debug.Log("Player killed by spikes! Loading scene: " + deathSceneName);

        // Load the scene you chose in the Inspector
        if (!string.IsNullOrEmpty(deathSceneName))
        {
            SceneManager.LoadScene(deathSceneName);
        }
        else
        {
            Debug.LogWarning("Death scene name is empty! Please set it in the Inspector.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KillPlayer(collision.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}