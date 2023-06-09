using BulletFury.Data;
using BulletFury;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCollider : MonoBehaviour
{
    private Vector3 tempPosition= Vector3.zero;
    private Quaternion tempRotation = Quaternion.identity;
    public bool isDead { get; private set; } = false;
    public bool isActive { get; private set; } = false;

    public UnityEvent startFire = new UnityEvent();
    public UnityEvent setRotation = new UnityEvent();
    public bool isFixedRotation;
    [SerializeField] Animator animator;
    [SerializeField] public SpawnSettings spawnSettings;
    [SerializeField] public BulletSettings bulletSettings;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private BulletCollider bulletCollider;

    private void Start()
    {
        setRotation.AddListener(() => { tempRotation = transform.rotation; } );
        audioSource.volume = DataManager.effectsVolume;
    }

    public void BulletCollided(BulletContainer container, BulletCollider collider, GameObject bullet)
    {
        if (isDead) return;

        if (container.Damage == 0)
        {
            tempPosition = transform.position;
            bulletCollider.enabled = false;
            animator.SetTrigger("Dead");
            DataManager.Instance.IncreaseKill();
            audioSource.Play();
            isDead= true;
        }
    }

    private void LateUpdate()
    {
        if (tempPosition != Vector3.zero)
        {
            transform.position = tempPosition;
        }

        if (tempRotation != Quaternion.identity)
        {
            transform.rotation = tempRotation;
        }
    }

    public void DeathEnd()
    {
        tempPosition = Vector3.zero;
        isDead= false;
        animator.ResetTrigger("Dead");
    }

    public void ActivationStart()
    {
        isActive = true;
        isDead = false;
        bulletCollider.enabled = true;
    }

    public void StartBullets()
    {
        startFire.Invoke();
    }

    public void ActivationEnd()
    {
        tempRotation = Quaternion.identity;
        isActive = false;
    }
}
