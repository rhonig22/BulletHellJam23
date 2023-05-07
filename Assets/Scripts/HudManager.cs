using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class HudManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        DataManager.Instance.Restart();
        DataManager.Instance.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + DataManager.Instance.GetScore();
    }
}
