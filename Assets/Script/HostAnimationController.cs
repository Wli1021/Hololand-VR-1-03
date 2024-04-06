using UnityEngine;

public class HostAnimationController : MonoBehaviour
{
    private Animator animator;
    private float idleTime = 1f; // Time in seconds to idle before starting gestures

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        // Start with the host idling
        animator.SetBool("isIdle", true);
        // Invoke the method to start gestures after the idleTime has passed
        Invoke("StartGestures", idleTime);
    }

    void StartGestures()
    {
        // Stop idling and start the first gesture
        animator.SetBool("isIdle", false);
        animator.SetTrigger("wave"); // Assuming you have a trigger to start the first gesture
    }
}
