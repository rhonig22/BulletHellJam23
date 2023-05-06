using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject shieldPowerUp;
    private PlayerController playerController;
    private float shieldWaitTime = 5f;
    private float shieldCooldownTime = 10f;
    private bool isWaiting = false;
    private readonly float xBound = 5.5f;
    private readonly float yBound = 4.2f;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting && playerController.CurrentAmmo <= playerController.minThreshold)
        {
            isWaiting = true;
            StartCoroutine(ShieldPowerUpTimer());
        }
    }

    IEnumerator ShieldPowerUpTimer()
    {
        yield return new WaitForSeconds(shieldWaitTime);
        if (playerController.CurrentAmmo <= playerController.minThreshold)
        {
            SpawnShieldPowerUp();
            yield return new WaitForSeconds(shieldCooldownTime);
            isWaiting= false;
        }
    }

    private void SpawnShieldPowerUp()
    {
        GameObject shield = Instantiate(shieldPowerUp);
        shield.SetActive(true);
        shield.transform.position = new Vector3(Random.Range(-xBound, xBound), Random.Range(-yBound, yBound), 0);
    }
}
