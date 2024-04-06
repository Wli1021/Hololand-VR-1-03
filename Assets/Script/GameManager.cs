using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject shopUI;
    public Transform head;
    public float spawnDistance = 4;

    [SerializeField] TMP_Text[] allCoinsUIText;

    public int coins;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("AssetManager instance before UpdateAllCoinsUIText: " + AssetManager.instance);
        UpdateAllCoinsUIText();
        HideShopUI();
        
    }


    public void HideShopUI()
    {
        shopUI.SetActive(false);


    }

    public void ShowShopUI()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(true); 
          
        }
 
    }
    public void CloseShopUI()
    {
        if (shopUI != null)
        {
            shopUI.SetActive(false); 
        }
        
    }


    //public void UseCoins(int amount)
    //{
    //AssetManager.instance.SpendCoins(amount);
    //}

    //public bool HasEnoughCoins(int amount)
    //{
    //return (AssetManager.instance.coins >= amount);
    //}

    public void UpdateAllCoinsUIText()
    {
        Debug.Log($"Updating UI with {AssetManager.instance.coins} coins across {allCoinsUIText.Length} text elements.");
        foreach (TMP_Text coinText in allCoinsUIText)
        {
            coinText.text = "Coins: " + AssetManager.instance.coins.ToString();
            Debug.Log($"Set text to: {coinText.text}");
        }
    }

}
