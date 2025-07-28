using UnityEngine;

public class Potion : MonoBehaviour
{
    string name;
    int points=1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameEvents.OnPotionSpawned.Invoke(name, transform.position);
        Destroy(gameObject, 3);
    }

    private void OnMouseDown()
    {
        GameEvents.OnPotionCollected.Invoke(name,points,System.DateTime.Now);
        DestroyImmediate(gameObject);
    }
}
