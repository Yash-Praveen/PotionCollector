using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionDetails", menuName = "Scriptable Objects/PotionDetails")]
public class PotionDetailsScirptable : ScriptableObject
{
    public List<PotionData> potions = new List<PotionData>();
}

[System.Serializable]
public class PotionData
{
    public string name;
    public int potency;
    public Sprite icon;
    public string description;
}