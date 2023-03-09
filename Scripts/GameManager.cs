using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float playerHealth;
    public float enemyHealth;

    public Vector3[] enemyShootDirs;
    public bool hasGameEnded = false;

    public GameObject[] players;
    public GameObject[] levels;
    public int[] playerHealths;
    Transform playerPos;
    Transform enemyPos;

    Collider2D playerColl;
    Image playerHealthBar;
    Image enemyHealthBar;

    float maxPlayerHealth;
    float maxEnemyHealth;

    public enum Stage
    {
        PlayerAim, PlayerShot, EnemyAim, EnemyShot
    }
    private static Stage stage;

    private void Awake()
    {
        int levelInd = Random.Range(0,levels.Length);
        levels[levelInd].SetActive(true);
        playerPos = levels[levelInd].transform.GetChild(1);
        enemyPos = levels[levelInd].transform.GetChild(2);

        

        int enemyInd = Random.Range(0, players.Length);
        GameObject enemy = Instantiate(players[enemyInd],enemyPos.position,enemyPos.rotation) as GameObject;
        enemy.GetComponent<ShootSrc>().isPlayer = false;
        enemy.GetComponent<ShootSrc>().force = 10;
        enemy.tag = "Enemy";
        enemy.layer = LayerMask.NameToLayer("Enemy");

        stage = Stage.PlayerAim;
        int ind = PlayerPrefs.GetInt("PlayerInd");
        GameObject playerGo = Instantiate(players[ind],playerPos.position,playerPos.rotation) as GameObject;
        playerColl = playerGo.GetComponent<Collider2D>();
        playerHealthBar = playerGo.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>();
        enemyHealthBar = enemy.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<Image>();

        maxPlayerHealth = playerHealths[ind];
        maxEnemyHealth = playerHealths[enemyInd];
        playerHealth = maxPlayerHealth;
        enemyHealth = maxEnemyHealth;
        GameObject.FindGameObjectWithTag("Enemy").GetComponent<ShootSrc>().enemyShootDir = enemyShootDirs[levelInd];
    }

    private void Update()
    {
        if (hasGameEnded) return;
        print("Player = " + playerHealth + " mph = " + maxPlayerHealth + "Fill Amount = " + (playerHealth/maxPlayerHealth));
        if (playerHealth <= 0) OnGameEnd(false);
        if (enemyHealth <= 0) OnGameEnd(true);

        playerHealthBar.fillAmount = (float)(playerHealth/maxPlayerHealth); 
        enemyHealthBar.fillAmount = (float)(enemyHealth/maxEnemyHealth);

        if (Input.GetKeyDown(KeyCode.Space)) playerHealth--;
    }

    public void SetStage(Stage newStage)
    {
        stage = newStage;
    }

    public Stage GetStage()
    {
        return stage;
    }

    void OnGameEnd(bool playerWon)
    {
        if (playerWon) PlayerPrefs.SetInt("PlayerWon",1);
        else PlayerPrefs.SetInt("PlayerWon", 0);
        SetStage(Stage.PlayerAim);
        hasGameEnded = true;
        playerColl.enabled = false;
        Invoke("ChangeScene",1f);
    }

    void ChangeScene()
    {
        SceneManager.LoadSceneAsync(3);
    }
}