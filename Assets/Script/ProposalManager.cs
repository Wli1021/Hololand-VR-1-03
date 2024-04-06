using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class DiscussionAudioClipList
{
public List<AudioSource> audioSources = new List<AudioSource>();
}

[System.Serializable]
public class AvatarVP
{
    public TMP_Text vpText; // Reference to the UI Text component displaying the Voting Power
    public int vp; // Current Voting Power value
    public TMP_Text floatingText; // Reference to the "+100" TextMeshPro component
    public Animator floatingTextAnimator;
    public Animator talkAnimator;

    // Call this to update the Voting Power and UI
    public void AddVP(int amount)
    {
        vp += amount;
        vpText.text = $"VP: {vp:N0}";
        PlayFloatingEffect(amount);
    }

    // Triggers the floating effect for the "+100" text
    private void PlayFloatingEffect(int amount)
    {
        floatingText.text = $"+{amount}";
        floatingText.gameObject.SetActive(true);
        floatingTextAnimator.Play("FloatingTextAnimation"); // Assume this animation makes the text appear, then disappear
    }


}
public class ProposalManager : MonoBehaviour
{
    public GameObject[] proposals;
    public GameObject[] proposalResultPanels;
    public Button yesButton, noButton, nextButton, startButton,backToRealityButton;
    public AudioSource introductionAudio;
    public AudioSource coinSoundEffect;
    public List<DiscussionAudioClipList> discussionAudioSources; // Changed to use a list of lists
    public AssetManagerForScene3 assetManager;
    public List<AvatarVP> avatars;
    public TMP_Text playerFloatingText;
    public Animator playerFloatingTextAnimator;


    private int currentProposalIndex = 0;
    private bool[] proposalOutcomes = new bool[6] { true, false, true, true, false, false }; // Preset outcomes
    private int[][] speakerOrders = new int[][]
    {
       new int[] {0, 3, 2, 0}, // Proposal 1: A, D, C, A
       new int[] {0, 3, 2, 1, 0}, // Proposal 2: A, D, C, B, A
       new int[] {3, 1, 2, 3, 0}, // Proposal 3: D, B, C, D, A
       new int[] {1, 3, 2, 0}, // Proposal 4: B, D, C, A
       new int[] {3, 2, 1, 0}, // Proposal 5: D, C, B, A
       new int[] {1, 3, 2, 0} // Proposal 6: B, D, C, A
    };

    void Start()
    {
        DeactivateAllProposalsAndResults();
        InitializeButtons();
        PlayIntroAudio();
        InitializeTalkingState();
    }
    void InitializeTalkingState()
    {      
        PlayTalkingAnimation(0); // Avatar A starts talking

    }
    void DeactivateAllProposalsAndResults()
    {
        foreach (var proposal in proposals) proposal.SetActive(false);
        foreach (var resultPanel in proposalResultPanels) resultPanel.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(false);
    }

    void PlayIntroAudio()
    {
        introductionAudio.Play();
        StartCoroutine(WaitForIntroEnd(introductionAudio.clip.length));
    }

    IEnumerator WaitForIntroEnd(float length)
    {
        yield return new WaitForSeconds(length);
        DeactivateAllTalkingAnimations();
        startButton.gameObject.SetActive(true); // Show start button after intro
    }

    void InitializeButtons()
    {
        yesButton.onClick.AddListener(() => HandleResponse(true));
        noButton.onClick.AddListener(() => HandleResponse(false));
        nextButton.onClick.AddListener(ShowNextProposal);
        startButton.onClick.AddListener(() => StartCoroutine(ShowProposalWithDelay(0, 1))); // Start with the first proposal
        backToRealityButton.gameObject.SetActive(false); 
        
    }

   

    IEnumerator ShowProposalWithDelay(int index, float delay)
    {
        proposals[index].SetActive(true);
        startButton.gameObject.SetActive(false); // Hide start button after voting starts
        yield return new WaitForSeconds(delay);
        StartCoroutine(PlayDiscussionClips(index));
    }

    //IEnumerator PlayDiscussionClips(int index)
    //{
    //foreach (var audioSource in discussionAudioSources[index].audioSources)
    //{
    //audioSource.Play();
    //yield return new WaitForSeconds(audioSource.clip.length + 0.5f); // Wait for the clip to finish plus a brief pause
    //}
    //yesButton.gameObject.SetActive(true);
    //noButton.gameObject.SetActive(true);
    //}

