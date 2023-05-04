using BulletFury.Data;
using BulletFury;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies= new List<GameObject>();
    [SerializeField] private GameObject player;
    [SerializeField] private BulletManager enemyBullets;
    private readonly float enemyWaitPeriod = 1f;
    private readonly float enemyYRange = 4f;
    private readonly float bulletDelay = .05f;

    private void Start()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            StartCoroutine(startEnemy(i, 0));
        }
    }

    private void Update()
    {
        
    }

    IEnumerator startEnemy(int index, float delay)
    {
        GameObject enemy = enemies[index];
        EnemyCollider collider = enemy.GetComponentInChildren<EnemyCollider>();
        collider.startFire.AddListener(() => { StartCoroutine(spawnBullets(enemy));  });
        while (true)
        {
            yield return new WaitForSeconds(enemyWaitPeriod + delay);
            delay = 4f;
            enemy.transform.position = new Vector3(0, Random.Range(-enemyYRange, enemyYRange), 0);
            enemy.GetComponentInChildren<Animator>().SetTrigger("Start");
        }
    }

    IEnumerator spawnBullets(GameObject enemy)
    {
        EnemyCollider collider = enemy.GetComponentInChildren<EnemyCollider>();
        while (!collider.isDead && collider.isActive)
        {
            yield return new WaitForSeconds(bulletDelay);
            enemyBullets.Spawn(enemy.transform.GetChild(0));

        }
    }
}
