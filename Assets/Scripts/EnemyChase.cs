using System.Collections; // Make sure this is at the top for Coroutines
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    private Transform player;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Find the player
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }

        // Start the routine to wait for the enemy to hit the ground
        StartCoroutine(EnableAgentDelay());
    }

    IEnumerator EnableAgentDelay()
    {
        // Wait for a fraction of a second to let gravity drop the enemy to the floor
        yield return new WaitForSeconds(0.1f); 
        
        // Turn the agent on now that it is safely on the ground
        agent.enabled = true; 
    }

    void Update()
    {
        // Only set destination if the player exists, the agent is on, AND it is on the NavMesh
        if (player != null && agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }
    }
}