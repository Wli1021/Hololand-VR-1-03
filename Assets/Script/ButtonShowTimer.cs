using UnityEngine;
using UnityEngine.UI; // Remember to include this namespace to work with UI elements

public class ButtonShowTimer : MonoBehaviour
{
    public GameObject buttonToAppear; // Assign the button GameObject in the Inspector
    public float delay = 95f; // Time in seconds before the button appears (1 minute and 40 seconds)

    // Start is called before the first frame update
    void Start()
    {
        if (buttonToAppear != null)
        {
            buttonToAppear.SetActive(false); // Initially hide the button
            Invoke("ShowButton", delay); // Schedule the button to appear after the delay
        }
    }

    void ShowButton()
    {
        if (buttonToAppear != null)
        {
            buttonToAppear.SetActive(true); // Show the button
        }
    }
}
