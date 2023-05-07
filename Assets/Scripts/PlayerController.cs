using BulletFury;
using BulletFury.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Vector3 mousePosition;
    private float horizontalInput = 0f;
    private float verticalInput = 0f;
    private float speed = 8f;
    private bool isDead = false;
    private bool isMouseDown = false;
    private readonly float leftBound = 5.5f;
    private readonly float topBound = 4.25f;
    private readonly int maxCapacity = 10;
    private readonly int shieldPowerUpCount = 5;
    public readonly int minThreshold = 3;
    [SerializeField] private BulletManager bulletManager;
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private Animator animator;
    private Shield shield;
    private int _currentAmmo = 10;
    public int CurrentAmmo
    {
        get
        {
            return _currentAmmo;
        }
        private set
        {
            _currentAmmo = value;
            shield.ShieldPercent = (float)value / (float)maxCapacity;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        shield = shieldObject.GetComponent<Shield>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isMouseDown = Input.GetMouseButton(0);
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        Move(horizontalInput * speed * Time.fixedDeltaTime, verticalInput * speed * Time.fixedDeltaTime);
        Rotate();
        Fire();
    }

    private void Move(float xSpeed, float ySpeed)
    {
        Vector3 newPos = transform.position + new Vector3(xSpeed, ySpeed);
        if (Mathf.Abs(newPos.x) > leftBound)
            newPos.x = newPos.x > 0 ? leftBound : -leftBound;

        if (Mathf.Abs(newPos.y) > topBound)
            newPos.y = newPos.y > 0 ? topBound : -topBound;

        transform.position = newPos;
    }

    private void Rotate()
    {
        Vector3 perpendicular = mousePosition - transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular);
    }

    private void Fire()
    {
        if (isMouseDown && CurrentAmmo > 0)
        {
            bulletManager.Spawn(transform.position, transform.up);
        }
    }

    public void WeaponFired()
    {
        if (CurrentAmmo > 0)
        {
            CurrentAmmo--;
        }
    }

    public void BulletCollided(BulletContainer container, BulletCollider collider, GameObject bullet)
    {
        bool isEnemy = container.Damage > 0;
        if (!isEnemy)
        {
            if (CurrentAmmo < maxCapacity)
            {
                CurrentAmmo++;
            }
        }
        else
        {
            if (CurrentAmmo == 0)
            {
                Death();
            }
            else
            {
                CurrentAmmo--;
            }
        }
    }

    public void Death()
    {
        isDead = true;
        animator.SetTrigger("Death");
    }

    public void DeathFinished()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(MenuUiController.gameOverScene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collide");
        if (isDead)
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyCollider collider = collision.gameObject.GetComponentInChildren<EnemyCollider>();
            if (collider != null && collider.isActive && !collider.isDead)
                CurrentAmmo = 0;
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            ShieldPowerUp();
            Destroy(collision.gameObject);
        }
    }

    private void ShieldPowerUp()
    {
        CurrentAmmo += shieldPowerUpCount;
        if (CurrentAmmo > maxCapacity)
        {
            CurrentAmmo = maxCapacity;
        }
    }
}
