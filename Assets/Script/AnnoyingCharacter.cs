using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AnnoyingCharacter : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isMoving = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.isStopped = true; // Initially stop the NavMeshAgent
    }

    void Update()
    {
        if (isMoving && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
        {
            Debug.Log("Reached waypoint or stopped: Going to next waypoint or stopping.");
            if (waypoints.Length > 0)
            {
                GoToNextWaypoint();
            }
            else
            {
                // No waypoints or finished the loop, stop moving
                DeactivateMovement();
            }
        }

        // Update the walking animation based on the agent's movement state
        bool isWalking = agent.velocity.sqrMagnitude > 0.1f;
        animator.SetBool("isWalking", isWalking);
        if (isWalking)
        {
            Debug.Log("Character is walking.");
        }
        else
        {
            Debug.Log("Character is idle.");
        }
    }

    void GoToNextWaypoint()
    {
        if (!isMoving || waypoints.Length == 0) return;

        agent.isStopped = false; // Make sure the NavMeshAgent is enabled
        agent.SetDestination(waypoints[currentWaypointIndex].position);
        Debug.Log($"Moving to Waypoint {currentWaypointIndex}");
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    public void ActivateMovement()
    {
        Debug.Log("Activating movement.");
        isMoving = true;
        agent.isStopped = false;
        GoToNextWaypoint(); // Start moving towards the first waypoint
    }

    public void DeactivateMovement()
    {
        Debug.Log("Deactivating movement.");
        isMoving = false;
        agent.isStopped = true; // Stop the NavMeshAgent from moving
        animator.SetBool("isWalking", false); // Set the animation to idle
    }
}
