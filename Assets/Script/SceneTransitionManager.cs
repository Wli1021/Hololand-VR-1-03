using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SceneTransitionManager : MonoBehaviour
{
    public static bool DeactivateElementsInSquareScene = false;
    public static int PreviousSceneIndex = -1; // Initializes to -1 to indicate no previous scene
    public FadeScreen fadeScreen;
    public VideoPlayer videoPlayer;
    public VideoClip[] videoClips;
    private int currentVideoIndex = 0;

    //private static readonly int SquareSceneIndex = 1; // Define the Square Scene index

    public void GoToScene(int sceneIndex)
    {
        PreviousSceneIndex = SceneManager.GetActiveScene().buildIndex; // Update previous scene index
        if (sceneIndex == 1 && (PreviousSceneIndex == 2 || PreviousSceneIndex == 3)) // 2 and 3 are Street and Voting scenes
        {
            DeactivateElementsInSquareScene = true;
        }
        if (sceneIndex == 0)
        {
            // When going back to the opening scene, reset session data
            EndSessionAndReset();
        }


        StartCoroutine(GoToSceneRountine(sceneIndex));
    }

    // Resets the session data and prepares for a new game session
    private void EndSessionAndReset()
    {
        PlayerPrefs.SetInt("NewSession", 1);
        PlayerPrefs.Save();

        // Reset asset data
        if (AssetManager.instance != null)
        {
            AssetManager.instance.ResetDataToInitialValues();
            //AssetManager.instance.UpdateAssetUI();
            //GameManager.instance.UpdateAllCoinsUIText();

        }

        // Reset shop item states
        //if (Shop.instance != null)
        //{
            //Shop.instance.ResetPurchaseStates();
        //}

       
    }
    IEnumerator GoToSceneRountine(int sceneIndex)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration);

       
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoToSceneAsync(int sceneIndex)
    {
        PreviousSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 1 && (PreviousSceneIndex == 2 || PreviousSceneIndex == 3)) // 2 and 3 are Street and Voting scenes
        {
            DeactivateElementsInSquareScene = true;
        }

        StartCoroutine(GoToSceneAsyncRountine(sceneIndex));
    }
    IEnumerator GoToSceneAsyncRountine(int sceneIndex)
    {
        fadeScreen.FadeOut();


        //Launch the new scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float timer = 0;
        while (timer <= fadeScreen.fadeDuration && !operation.isDone)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }

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
        if (scene.buildIndex == 1 && DeactivateElementsInSquareScene)
        {

            DeactivateCharactersAndMusicAndTextAndPanel();
           
            videoPlayer.gameObject.SetActive(true);

            if (videoClips.Length > 0)
            {
                SetupVideoPlayer();
                StartCoroutine(ShowButtonsAfterDelay(60));
            }

        }
       

        DeactivateElementsInSquareScene = false; // Reset the flag
    }

    private void DeactivateCharactersAndMusicAndTextAndPanel ()
    {
        // Find and deactivate characters
        foreach (GameObject character in GameObject.FindGameObjectsWithTag("Character"))
        {
            character.SetActive(false);
        }

        GameObject profilePanel = GameObject.FindGameObjectWithTag("ProfilePanel");
        if (profilePanel != null)
        {
            profilePanel.SetActive(false);
        }

        GameObject exploreBtn = GameObject.FindGameObjectWithTag("ExploreButton");
        if (exploreBtn != null)
        {
            exploreBtn.SetActive(false);
        }

        GameObject[] textsToDeactivate = GameObject.FindGameObjectsWithTag("DeactivatableText");
        foreach (GameObject textObject in textsToDeactivate)
        {
            if (textObject != null)
            {
                textObject.SetActive(false);
            }
        }

        // Find and stop background music
        GameObject backgroundMusic = GameObject.FindGameObjectWithTag("BackgroundMusic");
        if (backgroundMusic != null)
        {
            AudioSource audioSource = backgroundMusic.GetComponent<AudioSource>();
            if (audioSource != null) audioSource.Stop();
        }
     

    }
 

    void SetupVideoPlayer()
    {
       
        videoPlayer.clip = videoClips[currentVideoIndex];
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Prepare();
    }

    void OnVideoPrepared(VideoPlayer source)
    {
        source.Play();

        // Special case for the first video
        if (currentVideoIndex == 0)
        {
            Invoke(nameof(StopFirstVideo), 20);
        }

     
    }

    void OnVideoFinished(VideoPlayer source)
    {
        currentVideoIndex++;

        if (currentVideoIndex < videoClips.Length)
        {
            // There are more videos to play
            videoPlayer.clip = videoClips[currentVideoIndex];
            videoPlayer.Prepare(); // This will trigger OnVideoPrepared when done
        }
        else
        {
            // All videos have been played
          
            videoPlayer.prepareCompleted -= OnVideoPrepared;
            videoPlayer.loopPointReached -= OnVideoFinished;
            videoPlayer.gameObject.SetActive(false);
        }
    }

    void StopFirstVideo()
    {
        // Check if the first video is still playing, if so, move to the next
        if (currentVideoIndex == 0 && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            OnVideoFinished(videoPlayer); // Manually trigger the finish event for the first video
        }
    }
    IEnumerator ShowButtonsAfterDelay(float delay)
        {

            yield return new WaitForSeconds(delay);
           
            GameObject quitButton = GameObject.FindGameObjectWithTag("QuitButton");
            if (quitButton != null)
            {
                quitButton.SetActive(true);
                Debug.Log("Quit button is now active.");
            }
            else
            {
                Debug.Log("Quit button not found.");
            }
        }
    
}
