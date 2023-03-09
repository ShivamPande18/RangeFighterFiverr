using UnityEngine;
using UnityEngine.UI;

public class ShootSrc : MonoBehaviour
{
    public bool isPlayer;
    public float force;
    public float forceLimit;
    public GameObject arrow;
    public Transform center;
    public Vector3 enemyShootDir;
    public AudioSource shootFx;

    Vector3 currentPosition;

    Rigidbody2D arrowRB;
    LineRenderer lr;
    bool isMouseDown;
    bool isMouseUp;
    GameManager gameManager;

    private void Start()
    {
        InitArrow();
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        gameManager = FindObjectOfType<GameManager>();
        gameManager.SetStage(GameManager.Stage.PlayerAim);
    }

    private void Update()
    {
        if (isPlayer)
        {
            if (isMouseUp)
            {
                Shoot();
                
            }
            if(isMouseDown)
            {
                SetMousePosition();
                gameManager.SetStage(GameManager.Stage.PlayerAim);
            }
        }
    }

    void InitArrow()
    {
        arrowRB = Instantiate(arrow,center.position,center.rotation).GetComponent<Rigidbody2D>();
        arrowRB.GetComponentInChildren<SpriteRenderer>().enabled = false;
        arrowRB.GetComponent<ArrowSrc>().isPlayer = isPlayer;
        arrowRB.isKinematic = true;
        if (isPlayer) arrowRB.gameObject.layer = LayerMask.NameToLayer("Player");
        else arrowRB.gameObject.layer = LayerMask.NameToLayer("Enemy");
    }

    void SetMousePosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;

        currentPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        currentPosition = center.position + Vector3.ClampMagnitude(currentPosition - center.position, forceLimit);

        Vector3 startPos = center.position;
        Vector3 endPos = new Vector3(startPos.x - (currentPosition.x - startPos.x), startPos.y - (currentPosition.y - startPos.y));

        lr.enabled = true;
        lr.positionCount = 2;
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
        lr.useWorldSpace = true;
        lr.numCapVertices = 10;
    }

    public void EnemyShoot()
    {
        Invoke("Shoot",3);
    }

    void Shoot()
    {
        arrowRB.GetComponentInChildren<SpriteRenderer>().enabled = true;
        Vector3 arrowForce;
        if (isPlayer)
        {
            shootFx.Play();
            gameManager.SetStage(GameManager.Stage.PlayerShot);
            arrowForce = (currentPosition - center.position) * force * -1;
        }
        else
        {
            //arrowForce = new Vector3(Random.Range(6f,7f), Random.Range(-4f,-5f), 0) * force * -1;
            arrowForce = new Vector3(Random.Range(enemyShootDir.x - 0.5f, enemyShootDir.x + 0.5f), Random.Range(enemyShootDir.y+.5f,enemyShootDir.y-.5f), 0) * force * -1;
            //arrowForce = new Vector3(6.8f,-4.5f,0) * force * -1;
            gameManager.SetStage(GameManager.Stage.EnemyShot);
        }
        arrowRB.isKinematic = false;
        arrowRB.velocity = arrowForce;
        arrowRB.tag = "arrow";
        lr.enabled = false;
        isMouseUp = false;
        Invoke("InitArrow",2);
    }

    private void OnMouseDown()
    {
        isMouseDown = true;
    }

    private void OnMouseUp()
    {
        isMouseUp = true;
        isMouseDown = false;
    }

}