using UnityEngine;
using TMPro;

public class PointManager : MonoBehaviour
{
    public static PointManager Instance;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    private int enemyScore;
    private float comboScore;
    private float totalScore;



    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this; 
        }
        else
        { 
            Destroy(gameObject); 
        }

        totalScore = 0;
    }

    public void UpdateScore(int points, int combo)
    {
        Debug.Log("Enemy score: " + points + " " + (1.0f + (float)(combo / 10.0f)));
        totalScore += points * (1.0f + (float)(combo/10.0f));
        Debug.Log(totalScore);
        scoreText.text = totalScore.ToString();
    }
}
