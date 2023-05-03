using BulletFury.Data;
using BulletFury;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    [SerializeField] Animator animator;
    public void BulletCollided(BulletContainer container, BulletCollider collider, GameObject bullet)
    {
        if (container.Damage == 0)
        {
            var temp = transform.position;
            animator.SetTrigger("Dead");
            transform.position = temp;
        }
    }
}
