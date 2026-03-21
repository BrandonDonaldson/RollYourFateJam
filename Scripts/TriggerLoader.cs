using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerLoader : MonoBehaviour
{
    public string scene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(scene);
        }
    }
}
