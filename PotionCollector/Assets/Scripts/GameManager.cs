using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI txtScore,txtTimer,txtFinalScore;
    [SerializeField]
    GameObject startMenu,gameOver;
    [SerializeField]
    LeaderBoard lb;

    string sessionIdKey="SessionIdKey";

    int timeLeft=20;

    private void OnEnable()
    {
        GameEvents.OnGameStarted += GameStarted;
        GameEvents.OnGameEnded += GameEnded;
        GameEvents.OnPotionSpawned += PostionSpawned;
        GameEvents.OnPotionCollected += PostionCollected;
        GameEvents.OnScoreUpdated += Score;
        GameEvents.OnLeaderBoredLoaded += LeaderBoredLoaded;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStarted -= GameStarted;
        GameEvents.OnGameEnded -= GameEnded;
        GameEvents.OnPotionSpawned -= PostionSpawned;
        GameEvents.OnPotionCollected -= PostionCollected;
        GameEvents.OnScoreUpdated -= Score;
        GameEvents.OnLeaderBoredLoaded -= LeaderBoredLoaded;
    }

    public void StartButtonPressed()
    {
        if (!PlayerPrefs.HasKey(sessionIdKey)) 
            PlayerPrefs.SetInt(sessionIdKey,1);

        startMenu.SetActive(false);

        int sessionId = PlayerPrefs.GetInt(sessionIdKey);
        GameEvents.OnGameStarted.Invoke(DateTime.Now,sessionId);
        PlayerPrefs.SetInt(sessionIdKey, sessionId+1);
    }

    public void RestartButtonPressed()
    {
        timeLeft = 20;
        DataManager.score = 0;
        txtScore.text = "Score : 0";
        gameOver.SetActive(false);
        StartButtonPressed();
    }

    void GameStarted(System.DateTime time, int id)
    {
        DataManager.id = id;
        DataManager.sessionStartTime = time;
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        txtTimer.text = "Time Left : " + timeLeft + "sec";
        if (timeLeft <= 0)
        {
            GameEvents.OnGameEnded.Invoke(DateTime.Now, DataManager.score);
            yield break;
        }
        timeLeft--;
        yield return new WaitForSeconds(1);
        StartCoroutine(Timer());
    }

    void GameEnded(DateTime time,int score)
    {
        gameOver.SetActive(true);
        txtFinalScore.text = "Score : " + score;
    }

    private void PostionSpawned(string potionType, Vector3 position)
    {
        
    }

    void PostionCollected(string potionType, int value, DateTime timestamp)
    {
        DataManager.score += value;
        GameEvents.OnScoreUpdated.Invoke(DataManager.score,value);
    }

    void Score(int newScore,int scoreDelta)
    {
        txtScore.text = "Score : " + newScore;
    }

    public void LBButtonPressed()
    {
        GameEvents.OnLeaderBoredPressed.Invoke();
    }

    private void LeaderBoredLoaded(List<ScoreData> sd)
    {
        lb.ShowData(sd);
    }
}

public static class DataManager
{
    public static int id;
    public static int score;
    public static DateTime sessionStartTime;
}
public static class GameEvents
{
    public static Action<DateTime,int> OnGameStarted;
    public static Action<DateTime,int> OnGameEnded;
    public static Action<int,int> OnScoreUpdated;
    public static Action<string,Vector3> OnPotionSpawned;
    public static Action<string,int,DateTime> OnPotionCollected;
    public static Action OnLeaderBoredPressed;
    public static Action<List<ScoreData>> OnLeaderBoredLoaded;
}