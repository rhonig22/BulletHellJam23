using BulletFury.Data;
using BulletFury;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies= new List<GameObject>();
    [SerializeField] private GameObject player;
    [SerializeField] private BulletManager greenEnemyBullets;
    [SerializeField] private BulletManager redEnemyBullets;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private readonly float enemyWaitPeriod = 4f;
    private readonly float bulletDelay = .1f;
    private readonly float addEnemyPeriod = 30f;
    private readonly float horizontalXRange = 2f;
    private readonly float horizontalYRange = 4f;
    private readonly float verticalXRange = 5f;
    private readonly float verticalYRange = 1f;

    private void Start()
    {
        StartCoroutine(AddEnemies());
    }

    private void Update()
    {
        
    }

    IEnumerator AddEnemies()
    {
        while(enemies.Count > 0)
        {
            int i = Random.Range(0, enemies.Count);
            StartCoroutine(startEnemy(i, 1f));
            yield return new WaitForSeconds(addEnemyPeriod);
        }
    }

    IEnumerator startEnemy(int index, float delay)
    {
        GameObject enemy = enemies[index];
        enemies.RemoveRange(index, 1);
        activeEnemies.Add(enemy);
        EnemyCollider collider = enemy.GetComponentInChildren<EnemyCollider>();
        collider.startFire.AddListener(() => { StartCoroutine(spawnBullets(enemy, collider));  });
        while (true)
        {
            yield return new WaitForSeconds(delay);
            delay = 0;
            setEnemyOrientation(enemy, collider);
            enemy.GetComponentInChildren<Animator>().SetTrigger("Start");
            yield return new WaitForSeconds(enemyWaitPeriod);
        }
    }

    IEnumerator spawnBullets(GameObject enemy, EnemyCollider collider)
    {
        Transform enemyTransform = enemy.transform.GetChild(0);
        float startDelay = Random.Range(0, .1f);
        while (!collider.isDead && collider.isActive)
        {
            yield return new WaitForSeconds(startDelay);
            startDelay = 0;
            if (collider.bulletSettings == greenEnemyBullets.GetBulletSettings())
                greenEnemyBullets.Spawn(enemyTransform);
            else if (collider.bulletSettings == redEnemyBullets.GetBulletSettings())
                redEnemyBullets.Spawn(enemyTransform);

            yield return new WaitForSeconds(bulletDelay);
        }
    }

    private void setEnemyOrientation(GameObject enemy, EnemyCollider collider)
    {
        int orientation = Random.Range(0, 4);
        if (orientation == 0 || orientation == 1)
        {
            if (orientation == 0)
            {
                enemy.transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            }
            else if (orientation == 1)
            {
                enemy.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
            }

            enemy.transform.position = new Vector3(Random.Range(-horizontalXRange, horizontalXRange), Random.Range(-horizontalYRange, horizontalYRange), 0);
            rotateEnemy(enemy.transform.GetChild(0), collider, true);
        }
        else if (orientation == 2 || orientation == 3)
        {
            if (orientation == 2)
            {
                enemy.transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            }
            else if (orientation == 3)
            {
                enemy.transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
            }

            enemy.transform.position = new Vector3(Random.Range(-verticalXRange, verticalXRange), Random.Range(-verticalYRange, verticalYRange), 0);
            rotateEnemy(enemy.transform.GetChild(0), collider, false);
        }
    }

    private void rotateEnemy(Transform enemyTransform, EnemyCollider collider, bool isHorizontal)
    {
        Vector3 perpendicular = player.transform.position - enemyTransform.position;
        if (isHorizontal)
            perpendicular.x = 0;
        else
            perpendicular.y = 0;

        enemyTransform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular.normalized);
        if (collider.isFixedRotation)
        {
            collider.setRotation.Invoke();
        }
    }
}
