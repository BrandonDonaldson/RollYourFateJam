using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private static float totalScore = 0;

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this; 
        }
        else
        { 
            Destroy(gameObject); 
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        scoreText = GameObject.Find("Score Count").GetComponent<TextMeshProUGUI>();

        if (scene.name == "Main")
        {
            totalScore = 0;
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        if (scoreText != null)
        {
            scoreText.text = totalScore.ToString();
        }
    }

    public void UpdateScore(int points, int combo)
    {
        Debug.Log("Enemy score: " + points + " " + (1.0f + (float)(combo / 10.0f)));
        totalScore += points * (1.0f + (float)(combo/10.0f));
        Debug.Log(totalScore);
        RefreshUI();
    }
}
