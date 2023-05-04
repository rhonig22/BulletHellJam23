using BulletFury.Data;
using BulletFury;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCollider : MonoBehaviour
{
    Vector3 tempPosition= Vector3.zero;
    public bool isDead { get; private set; } = false;
    public bool isActive { get; private set; } = false;

    public UnityEvent startFire = new UnityEvent();

    [SerializeField] Animator animator;
    public void BulletCollided(BulletContainer container, BulletCollider collider, GameObject bullet)
    {
        if (container.Damage == 0)
        {
            tempPosition = transform.position;
            animator.SetTrigger("Dead");
            isDead= true;
        }
    }

    private void LateUpdate()
    {
        if (tempPosition != Vector3.zero)
        {
            transform.position = tempPosition;
        }
    }

    public void DeathEnd()
    {
        tempPosition = Vector3.zero;
        isDead= false;
    }

    public void ActivationStart()
    {
        isActive = true;
        isDead = false;
        startFire.Invoke();
    }

    public void ActivationEnd()
    {
        isActive = false;
    }
}
