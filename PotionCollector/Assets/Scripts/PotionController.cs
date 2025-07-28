using System.Collections;
using UnityEngine;

public class PotionController : MonoBehaviour
{
    [SerializeField]
    GameObject[] potionPrefabs;
    Coroutine c;

    private void OnEnable()
    {
        GameEvents.OnGameStarted += GameStarted;
        GameEvents.OnGameEnded += GameEnded;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStarted -= GameStarted;
        GameEvents.OnGameEnded += GameEnded;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void GameStarted(System.DateTime time, int id)
    {
        StartCoroutine(SpawnPotion());
    }

    IEnumerator SpawnPotion()
    {
        float waitTime = Random.Range(1f, 3f);
        yield return new WaitForSeconds(waitTime);
        int potionIndex = Random.Range(0, potionPrefabs.Length);
        float posX = Random.Range(-4.5f, 4.5f);
        float posZ = Random.Range(-4.5f, 4.5f);
        Instantiate(potionPrefabs[potionIndex],new Vector3(posX,0.5f,posZ),Quaternion.identity);
        c = StartCoroutine(SpawnPotion());
    }

    private void GameEnded(System.DateTime time, int score)
    {
        StopCoroutine(c);
    }
}
