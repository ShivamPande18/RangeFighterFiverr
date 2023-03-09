using UnityEngine;

public class ShootingSrc : MonoBehaviour
{
    public Vector3 currentPosition;

    public GameObject weapon;
    public Transform center;


    public float weaponPositionOffset;
    public float maxLength;
    public float force;

    public bool isPlayer;

    GameManager gameManager;
    Rigidbody2D weaponRB;
    LineRenderer lr;
    Camera cam;

    bool isMouseDown;

    Vector3 startPos;
    Vector3 endPos;
    //float error = 2;

    private void Start()
    {
        InitWeapon();
        startPos = center.position;
        cam = Camera.main;
        lr = GetComponent<LineRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        weaponRB.isKinematic = true;
    }

    void InitWeapon()
    {
        weaponRB = Instantiate(weapon).GetComponent<Rigidbody2D>();
        weaponRB.GetComponent<ArrowSrc>().isPlayer = isPlayer;
        ResetStrips();
    }


    void Update()
    {
        if (!isPlayer) return;


        if(Input.GetMouseButtonDown(0)) gameManager.SetStage(GameManager.Stage.PlayerAim);
        if (Input.GetMouseButtonUp(0)) gameManager.SetStage(GameManager.Stage.PlayerShot);
        

        if (isMouseDown)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;

            currentPosition = cam.ScreenToWorldPoint(mousePosition);
            currentPosition = center.position + Vector3.ClampMagnitude(currentPosition - center.position, maxLength);

            SetStrips(currentPosition);

            Vector2 arrowPos = weaponRB.transform.position;
            endPos = new Vector3(startPos.x - (arrowPos.x - startPos.x), startPos.y - (arrowPos.y - startPos.y));

            lr.enabled = true;
            lr.positionCount = 2;
            lr.SetPosition(0, startPos);
            lr.SetPosition(1, endPos);
            lr.useWorldSpace = true;
            lr.numCapVertices = 10;
        }
        else
        {
            ResetStrips();
            lr.enabled = false;
        }
    }

    

    private void OnMouseDown()
    {
        isMouseDown = true;
    }

    private void OnMouseUp()
    {
        if (!isPlayer) return;
        isMouseDown = false;
        Shoot();
        currentPosition = center.position;
    }
    public void Shoot()
    {
        if (isPlayer)
        {
            weaponRB.isKinematic = false;
            weaponRB.tag = "arrow";
            Vector3 arrowForce = (currentPosition - center.position) * force * -1;
            weaponRB.velocity = arrowForce;
            gameManager.SetStage(GameManager.Stage.PlayerAim);

        }
        else
        {
            weaponRB.isKinematic = false;
            weaponRB.tag = "arrow";
            Vector3 arrowForce = new Vector3(1, -2, 0) * force * -1;
            weaponRB.velocity = arrowForce;
            gameManager.SetStage(GameManager.Stage.EnemyShot);
        }
        InitWeaponHelper();
    }

    public void InitWeaponHelper()
    {
        Invoke("InitWeapon", 2);
    }

    void ResetStrips()
    {
        currentPosition = center.position;
        SetStrips(currentPosition);
    }

    void SetStrips(Vector3 position)
    {
        if (weaponRB)
        {
            Vector3 dir = position - center.position;
            weaponRB.transform.position = position + dir.normalized * weaponPositionOffset;
            weaponRB.transform.right = -dir.normalized;
        }
    }
}