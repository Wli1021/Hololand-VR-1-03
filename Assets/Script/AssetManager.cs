using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AssetManager : MonoBehaviour
{
    #region Singleton: AssetManager
    public static AssetManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //CheckStartType();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

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
        CheckForNewSession();

        UpdateAssetUI();
      
    }

    void CheckForNewSession()
    {
        if (PlayerPrefs.GetInt("NewSession", 0) == 1)
        {
            ResetDataToInitialValues();
            PlayerPrefs.SetInt("NewSession", 0); // Clear the new session flag
            PlayerPrefs.Save();
        }
        //else
        //{
            //LoadData(); // Load data if not a new session
        //}
    }

    public void ResetDataToInitialValues()
    {
        // Reset data to initial values
        coins = 6;
        landCount = 1;
        rentedLandCount = 0;
        collectionCount = 1;
        //SaveData(); // Save the reset state
    }

    //void LoadData()
    //{
        // Load data from PlayerPrefs
        //coins = PlayerPrefs.GetInt("coins", 6);
        //landCount = PlayerPrefs.GetInt("landCount", 1);
        //rentedLandCount = PlayerPrefs.GetInt("RentedLandCount", 0);
        //collectionCount = PlayerPrefs.GetInt("CollectionCount", 1);
    //}



    public void SaveData()
    {
        PlayerPrefs.SetInt("coins", coins);
        PlayerPrefs.SetInt("landCount", landCount);
        PlayerPrefs.SetInt("RentedLandCount", rentedLandCount);
        PlayerPrefs.SetInt("CollectionCount", collectionCount);
       
        PlayerPrefs.Save();
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

    public void UpdateAssetUI()
    {
        coinsText.text = "Coins: " + coins;
        landText.text = "Land: " + landCount;
        rentedLandText.text = "Rented Land: " + rentedLandCount;
        collectionText.text = "Collection: " + collectionCount;

        int totalVotingPower = GetTotalVotingPower();
        totalVotingPowerText.text = "Total Voting Power: " + totalVotingPower.ToString();

        Debug.Log($"Updating UI: Coins: {coins}, Land: {landCount}, RentedLand: {rentedLandCount}, Collection: {collectionCount}");
    }

    public void PurchaseLand(int price)
    {
        coins -= price; //Deduct the price when purchasing land
        landCount++;
        UpdateAssetUI();
        SaveData();
    }

    public void PurchaseRentedLand(int price)
    {
        coins -= price;
        rentedLandCount++;
        UpdateAssetUI();
        SaveData();
    }

    public void PurchaseCollection(int price)
    {
        coins -= price;
        collectionCount++;
        UpdateAssetUI();
        SaveData();
    }

    //Spending coins
    public void SpendCoins(int amount)
    {
        coins -= amount; // Deduct spent coins from the total number of coins
        int spentCoinsVP = amount * 1; // Calculate the contribution of spent coins to total voting power
        totalVotingPower -= spentCoinsVP; // Deduct the contribution to total voting power based on spent coins
        UpdateAssetUI();
        SaveData();
    }

    public bool HasEnoughCoins(int amount)
    {
        return (coins >= amount);
    }

}
