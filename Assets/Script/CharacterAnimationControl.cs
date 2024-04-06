using UnityEngine;
using Dreamteck.Splines;

public class CharacterAnimationControl : MonoBehaviour
{
    private Animator animator;
    public SplineFollower splineFollower; // Assign in the Inspector

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", false);
        if (splineFollower != null)
        {
            // Optionally, you can set splineFollower's properties or events here
            // For example, to automatically start following the spline, but initially set to not move
            splineFollower.follow = false;
        }
    }

    // Call this method when the player enters the land area
    public void EnterLand()
    {
        splineFollower.follow = true; // Start the spline following
        animator.SetBool("isWalking", true); // Trigger walking animation
    }

    // Call this method when the player exits the land area
    public void ExitLand()
    {
        splineFollower.follow = false; // Stop the spline following
        animator.SetBool("isWalking", false); // Switch to idle animation
    }
}
