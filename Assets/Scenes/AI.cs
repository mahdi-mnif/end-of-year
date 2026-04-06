using UnityEngine;
using UnityEngine.AI;

public class NPCFollow : MonoBehaviour
{
    public Transform player;          // Assign player in Inspector
    public float stopDistance = 2f;   // Distance to stop from player

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);

            // Stop when close to player
            if (Vector3.Distance(transform.position, player.position) <= stopDistance)
            {
                agent.ResetPath(); // stop moving
            }
        }
    }
}