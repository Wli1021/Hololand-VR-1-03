using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonVisibilityController : MonoBehaviour
{
    
    public GameObject quitButton;
    private static readonly int SquareSceneIndex = 1; // Define the Square Scene index
    private static readonly int StreetSceneIndex = 2; // Define the Street Scene index
    private static readonly int VotingSceneIndex = 3; // Define the Voting Scene index

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if we are loading the Square Scene
        if (scene.buildIndex == SquareSceneIndex)
        {
            // Check if we came from Scene 2 or 3
            if (SceneTransitionManager.PreviousSceneIndex == StreetSceneIndex || SceneTransitionManager.PreviousSceneIndex == VotingSceneIndex)
            {
                // Wait and show buttons
                StartCoroutine(ShowButtonAfterDelay(50f)); // Adjust delay as necessary
            }
            else
            {
                // Optionally immediately hide buttons if coming from any other scene
               
                quitButton.SetActive(false);
            }
        }
    }

    IEnumerator ShowButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        quitButton.SetActive(true);
    }
}
