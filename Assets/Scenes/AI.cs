using UnityEngine;
using UnityEngine.AI;

public class NPCFollow : MonoBehaviour
{
    public Transform player;

    [Header("Chase Settings")]
    public float detectionDistance = 10f;
    public float stopDistance = 2f;

    [Header("Home Position")]
    public Vector3 homePosition = new Vector3(-0.75f, 0f, -66.91301f);

    [Header("Health")]
    public int hitsToDisable = 3;

    [Header("Key")]
    public GameObject keyObject;

    private int currentHits = 0;

    [Header("Audio")]
    public AudioSource audioSource;

    private NavMeshAgent agent;

    private Transform currentDoor;
    private DoorScript.Door currentDoorScript;

    [Header("Animation")]
    public Animator animator;
    public string deathTrigger = "Die";
    public string attackTrigger = "collision";
    public float disableDelay = 2f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer =
            Vector3.Distance(transform.position, player.position);

        // CHASE PLAYER
        if (distanceToPlayer <= detectionDistance)
        {
            if (distanceToPlayer > stopDistance)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                agent.ResetPath();
            }

            return;
        }

        // GO TO DOOR
        if (currentDoor != null)
        {
            agent.SetDestination(currentDoor.position);

            float distanceToDoor =
                Vector3.Distance(transform.position, currentDoor.position);

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

    // ✅ TRIGGER INSTEAD OF COLLISION
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