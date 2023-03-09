using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbySrc : MonoBehaviour
{
    public int scene;
    public bool isEnd;
    public TMP_Text resultTxt;
    public TMP_Text coinTxt;
    public TMP_Text rewardText;
    public Sprite[] playerSprites;
    public SpriteRenderer playerSpriteImage;

    private void Start()
    {

        Invoke("ChangeScene",2f);

        if (!isEnd) return;

        if (PlayerPrefs.GetInt("PlayerWon") == 1)
        {
            PlayerPrefs.SetInt("coin", PlayerPrefs.GetInt("coin") + 200);
            resultTxt.text = "WON";
            rewardText.text = "+200";
        }
        else
        {
            resultTxt.text = "LOST";
            rewardText.text = "+0";
        }

        coinTxt.text = PlayerPrefs.GetInt("coin").ToString();

        if (PlayerPrefs.GetInt("PlayerInd") == 9) playerSpriteImage.color = Color.black;
        playerSpriteImage.sprite = playerSprites[PlayerPrefs.GetInt("PlayerInd")];

    }

    void ChangeScene()
    {
        if(isEnd)   FindObjectOfType<AdsManager>().ShowInterAd();
        SceneManager.LoadSceneAsync(scene);
    }
}