using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Player & Environment Stats")]
    [SerializeField] private float gravityStrength = -9.6f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 25f;
    [SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private float attackActionCooldown = 0.25f;

    [Header("Player Objects")]
    [SerializeField] private CollisionTrigger groundDetection;
    [SerializeField] private Rigidbody2D playerPhysics;
    [SerializeField] private GameObject[] attackOrigin;
    [SerializeField] private GameObject[] attackPrefabs;

    [Header("For Viewing")]
    [SerializeField] private bool isGrounded = false;

    [SerializeField] private Vector2 movementVel;
    [SerializeField] private Vector2 environmentVel;
    [SerializeField] private Vector2 naturalVel;
    [SerializeField] private Vector2 velocity;

    [SerializeField] private KeyCode lastInput;
    [SerializeField] private float timeSinceLastInput = 0.0f;
    [SerializeField] private float timeSinceAttack = 0.0f;

    [SerializeField] private GameObject currentAttackObject;

    [SerializeField] private bool hasUsedAirAttack = false;

    private enum dirData
    {
        Right,
        Left
    };

    [SerializeField] private dirData dir;

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            movementVel.y += gravityStrength;
        }

        velocity = movementVel + environmentVel + naturalVel;
        playerPhysics.linearVelocity = velocity;
    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        timeSinceLastInput += Time.deltaTime;

        if (lastInput == KeyCode.D)
        {
            dir = dirData.Right;
        }
        else if (lastInput == KeyCode.A)
        {
            dir = dirData.Left;
        }

        if (isGrounded)
        {
            hasUsedAirAttack = false;
        }

        if (timeSinceAttack < attackActionCooldown)
        {
            return;
        }
        else
        {
            if (currentAttackObject != null)
            {
              Destroy(currentAttackObject.gameObject);
            }

            if (timeSinceAttack > attackCooldown && Input.GetKey(KeyCode.Mouse0) || timeSinceAttack > attackCooldown && Input.GetKey(KeyCode.Mouse1))
            {
                timeSinceAttack = 0;

                if (isGrounded)
                {
                    movementVel.x = 0;
                }

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    lastInput = KeyCode.Mouse0;
                    if (isGrounded)
                    {
                        print("Punching...");
                        if (dir == dirData.Right)
                        {
                            currentAttackObject = Instantiate(attackPrefabs[0], attackOrigin[0].transform.position, attackOrigin[0].transform.rotation, attackOrigin[0].transform);
                        }
                        else if (dir == dirData.Left)
                        {
                            currentAttackObject = Instantiate(attackPrefabs[0], attackOrigin[1].transform.position, attackOrigin[1].transform.rotation, attackOrigin[1].transform);
                        }
                    }
                    else
                    {
                        if (!hasUsedAirAttack)
                        {
                            hasUsedAirAttack = true;
                            print("Air Punching...");
                            if (dir == dirData.Right)
                            {
                                currentAttackObject = Instantiate(attackPrefabs[2], attackOrigin[0].transform.position, attackOrigin[0].transform.rotation, attackOrigin[0].transform);
                            }
                            else if (dir == dirData.Left)
                            {
                                currentAttackObject = Instantiate(attackPrefabs[2], attackOrigin[1].transform.position, attackOrigin[1].transform.rotation, attackOrigin[1].transform);
                            }
                        }
                    }
                }
                //Punch takes priority over kick if both inputs used simultaneously
                else if (Input.GetKey(KeyCode.Mouse1))
                {
                    lastInput = KeyCode.Mouse1;
                    if (isGrounded)
                    {
                        print("Kicking...");
                        if (dir == dirData.Right)
                        {
                            currentAttackObject = Instantiate(attackPrefabs[1], attackOrigin[2].transform.position, attackOrigin[2].transform.rotation, attackOrigin[2].transform);
                        }
                        else if (dir == dirData.Left)
                        {
                            currentAttackObject = Instantiate(attackPrefabs[1], attackOrigin[3].transform.position, attackOrigin[3].transform.rotation, attackOrigin[3].transform);
                        }
                    }
                    else
                    {
                        if (!hasUsedAirAttack)
                        {
                            hasUsedAirAttack = true;
                            print("Air Kicking...");
                            if (dir == dirData.Right)
                            {
                                currentAttackObject = Instantiate(attackPrefabs[3], attackOrigin[2].transform.position, attackOrigin[2].transform.rotation, attackOrigin[2].transform);
                            }
                            else if (dir == dirData.Left)
                            {
                                currentAttackObject = Instantiate(attackPrefabs[3], attackOrigin[3].transform.position, attackOrigin[3].transform.rotation, attackOrigin[3].transform);
                            }
                        }
                    }
                }
            }

            if (timeSinceAttack < attackActionCooldown)
            {
                return;
            }

            if (Input.GetKey(KeyCode.D) && isGrounded)
            {
                movementVel.x = movementSpeed;
                lastInput = KeyCode.D;
                timeSinceLastInput = 0;
            }
            if (Input.GetKey(KeyCode.A) && isGrounded)
            {
                movementVel.x = -movementSpeed;
                lastInput = KeyCode.A;
                timeSinceLastInput = 0;
            }

            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A) && isGrounded || !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && isGrounded)
            {
                movementVel.x = 0.0f;
            }


            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                movementVel.y = jumpForce;
                lastInput = KeyCode.Space;
                timeSinceLastInput = 0;
            }

            if (!Input.GetKey(KeyCode.Space) && isGrounded)
            {
                movementVel.y = 0;
            }
        }
    }

    public void GroundCollider()
    {
        for (int i = 0; i < groundDetection.currentlyTouching.Count; i++)
        {
            if (groundDetection.currentlyTouching[i].tag == "Ground")
            {
                isGrounded = true;
                return;
            }
        }

        isGrounded = false;
    }
}
