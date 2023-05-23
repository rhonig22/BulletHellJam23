using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUxManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeScore;
    [SerializeField] TextMeshProUGUI killsScore;
    [SerializeField] TextMeshProUGUI bonusScore;
    [SerializeField] TextMeshProUGUI totalScore;
    [SerializeField] GameObject ViewName;
    [SerializeField] GameObject EditName;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TMP_InputField playerNameInput;
    private readonly int maxLength = 12;
    private float waitTime = .5f;

    // Start is called before the first frame update
    void Start()
    {
        SetCurrentName();
        StartCoroutine(LoadScores());
    }

    private IEnumerator LoadScores()
    {
        yield return new WaitForSeconds(waitTime);
        timeScore.text = (int)DataManager.timer + " x " + DataManager.timeValue;
        yield return new WaitForSeconds(waitTime);
        killsScore.text = DataManager.killCount + " x " + DataManager.killValue;
        yield return new WaitForSeconds(waitTime);
        bonusScore.text = DataManager.bonusCount + " x " + DataManager.bonusValue;
        yield return new WaitForSeconds(waitTime);
        totalScore.text = "" + DataManager.Instance.GetScore();
        DataManager.Instance.SubmitLootLockerScore(DataManager.Instance.GetScore());
    }

    public void EditNameClicked()
    {
        ViewName.SetActive(false);
        EditName.SetActive(true);
    }

    private void ShowName()
    {
        ViewName.SetActive(true);
        EditName.SetActive(false);
    }

    public void SubmitNameClicked()
    {
        string newName = playerNameInput.text;
        if (newName == null || newName.Length > maxLength)
            newName = newName.Substring(0, maxLength);

        if (newName != DataManager.playerData.UserName)
        {
            DataManager.Instance.SetUserName(newName, (string name) =>
            {
                if (name != null)
                {
                    SetCurrentName();
                    ShowName();
                }
            });
        }
        else
        {
            ShowName();
        }
    }

    private void SetCurrentName()
    {
        string newName = DataManager.playerData.UserName;
        bool submit = false;
        if (newName == null || newName == string.Empty)
        {
            newName = "Player" + Random.Range(10000, 100000);
            submit = true;
        }

        playerNameInput.text = newName;
        nameText.text = newName;
        if (submit)
            SubmitNameClicked();
    }

}
