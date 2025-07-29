using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Database;
using System;
using System.Collections.Generic;

public class FirebaseDB : MonoBehaviour
{

    DatabaseReference dbRef;

    private void OnEnable()
    {
        GameEvents.OnGameEnded += GameEnded;
        GameEvents.OnLeaderBoredPressed += GetTopUsers;
    }

    private void OnDisable()
    {
        GameEvents.OnGameEnded -= GameEnded;
        GameEvents.OnLeaderBoredPressed -= GetTopUsers;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase is ready!"); 
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }
    void GameEnded(DateTime time, int score)
    {
        ScoreData scoreData = new ScoreData();
        scoreData.id = DataManager.id.ToString();
        scoreData.score = score.ToString("D3");
        scoreData.sessionStartTime = DataManager.sessionStartTime.ToString();
        scoreData.sessionEndTime = time.ToString();

        string json = JsonUtility.ToJson(scoreData);

        dbRef.Child("Scores").Child(SystemInfo.deviceUniqueIdentifier+"_"+DataManager.id.ToString()).SetRawJsonValueAsync(json);
    }

    public void GetTopUsers()
    {
        dbRef.Child("Scores")
            .OrderByChild("score")
            .LimitToLast(5)
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Error retrieving data: " + task.Exception);
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    List<ScoreData> scoreData = new List<ScoreData>();
                    foreach(DataSnapshot s in snapshot.Children)
                    {
                        ScoreData sd = new ScoreData();
                        sd.id = s.Child("id").Value.ToString();
                        sd.score = s.Child("score").Value.ToString();
                        sd.sessionStartTime = s.Child("sessionStartTime").Value.ToString();
                        sd.sessionEndTime = s.Child("sessionEndTime").Value.ToString();

                        scoreData.Add(sd);
                    }
                    scoreData.Sort((a, b) => b.score.CompareTo(a.score));

                    GameEvents.OnLeaderBoredLoaded.Invoke(scoreData);
                }
            });
    }
}

public class ScoreData
{
    public string id;
    public string score;
    public string sessionStartTime;
    public string sessionEndTime;
}