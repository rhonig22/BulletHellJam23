using LootLocker.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public static int killCount { get; private set; } = 0;
    public static bool dataRetrieved { get; private set; } = false;
    public static UserData playerData { get; private set; }
    public static bool timerStart = false;

    public static int currentLevel = 1;
    public static float timer { get; private set; } = 0;
    public static float effectsVolume { get; private set; } = 1f;
    public static UnityEvent gameOver = new UnityEvent();
    public static UnityEvent userDataRetrieved = new UnityEvent();
    public static readonly int killValue = 100;
    private readonly string leaderboardID = "bullethell_leaderboard";

    // Start is called before the first frame update
    void Start()
    {
        // Set up singleton
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartLootLockerSession();
        userDataRetrieved.AddListener(() => { dataRetrieved = true; });
    }

    private void Update()
    {
        if (timerStart)
        {
            timer += Time.deltaTime;
        }
    }

    public void IncreaseKill()
    {
        killCount++;
    }

    public void SetEffectsVolume(float vol)
    {
        effectsVolume = vol;
    }

    public void StartTimer()
    {
        timerStart = true;
    }

    public void Restart()
    {
        killCount = 0;
        timerStart = false;
        timer = 0;
    }

    public int GetScore()
    {
        return (int)timer + killCount * killValue;
    }

    private void StartLootLockerSession()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session");
                return;
            }

            SetUserData("", response.player_identifier);
            LootLockerSDKManager.GetPlayerName((response) =>
            {
                if (!response.success)
                {
                    return;
                }

                playerData.UserName = response.name;
                userDataRetrieved.Invoke();
                Debug.Log("successfully started LootLocker session");
            });

            Debug.Log("successfully started LootLocker session");
        });
    }

    public void SubmitLootLockerScore(int score)
    {
        LootLockerSDKManager.SubmitScore(playerData.UserId, score, leaderboardID, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successful");
            }
            else
            {
                Debug.Log("failed: " + response.Error);
            }
        });
    }

    public void SetUserName(string username, Action<string> callback)
    {
        LootLockerSDKManager.SetPlayerName(username, (response) =>
        {
            if (!response.success)
            {
                callback(null);
                return;
            }

            playerData.UserName = username;
            callback(username);
            Debug.Log("successfully started LootLocker session");
        });
    }

    private void SetUserData(string name, string id)
    {
        playerData = new UserData
        {
            UserName = name,
            UserId = id
        };
    }

    public void GetHighScores(int count, Action<LootLockerLeaderboardMember[]> callback)
    {
        LootLockerSDKManager.GetScoreList(leaderboardID, count, 0, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successful");
                callback(response.items);
            }
            else
            {
                Debug.Log("failed: " + response.Error);
            }
        });
    }
}
