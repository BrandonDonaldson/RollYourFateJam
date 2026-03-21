using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector2 velocity;
    private bool left;
    private int hp;
    [SerializeField]
    private GameObject attackPrefab;
    [SerializeField]
    private List<GameObject> attackOrigins;
    public int HP
    {
        get { return hp; }
    }
    public GameObject attackObj;
    private bool isAttacking;
    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        velocity = new Vector2(.01f, 0);
        left = true;
        isAttacking = false;
        hp = 10; //default num
    }

    // Update is called once per frame
    void Update()
    {
        //Get rid of attackObj when not attacking
        if (!isAttacking && attackObj != null)
        {
            Destroy(attackObj);
        }
    }

    /// <summary>
    /// Moves the enemy within a certain range
    /// </summary>
    /// <param name="leftBound">leftmost x coordinate the enemy can reach</param>
    /// <param name="rightBound">rightmost x coordinate the enemy can reach</param>
    public void Walk(float leftBound, float rightBound)
    {
        //move when not attacking
        if (!isAttacking)
        {
            if (transform.position.x > rightBound - .5f && !left)
            {
                //Debug.Log("RightBound" + rightBound + "Xval = " + transform.transform.position.x);
                left = true;
            }
            if (transform.position.x < leftBound + .5f && left)
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
    }

    /// <summary>
    /// Follow's the player's x movement if the player is near
    /// </summary>
    /// <param name="playerPos">the player's current position</param>
    public void Follow(Vector2 playerPos)
    {
        //Debug.Log("Following!");
        //move when not attacking
        if (!isAttacking)
        {
            if (playerPos.x < transform.position.x)
            {
                left = true;
                Vector2 tempPos = transform.position;
                tempPos -= velocity / 2.0f;
                transform.position = tempPos;
            }
            else if (playerPos.x >= transform.position.x)
            {
                left = false;
                Vector2 tempPos = transform.position;
                tempPos += velocity / 2.0f;
                transform.position = tempPos;
            }
        }
    }

    /// <summary>
    /// Spawn the attack hitbox for the enemy
    /// </summary>
    /// <param name="playerPos">the player's current position</param>
    public void Attack(Vector2 playerPos)
    {
        Debug.Log("Attacking");
        if(playerPos.x < transform.position.x)
        {
            attackObj = Instantiate(attackPrefab, attackOrigins[0].transform.position, Quaternion.identity);
        }
        if (playerPos.x >= transform.position.x)
        {
            attackObj = Instantiate(attackPrefab, attackOrigins[1].transform.position, Quaternion.identity);
        }
        isAttacking = true;
    }

    /// <summary>
    /// Recognizes when a player's attack hitbox interacts with the enemy
    /// </summary>
    /// <param name="collision">attackObj for the player</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if(collision.CompareTag("Attack"))
            {
            Debug.Log("Hit!");
            hp = 0;
        }
    }
}
