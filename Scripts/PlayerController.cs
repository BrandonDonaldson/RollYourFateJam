using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header("Player & Environment Stats")]
    [SerializeField] private float gravityStrength = -9.6f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 25f;
    [SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private float attackActionCooldown = 0.25f;
    [SerializeField] private float staminaRechargeCooldown = 0.5f;
    [SerializeField] private float staminaRegenerationRate = 0.1f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaJump = 20f;
    [SerializeField] private float staminaPunch = 15f;
    [SerializeField] private float staminaAirPunch = 30f;
    [SerializeField] private float staminaKick = 15f;
    [SerializeField] private float staminaAirKick = 30f;

    [Header("Player Objects")]
    [SerializeField] private CollisionTrigger groundDetection;
    [SerializeField] private Rigidbody2D playerPhysics;
    [SerializeField] private GameObject[] attackOrigin;
    [SerializeField] private GameObject[] attackPrefabs;
    [SerializeField] private Image staminaBarFill;

    [Header("For Viewing")]
    [SerializeField] private bool isGrounded = false;

    [SerializeField] private Vector2 movementVel;
    [SerializeField] private Vector2 environmentVel;
    [SerializeField] private Vector2 naturalVel;
    [SerializeField] private Vector2 velocity;

    [SerializeField] private KeyCode lastInput;
    [SerializeField] private float timeSinceLastInput = 0.0f;
    [SerializeField] private float timeSinceAttack = 0.0f;
    [SerializeField] private float timeSinceStaminaUse = 0.0f;
    [SerializeField] private float stamina = 0;

    [SerializeField] private GameObject currentAttackObject;

    [SerializeField] private bool hasUsedAirAttack = false;

    private enum dirData
    {
        Right,
        Left
    };

    [SerializeField] private dirData dir;

    private void Start()
    {
        stamina = maxStamina;
    }

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
        staminaBarFill.fillAmount = stamina / maxStamina;

        timeSinceAttack += Time.deltaTime;
        timeSinceLastInput += Time.deltaTime;
        timeSinceStaminaUse+= Time.deltaTime;

        if (timeSinceStaminaUse >= staminaRechargeCooldown)
        {
            stamina += staminaRegenerationRate;

            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
        }

        if (stamina < 0)
        {
            stamina = 0;
        }

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

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (isGrounded)
                    {
                        if (stamina >= staminaPunch)
                        {
                            //PUNCH triggered here!
                            stamina -= staminaPunch;
                            print("Punching...");
                            lastInput = KeyCode.Mouse0;
                            timeSinceAttack = 0;
                            timeSinceStaminaUse = 0;

                            if (isGrounded)
                            {
                                movementVel.x = 0;
                            }
                            if (dir == dirData.Right)
                            {
                                currentAttackObject = Instantiate(attackPrefabs[0], attackOrigin[0].transform.position, attackOrigin[0].transform.rotation, attackOrigin[0].transform);
                            }
                            else if (dir == dirData.Left)
                            {
                                currentAttackObject = Instantiate(attackPrefabs[0], attackOrigin[1].transform.position, attackOrigin[1].transform.rotation, attackOrigin[1].transform);
                            }
                        }
                    }
                    else
                    {
                        if (!hasUsedAirAttack && stamina >= staminaAirPunch)
                        {
                            //AIR PUNCH triggered here!
                            stamina -= staminaAirPunch;
                            hasUsedAirAttack = true;
                            print("Air Punching...");
                            lastInput = KeyCode.Mouse0;
                            timeSinceAttack = 0;
                            timeSinceStaminaUse = 0;

                            if (isGrounded)
                            {
                                movementVel.x = 0;
                            }
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
                    if (isGrounded)
                    {
                        if (stamina >= staminaAirKick)
                        {
                            //KICK triggered here!
                            stamina -= staminaKick;
                            print("Kicking...");
                            lastInput = KeyCode.Mouse1;
                            timeSinceAttack = 0;
                            timeSinceStaminaUse = 0;

                            if (isGrounded)
                            {
                                movementVel.x = 0;
                            }
                            if (dir == dirData.Right)
                            {
                                currentAttackObject = Instantiate(attackPrefabs[1], attackOrigin[2].transform.position, attackOrigin[2].transform.rotation, attackOrigin[2].transform);
                            }
                            else if (dir == dirData.Left)
                            {
                                currentAttackObject = Instantiate(attackPrefabs[1], attackOrigin[3].transform.position, attackOrigin[3].transform.rotation, attackOrigin[3].transform);
                            }
                        }
                    }
                    else
                    {
                        if (!hasUsedAirAttack && stamina >= staminaAirKick)
                        {
                            //AIR KICK triggered here!
                            stamina -= staminaAirKick;
                            hasUsedAirAttack = true;
                            print("Air Kicking...");
                            lastInput = KeyCode.Mouse1;
                            timeSinceAttack = 0;
                            timeSinceStaminaUse = 0;

                            if (isGrounded)
                            {
                                movementVel.x = 0;
                            }
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


            if (Input.GetKey(KeyCode.Space) && isGrounded && stamina >= staminaJump)
            {
                //JUMP triggered here!
                stamina -= staminaJump;
                movementVel.y = jumpForce;
                lastInput = KeyCode.Space;
                timeSinceLastInput = 0;
                timeSinceStaminaUse = 0;
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
