using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector2 position;
    private Vector2 velocity;
    public bool left;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = transform.position;
        velocity = new Vector2(.01f, 0);
        left = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Walk(float leftBound, float rightBound)
    {
        position = transform.position;
        if (leftBound == 50)
        {
            //Debug.Log("XVAL = " + transform.position.x + "\nBounds = {" + leftBound + ", " + rightBound + "}");
        }
        if (position.x > rightBound-.5f && !left)
        {
            //Debug.Log("RightBound" + rightBound + "Xval = " + transform.position.x);
            left = true;    
        }
        if (position.x < leftBound + .5f && left)
        {
            left = false;
        }

        if (left)
        {
            Vector2 tempPos = transform.position;
            tempPos -= velocity;
            transform.position = tempPos;
        }
        if (!left)
        {
            Vector2 tempPos = transform.position;
            tempPos += velocity;
            transform.position = tempPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.CompareTag("Attack"))
            {
            Debug.Log("Hit!");
            Destroy(gameObject);
        }
    }
}
