using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NPCFollow : MonoBehaviour
{
    public Transform player;

    [Header("Chase Settings")]
    public float detectionDistance = 10f;
    public float stopDistance = 2f;

    [Header("Caught Settings")]
    public string caughtSceneName = "CaughtScene";

    [Header("Home Position")]
    public Vector3 homePosition = new Vector3(-0.75f, 0f, -66.91301f);

    [Header("Health")]
    public int hitsToDisable = 3;

    [Header("Key")]
    public GameObject keyObject;

    private int currentHits = 0;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip detectionSound;           // ← Drag your looping sound here

    private NavMeshAgent agent;
    private Transform currentDoor;
    private DoorScript.Door currentDoorScript;

    [Header("Animation")]
    public Animator animator;
    public string deathTrigger = "Die";
    public string attackTrigger = "collision";
    public float disableDelay = 2f;

    private bool hasCaughtPlayer = false;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // This will create the manager automatically if it doesn't exist
        DifficultyManager manager = DifficultyManager.Instance;

        detectionDistance = manager.CurrentDetectionDistance;
        stopDistance = manager.CurrentStopDistance;
        hitsToDisable = manager.CurrentHitsToDisable;

        Debug.Log($"NPC received difficulty → Detection: {detectionDistance}, Stop: {stopDistance}, Hits: {hitsToDisable}");
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // ====================== PLAYER DETECTION ======================
        if (distanceToPlayer <= detectionDistance)
        {
            // Play looping detection sound
            if (audioSource != null && detectionSound != null && !audioSource.isPlaying)
            {
                audioSource.clip = detectionSound;
                audioSource.loop = true;
                audioSource.Play();
            }

            if (distanceToPlayer > stopDistance)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                // NPC stopped → Load caught scene
                agent.ResetPath();

                if (!hasCaughtPlayer)
                {
                    hasCaughtPlayer = true;
                    Debug.Log("NPC caught the player! Loading scene: " + caughtSceneName);

                    if (!string.IsNullOrEmpty(caughtSceneName))
                    {
                        SceneManager.LoadScene(caughtSceneName);
                    }
                    else
                    {
                        Debug.LogWarning("Caught scene name is empty! Please set it in the Inspector.");
                    }
                }
            }
            return;
        }

        // ====================== PLAYER OUT OF RANGE → STOP SOUND ======================
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // ====================== GO TO DOOR ======================
        if (currentDoor != null)
        {
            agent.SetDestination(currentDoor.position);
            float distanceToDoor = Vector3.Distance(transform.position, currentDoor.position);

            if (distanceToDoor <= 1.5f)
            {
                if (currentDoorScript != null && currentDoorScript.open)
                {
                    currentDoorScript.OpenDoor();
                }
                currentDoor = null;
                currentDoorScript = null;
            }
        }
        else
        {
            // RETURN HOME
            agent.SetDestination(homePosition);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (animator != null)
            {
                animator.SetTrigger(attackTrigger);
            }
            gameObject.SetActive(false);
        }

        if (other.CompareTag("Bullet"))
        {
            currentHits++;
            Destroy(other.gameObject);

            if (currentHits >= hitsToDisable)
            {
                DisableNPC();
            }
        }
    }

    public void DoorOpened(DoorScript.Door door)
    {
        currentDoorScript = door;
        currentDoor = door.transform;
    }

    void DisableNPC()
    {
        if (animator != null)
        {
            animator.SetTrigger(deathTrigger);
        }
        Invoke(nameof(FinishDeath), disableDelay);
    }

    void FinishDeath()
    {
        if (keyObject != null)
        {
            keyObject.transform.position = transform.position;
            keyObject.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}