using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Transform player;
    public Transform goal;

    public Image progressBarFill;
    private float startDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startDistance = Vector2.Distance(player.position, goal.position);
    }

    // Update is called once per frame
    void Update()
    {
        float currentDistance = Vector2.Distance(player.position, goal.position);

        float progress = 1f - (currentDistance / startDistance);
        progress = Mathf.Clamp01(progress);
        progressBarFill.fillAmount = progress;
    }
}
