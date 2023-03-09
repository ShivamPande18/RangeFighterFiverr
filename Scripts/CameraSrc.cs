using UnityEngine;

public class CameraSrc : MonoBehaviour
{
    GameManager gameManager;
    Transform player;
    Transform enemy; 
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
        gameManager = FindObjectOfType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
    }

    private void Update()
    {
        if (gameManager.hasGameEnded)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y, -10), 2 * Time.deltaTime);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 10, 10 * Time.deltaTime);
        }
        else
        {
            if (gameManager.GetStage() == GameManager.Stage.PlayerAim)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y, -10), 2 * Time.deltaTime);
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5, 10 * Time.deltaTime);
            }
            else if (gameManager.GetStage() == GameManager.Stage.PlayerShot)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(GameObject.FindGameObjectWithTag("arrow").transform.position.x, GameObject.FindGameObjectWithTag("arrow").transform.position.y, -10), 10);
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 20, 10 * Time.deltaTime);
            }
            else if (gameManager.GetStage() == GameManager.Stage.EnemyAim)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(enemy.position.x, enemy.position.y, -10), 2 * Time.deltaTime);
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5, 10 * Time.deltaTime);
            }
            else if (gameManager.GetStage() == GameManager.Stage.EnemyShot)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(GameObject.FindGameObjectWithTag("arrow").transform.position.x, GameObject.FindGameObjectWithTag("arrow").transform.position.y, -10), 10);
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 20, 10 * Time.deltaTime);
            }
        }
    }
}