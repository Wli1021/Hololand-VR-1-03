using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AssetManagerForScene3 : MonoBehaviour
{
    

    public int coins;
    public int landCount;
    public int rentedLandCount;
    public int collectionCount;
    public int totalVotingPower;

    public TMP_Text coinsText;
    public TMP_Text landText;
    public TMP_Text rentedLandText;
    public TMP_Text collectionText;
    public TMP_Text totalVotingPowerText;



    void Start()
    {
        LoadData();
     
    }



    void LoadData()
    {
        // Load saved data from PlayerPrefs and assign to class-level variables
        coins = PlayerPrefs.GetInt("coins", 6);
        Debug.Log($"Loaded Coins: {coins}");
        landCount = PlayerPrefs.GetInt("landCount", 1); 
        rentedLandCount = PlayerPrefs.GetInt("RentedLandCount", 0);
        collectionCount = PlayerPrefs.GetInt("CollectionCount", 1);

        UpdateAssetUI();
    }

    
    public int GetTotalVotingPower()
    {
        // Calculate total voting power based on assets and coins
        int spentCoinsVP = coins * 1; // Contribution of spent coins to total voting power
        int landVP = landCount * 2000;
        int collectionVP = collectionCount * 100;

        int totalVotingPower = spentCoinsVP + landVP + collectionVP;
        return totalVotingPower;
    }

    //Added for coins increase if the proposal vote yes been approved,this method will be used in ProposalManager
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateAssetUI(); // Assuming this updates the UI with the new coin total
       
    }

    public void UpdateAssetUI()
    {
       
        coinsText.text = "Coins: " + coins;
        landText.text = "Land: " + landCount;
        rentedLandText.text = "Rented Land: " + rentedLandCount;
        collectionText.text = "Collection: " + collectionCount;

        int totalVotingPower = GetTotalVotingPower();
        totalVotingPowerText.text = "Total Voting Power: " + totalVotingPower.ToString();
       
        Debug.Log(PlayerPrefs.GetInt("coins"));
    }
}
