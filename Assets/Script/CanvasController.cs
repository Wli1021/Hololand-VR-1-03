using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject proposalCanvas;
    private Animator  canvasAnimator;
    public GameObject nextButton;
    public GameObject yesButton;
    public GameObject noButton;
    public GameObject BackToRealityButton;

    void Start()
    {
       canvasAnimator = proposalCanvas.GetComponent<Animator>();
    }
    //public void MakeItTrue()
    //{
       // canvasAnimator.SetBool("PlayTransition", true);
    //}
    //public void MakeItFalse()
    //{
       // canvasAnimator.SetBool("PlayTransition", false);
    //}

    public void ShowYesNo()
    {
        yesButton.SetActive(true);
        noButton.SetActive(true);
    }
    public void HideYesNo()
    {
        yesButton.SetActive(false);
        noButton.SetActive(false);
    }
    public void ShowNext()
    {
        nextButton.SetActive(true);

    }
    public void HideNext()
    {
        nextButton.SetActive(false);

    }

    public void ShowQuit()
    {
        BackToRealityButton.SetActive(true);

    }
}
