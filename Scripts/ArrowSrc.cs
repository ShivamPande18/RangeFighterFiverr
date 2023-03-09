using UnityEngine;

public class ArrowSrc : MonoBehaviour
{
    public GameObject bloodPart;
    public GameObject impactPart;
    public bool isPlayer = true;

    Rigidbody2D rb;
    GameManager gameManager;
    float angle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        angle = rb.velocity.y == 0 ? angle = 0 : Mathf.Atan(rb.velocity.y / rb.velocity.x);
        
        if (isPlayer)
        {
            transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg - 90);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg + 90);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPlayer)
        { 
            gameManager.SetStage(GameManager.Stage.EnemyAim);
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<ShootSrc>().EnemyShoot();
            if (collision.gameObject.CompareTag("Enemy"))
            { 
                gameManager.enemyHealth--;
                Destroy(Instantiate(bloodPart,transform.position,transform.rotation),2f);
            }
            else
            {
                Destroy(Instantiate(impactPart, transform.position, transform.rotation), 2f);
            }
        }
        else
        {
            gameManager.SetStage(GameManager.Stage.PlayerAim);
            if (collision.gameObject.CompareTag("Player"))
            {
                gameManager.playerHealth--;
                Destroy(Instantiate(bloodPart, transform.position, transform.rotation), 2f);
            }
            else
            {
                Destroy(Instantiate(impactPart, transform.position, transform.rotation), 2f);
            }
        }
        //Destroy(Instantiate(bloodPart, transform.position, transform.rotation), 2f);
        Destroy(gameObject);
    }
}