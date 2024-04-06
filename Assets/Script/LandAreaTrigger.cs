using UnityEngine;

public class LandAreaTrigger : MonoBehaviour
{
    public CharacterAnimationControl characterAnimationControl; // Assign in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerSensor")) // Ensure your player GameObject has the "Player" tag
        {
            characterAnimationControl.EnterLand();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerSensor"))
        {
            characterAnimationControl.ExitLand();
        }
    }
}
