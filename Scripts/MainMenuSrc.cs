using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuSrc : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject shopScreen;
    public GameObject settingsScreen;
    public GameObject nameInput;
    public GameObject shopSelect;
    public GameObject shopReward;

    public int[] charPrices;

    public TMP_Text InputTxt;
    public TMP_Text coinTxt1;
    public TMP_Text coinTxt2;
    public TMP_Text priceTxt;

    public Transform[] ShopSprites;
    public List<int> unlockedChars = new List<int>();

    int charInd = 0;
    public int coinCnt;
    int charCnt;
    Transform cam;
    Vector3 camTargetpos;

    private void Start()
    {
        string unlockCharStr = PlayerPrefs.GetString("CharUnlocked");

        for (int i = 0; i < unlockCharStr.Length; i++)
        {
            unlockedChars.Add(int.Parse(unlockCharStr[i].ToString()));
        }
        charCnt = ShopSprites.Length;
        cam = Camera.main.transform;
        camTargetpos = new Vector3(0, 0, -10);
        charInd = PlayerPrefs.GetInt("PlayerInd");
        coinCnt = PlayerPrefs.GetInt("coin");
        SetPrice();
    }

    private void Update()
    {
        PlayerPrefs.SetInt("coin",coinCnt);
    }

    private void FixedUpdate()
    {
        cam.position = Vector3.Lerp(cam.position,camTargetpos,8f* Time.fixedDeltaTime);
        coinTxt1.text = coinCnt.ToString();
        coinTxt2.text = coinCnt.ToString();
    }

    public void OnMainToShop()
    {
        string unlockCharStr = PlayerPrefs.GetString("CharUnlocked");
        unlockedChars.Clear();

        for (int i = 0; i < unlockCharStr.Length; i++)
        {
            unlockedChars.Add(int.Parse(unlockCharStr[i].ToString()));
        }

        mainScreen.SetActive(false);
        shopScreen.SetActive(true);
        charInd = PlayerPrefs.GetInt("PlayerInd");
        camTargetpos = new Vector3(ShopSprites[charInd].position.x, ShopSprites[0].position.y, -10);

        for (int i = 0; i < unlockedChars.Count; i++)
        {
            charPrices[unlockedChars[i]] = 0;
        }
    }

    public void OnShopToMain()
    {
        shopScreen.SetActive(false);
        mainScreen.SetActive(true);
        camTargetpos = new Vector3(0, 0, -10);
    }

    public void OnShopNext()
    {
        if (charInd >= charCnt - 1) charInd = 0;
        else charInd++;
        camTargetpos = camTargetpos = new Vector3(ShopSprites[charInd].position.x, ShopSprites[charInd].position.y, -10);
        SetPrice();
    }

    public void OnShopPrev()
    {
        if (charInd <= 0) charInd = charCnt-1;
        else charInd--;
        camTargetpos = camTargetpos = new Vector3(ShopSprites[charInd].position.x, ShopSprites[charInd].position.y, -10);
        SetPrice();
    }

    public void OnShopSelect()
    {
        PlayerPrefs.SetString("CharUnlocked", PlayerPrefs.GetString("CharUnlocked")+charInd);

        string unlockCharStr = PlayerPrefs.GetString("CharUnlocked");
        unlockedChars.Clear();

        for (int i = 0; i < unlockCharStr.Length; i++)
        {
            unlockedChars.Add(int.Parse(unlockCharStr[i].ToString()));
        }

        coinCnt -= charPrices[charInd];



        PlayerPrefs.SetInt("PlayerInd", charInd);
        OnShopToMain();
    }

    public void OnMainToSetting()
    {
        mainScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }
    
    public void OnSettingToMain()
    {
        settingsScreen.SetActive(false);
        mainScreen.SetActive(true);
    }

    public void OnInputPlay()
    {
        PlayerPrefs.SetString("Player1Name", InputTxt.text);
        OnPlay();
    }

    public void OnPlay()
    {
        if (PlayerPrefs.GetString("Player1Name") == "")
        {
            nameInput.SetActive(true);
        }
        else
        { 
            SceneManager.LoadSceneAsync(1);
        
        }
    }

    public void SetPrice()
    {
        int price = charPrices[charInd];

        if (price == 0)
        { 
            priceTxt.text = "Unlocked";
            shopSelect.SetActive(true);
            shopReward.SetActive(false);
        }
        else if(price <= coinCnt)
        {
            shopSelect.SetActive(true);
            shopReward.SetActive(false);
            priceTxt.text = price.ToString();
        }
        else
        {
            shopSelect.SetActive(false);
            shopReward.SetActive(true);
            priceTxt.text = price.ToString();
        }
    }

    public void OnReward()
    {
        FindObjectOfType<AdsManager>().ShowRewardAd();
        SetPrice();
    }
}