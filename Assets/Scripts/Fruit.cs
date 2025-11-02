using UnityEngine;

public class Fruit : MonoBehaviour
{
    public float bottomY = -6.5f;
    private GameManager gm;

    void Start()
    {
        gm = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        if (transform.position.y < bottomY)
        {
            if (CompareTag("BadFruit"))
                gm.LoseLife();
            else
                gm.MissedFruit();

            Destroy(gameObject);
        }
    }
}

