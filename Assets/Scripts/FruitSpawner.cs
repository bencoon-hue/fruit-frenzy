using UnityEngine;

[System.Serializable]
public class FruitType
{
    public GameObject prefab;       // The fruit prefab (apple, banana, etc.)
    [Range(0f, 1f)]
    public float spawnChance = 0.25f;  // Probability weight for spawning this fruit
    public bool isBadFruit = false;    // Is this a bad fruit?
}

public class FruitSpawner : MonoBehaviour
{
    public FruitType[] fruits;          // List of possible fruits
    public float spawnInterval = 1.0f;  // Seconds between fruits

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnFruit();
        }
    }

    void SpawnFruit()
    {
        if (fruits == null || fruits.Length == 0)
        {
            Debug.LogWarning("⚠️ No fruits assigned in FruitSpawner!");
            return;
        }

        // Pick a random fruit based on spawnChance weights
        float totalWeight = 0f;
        foreach (var f in fruits)
            totalWeight += f.spawnChance;

        float randomValue = Random.Range(0, totalWeight);
        float cumulative = 0f;
        FruitType selectedFruit = fruits[0];

        foreach (var f in fruits)
        {
            cumulative += f.spawnChance;
            if (randomValue <= cumulative)
            {
                selectedFruit = f;
                break;
            }
        }

        if (selectedFruit.prefab == null)
        {
            Debug.LogWarning("⚠️ Missing prefab in FruitSpawner entry!");
            return;
        }

        Instantiate(selectedFruit.prefab, transform.position, Quaternion.identity);
        Debug.Log($"Spawned {(selectedFruit.isBadFruit ? "Bad" : "Good")} {selectedFruit.prefab.name}");
    }
}
