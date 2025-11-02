using UnityEngine;

public class BadApple : MonoBehaviour
{
    public int lifePenalty = 1; // how many lives to remove

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Basket"))
        {
            GameManager.Instance.LoseLife();
            Debug.Log("Collected a bad apple! -1 life 💀");
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
