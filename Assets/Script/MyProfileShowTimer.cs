using UnityEngine;
using UnityEngine.UI; // Remember to include this namespace to work with UI elements

public class MyProfileShowTimer : MonoBehaviour
{
    public GameObject profileToAppear; // Assign the button GameObject in the Inspector
    public float delay = 85f; //100f; // Time in seconds before the button appears (1 minute and 40 seconds)

    // Start is called before the first frame update
    void Start()
    {
        if (profileToAppear != null)
        {
            profileToAppear.SetActive(false); // Initially hide the button
            Invoke("ShowButton", delay); // Schedule the button to appear after the delay
        }
    }

    void ShowButton()
    {
        if (profileToAppear != null)
        {
            profileToAppear.SetActive(true); // Show the button
        }
    }
}