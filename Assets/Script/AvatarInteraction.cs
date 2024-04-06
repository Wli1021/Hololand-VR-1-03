using UnityEngine;

public class AvatarInteraction : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;

    private void Start()
    {
        // Automatically assign the Animator and AudioSource components if not already assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the entering collider is a VR controller or the invisible GameObject
        if (other.CompareTag("PlayerSensor")) // Ensure the controllers or sensor GameObject is tagged with "PlayerSensor"
        {
            Debug.Log("PlayerSensor has entered the trigger area.");
            animator.SetBool("isTalking", true); // Activate talking animation
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); 
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting collider is the VR controller or the invisible GameObject
        if (other.CompareTag("PlayerSensor"))
        {
            Debug.Log("PlayerSensor has exited the trigger area.");
            animator.SetBool("isTalking", false); // Return to idling animation
            audioSource.Stop(); // Stop the audio when the player leaves the trigger area
        }
    }
}

