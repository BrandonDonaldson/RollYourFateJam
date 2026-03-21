using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool enemyHit = false;
    private List<GameObject> currentlyTouching = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!currentlyTouching.Contains(collision.gameObject))
        {
            if (collision.gameObject.tag == "Enemy")
            {
                enemyHit = true;
            }
            currentlyTouching.Add(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!currentlyTouching.Contains(collision.gameObject))
        {
            if (collision.gameObject.tag == "Enemy")
            {
                enemyHit = true;
            }
            currentlyTouching.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentlyTouching.Contains(collision.gameObject))
        {
            currentlyTouching.Remove(collision.gameObject);
        }
    }
}
