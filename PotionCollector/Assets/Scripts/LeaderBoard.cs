using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI[] id, score;
    public void ShowData(List<ScoreData> sd)
    {
        for(int i = 0; i < sd.Count; i++)
        {
            id[i].text = sd[i].id;
            score[i].text = sd[i].score;
        }
        gameObject.SetActive(true);
    }
}