    IEnumerator PlayDiscussionClips(int index)
    {
        if (index < 0 || index >= speakerOrders.Length)
        {
            Debug.LogError("Invalid proposal index. Cannot play discussion clips.");
            yield break; // Stop the coroutine if the index is out of range
        }

        int[] speakerOrder = speakerOrders[index]; // Get the speaker order for the current proposal

        for (int i = 0; i < discussionAudioSources[index].audioSources.Count; i++)
        {
            var audioSource = discussionAudioSources[index].audioSources[i];
            int avatarIndex = speakerOrder[i]; // Use the modified speaker order for each proposal

            // Ensure all talking animations are deactivated before activating the current speaker's animation
            DeactivateAllTalkingAnimations();

            // Activate talking animation for the current speaker
            avatars[avatarIndex].talkAnimator.SetBool("IsTalking", true);

            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length); // Wait for the clip to finish

            // Immediately deactivate the talking animation of the current speaker after their audio clip ends
            avatars[avatarIndex].talkAnimator.SetBool("IsTalking", false);
        }

        // Ensure all talking animations are deactivated after the discussion
        DeactivateAllTalkingAnimations();

        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }

    private void PlayTalkingAnimation(int avatarIndex)
    {
        // Deactivate talking animations for all avatars first
        foreach (var avatar in avatars)
        {
            avatar.talkAnimator.SetBool("IsTalking", false); // Assuming the animation is controlled by a boolean parameter named "IsTalking"
        }

        // Activate talking animation for the current speaker, if the index is valid
        if (avatarIndex >= 0 && avatarIndex < avatars.Count)
        {
            avatars[avatarIndex].talkAnimator.SetBool("IsTalking", true);
        }
    }

    private void DeactivateAllTalkingAnimations()
    {
        foreach (var avatar in avatars)
        {
            avatar.talkAnimator.SetBool("IsTalking", false);
        }
    }
    void HandleResponse(bool playerVotedYes)
    {
        proposals[currentProposalIndex].SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        if (playerVotedYes && proposalOutcomes[currentProposalIndex])
        {
            Debug.Log("Proposal approved, player voted YES. Adding 100 coins as a reward.");
            assetManager.AddCoins(100); // Assuming AssetManagerForScene3 has a method UpdateCoins(int amount)
            PlayFloatingTextForPlayer();
        }

        proposalResultPanels[currentProposalIndex].SetActive(true);
        UpdateAvatarsVP(currentProposalIndex);
        StartCoroutine(EnableNextButtonAfterDelay(5)); // Adjust time as needed
    }
    private void PlayFloatingTextForPlayer()
    {
        playerFloatingText.text = "+100";
        playerFloatingText.gameObject.SetActive(true);
        playerFloatingTextAnimator.Play("FloatingTextAnimation"); // Play floating text animation for the player
        coinSoundEffect.Play(); // Play the coin sound effect
    }

    IEnumerator EnableNextButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        nextButton.gameObject.SetActive(true);
    }

    void UpdateAvatarsVP(int proposalIndex)
    {
        // Example logic for updating based on the proposal outcome
        switch (proposalIndex)
        {
            case 0: // Proposal 1
                foreach (var avatar in avatars) avatar.AddVP(100); // All avatars get 100 VP
                break;
            case 2: // Proposal 3
                avatars[0].AddVP(100); // A gets 100 VP
                avatars[2].AddVP(100); // C gets 100 VP
                avatars[3].AddVP(100); // D gets 100 VP
                break;
            case 3: // Proposal 4
                avatars[1].AddVP(100); // B gets 100 VP
                avatars[2].AddVP(100); // C gets 100 VP
                break;
             
        }
        // Play coin sound effect if any avatar's VP was updated
        if (proposalOutcomes[proposalIndex])
        {
            coinSoundEffect.Play();
        }
    }

    void ShowNextProposal()
    {
        nextButton.gameObject.SetActive(false);
        proposalResultPanels[currentProposalIndex].SetActive(false);

        if (++currentProposalIndex < proposals.Length)
        {
            StartCoroutine(ShowProposalWithDelay(currentProposalIndex, 1)); // Show next proposal after a delay
        }
        else
        {
            Debug.Log("All proposals have been handled.");
           
            backToRealityButton.gameObject.SetActive(true);
            // Handle the end of all proposals here (e.g., navigate to a summary screen or return to the main menu)
        }
    }
}
