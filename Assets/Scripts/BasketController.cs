using UnityEngine;
using TMPro;  // For TextMeshPro UI

public class BasketController : MonoBehaviour
{
    public float speed = 10f;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject[] bars;  // assign Bar1, Bar2, Bar3 in Inspector

    private int score = 0;
    private int highScore = 0;
    private int lives;

    void Start()
    {
        lives = bars.Length;
        UpdateUI();
    }

    void Update()
    {
        // Basket movement
        float move = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(move, 0, 0);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Basket collided with: " + col.name + " (" + col.tag + ")");
        if (col.CompareTag("Apple"))
        {
            score += 100;
            if (score > highScore) highScore = score;
            UpdateUI();
            Destroy(col.gameObject);
        }
        else if (col.CompareTag("BadApple"))
        {
            MissedApple();
            Destroy(col.gameObject);
        }
    }

    void MissedApple()
    {
        // Lose one life
        if (lives > 0)
        {
            lives--;
            bars[lives].SetActive(false);
        }

        if (lives <= 0)
        {
            score = 0;
            lives = bars.Length;
            foreach (GameObject bar in bars)
                bar.SetActive(true);
        }

        UpdateUI();

        // Clear all apples on screen safely
        foreach (GameObject a in GameObject.FindGameObjectsWithTag("Apple"))
            if (a != null) Destroy(a);

        foreach (GameObject b in GameObject.FindGameObjectsWithTag("BadApple"))
            if (b != null) Destroy(b);
    }


    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (highScoreText != null)
            highScoreText.text = "High Score: " + highScore;
    }
}
