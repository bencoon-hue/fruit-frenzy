using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    [Header("Basket & Lives")]
    public GameObject basket; // Basket object that holds life bars

    [Header("Spawner Reference")]
    public FruitSpawner spawner; // Assign your FruitSpawner in Inspector

    private int score = 0;
    private int highScore = 0;
    private int lives = 3;
    private GameObject[] bars;

    // Combo and Frenzy system
    private string lastFruitType = "";
    private int comboCount = 0;
    private bool frenzyActive = false;
    [SerializeField] private float frenzyDuration = 10f; // seconds
    [SerializeField] private int frenzyScoreThreshold = 1000; // activates at 1000 pts

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        SetupBars();
        UpdateUI();
    }

    void SetupBars()
    {
        if (basket != null)
        {
            bars = new GameObject[Mathf.Min(3, basket.transform.childCount)];
            for (int i = 0; i < bars.Length; i++)
                bars[i] = basket.transform.GetChild(i).gameObject;
        }
    }

    // ------------------ SCORING ------------------
    public void AddScore(string fruitType, int pts)
    {
        // Combo scoring logic
        if (fruitType == lastFruitType)
        {
            comboCount++;
            if (comboCount >= 3)
            {
                pts *= 2; // double points for combo
                comboCount = 0;
                Debug.Log("🔥 Combo achieved! Double points!");
            }
        }
        else
        {
            comboCount = 1;
            lastFruitType = fruitType;
        }

        // Add to score
        score += pts;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        // Trigger frenzy mode at threshold
        if (!frenzyActive && score >= frenzyScoreThreshold)
            StartCoroutine(ActivateFrenzy());

        UpdateUI();
    }

    // ------------------ LIFE SYSTEM ------------------
    public void LoseLife()
    {
        lives = Mathf.Max(0, lives - 1);
        RemoveBar();
        UpdateUI();
        if (lives <= 0)
            StartCoroutine(ResetAfterDelay(1f));
    }

    public void MissedFruit()
    {
        ClearAllFruits();
        lives = Mathf.Max(0, lives - 1);
        RemoveBar();
        UpdateUI();
        if (lives <= 0)
            StartCoroutine(ResetAfterDelay(1f));
    }

    void RemoveBar()
    {
        if (bars == null || bars.Length == 0) return;
        for (int i = bars.Length - 1; i >= 0; i--)
        {
            if (bars[i].activeSelf)
            {
                bars[i].SetActive(false);
                return;
            }
        }
    }

    void RestoreBars()
    {
        if (bars == null) return;
        foreach (GameObject b in bars) b.SetActive(true);
    }

    // ------------------ CLEANUP ------------------
    void ClearAllFruits()
    {
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
        GameObject[] badFruits = GameObject.FindGameObjectsWithTag("BadFruit");
        foreach (var f in fruits) Destroy(f);
        foreach (var bf in badFruits) Destroy(bf);
    }

    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetGame();
    }

    void ResetGame()
    {
        ClearAllFruits();
        score = 0;
        lives = 3;
        comboCount = 0;
        frenzyActive = false;
        RestoreBars();
        UpdateUI();
    }

    // ------------------ UI ------------------
    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (highScoreText != null) highScoreText.text = "High Score: " + highScore;
    }

    // ------------------ FRENZY MODE ------------------
    IEnumerator ActivateFrenzy()
    {
        frenzyActive = true;
        Debug.Log("🍒 FRUIT FRENZY MODE ACTIVATED!");
        if (spawner != null)
            spawner.spawnInterval /= 3f; // triple the spawn rate

        yield return new WaitForSeconds(frenzyDuration);

        if (spawner != null)
            spawner.spawnInterval *= 3f; // restore
        frenzyActive = false;
        Debug.Log("🍒 FRUIT FRENZY MODE ENDED!");
    }

    // ------------------ ACCESSORS ------------------
    public int GetScore() => score;
}
