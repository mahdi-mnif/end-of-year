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
    public LayerMask triggerLayers;           // Layers that activate the trap (Player + Interact)
    public LayerMask deadlyLayers;            // Layers that kill the player (usually just Player)

    private bool isActive = false;
    private Coroutine currentCycle;

    private void Awake()
    {
        if (spikeTrapAnim == null)
            spikeTrapAnim = GetComponent<Animator>();
    }

    // ====================== TRAP ACTIVATION ======================
    private void OnTriggerEnter(Collider other)
    {
        if (isActive) return;

        if (((1 << other.gameObject.layer) & triggerLayers) != 0)
        {
            TriggerTrap();
        }
        // This second OnTriggerEnter is intentional - Unity allows it
        if (((1 << other.gameObject.layer) & deadlyLayers) != 0)
        {
            if (other.CompareTag("Player"))
            {
                KillPlayer(other.gameObject);
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

        // Optional: disable the trap after killing player
        // isActive = false;
    }

    // Optional: Also support physical collision (if you turn Is Trigger OFF on spears)
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KillPlayer(collision.gameObject);
        }
    }
}