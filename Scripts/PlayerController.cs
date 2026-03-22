using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{

    [Header("Player & Environment Stats")]
    [SerializeField] private float gravityStrength = -9.6f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float jumpForce = 25f;
    [SerializeField] private float dashForce = 25f;
    [SerializeField] private float dashDecay = .1f;
    [SerializeField] private float kbDecay = 0.85f;
    [SerializeField] private float kbMultiplier = 25;
    [SerializeField] private float dashInputWindow = .1f;
    [SerializeField] private float jumpCooldown = 0.1f;
    [SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private float iFrameTime = .5f;
    [SerializeField] private float actionCooldown = 0.25f;
    [SerializeField] private float staminaRechargeCooldown = 0.5f;
    [SerializeField] private float staminaRegenerationRate = 0.1f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaJump = 20f;
    [SerializeField] private float staminaDash = 50f;
    [SerializeField] private float staminaPunch = 15f;
    [SerializeField] private float staminaAirPunch = 30f;
    [SerializeField] private float staminaKick = 15f;
    [SerializeField] private float staminaAirKick = 30f;
    [SerializeField] private float maxHealth = 100f;

    [Header("Player Objects")]
    [SerializeField] private CollisionTrigger groundDetection;
    [SerializeField] private Rigidbody2D playerPhysics;
    [SerializeField] private GameObject[] attackOrigin;
    [SerializeField] private GameObject[] attackPrefabs;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Image staminaBarFill;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeReference] private AnimatedObject playerAnim;

    [Header("For Viewing")]
    [SerializeField] private bool isGrounded = false;

    [SerializeField] private Vector2 movementVel;
    [SerializeField] private Vector2 additionalVel;
    [SerializeField] private Vector2 kbVel;
    [SerializeField] private Vector2 velocity;

    [SerializeField] private KeyCode lastDashInput;
    [SerializeField] private float timeSinceDashInput = 0.0f;
    [SerializeField] private float timeSinceAttack = 0.0f;
    [SerializeField] private float timeSinceStaminaUse = 0.0f;
    [SerializeField] private float timeSinceJump = 0.0f;
    [SerializeField] private float timeSinceDash = 0.0f;
    [SerializeField] private float timeSinceBeingAttacked = 0.0f;
    [SerializeField] private float stamina = 0;
    [SerializeField] private int currentCombo = 0;
    public int CurrentCombo
    {
        get { return currentCombo; }
    }
    [SerializeField] private float health = 0;

    [SerializeField] private GameObject currentAttackObject;

    [SerializeField] private bool hasUsedAirAttack = false;
    [SerializeField] private bool comboGainedFromCurrentAttack = false;

    private enum dirData
    {
        Right,
        Left
    };

    [SerializeField] private dirData dir;

    //Called on Initialization
    private void Start()
    {
        stamina = maxStamina;
        health = maxHealth;
        timeSinceAttack = 20;
    }

    //Called every Physics Frame
    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            movementVel.y += gravityStrength;
        }

        additionalVel.x *= dashDecay;
        kbVel.x *= kbDecay;
        kbVel.y *= kbDecay;

        if (Mathf.Abs(movementVel.x) > Mathf.Abs(additionalVel.x))
        {
            additionalVel.x = 0;
        }

        velocity = movementVel + additionalVel + kbVel;
        playerPhysics.linearVelocity = velocity;
    }

    //Called every frame
    private void Update()
    {
        //UI Updates
        healthBarFill.fillAmount = health / maxHealth;
        staminaBarFill.fillAmount = stamina / maxStamina;
        comboText.text = currentCombo.ToString();

        Debug.Log(isGrounded);

        //Animation Check
        if (timeSinceAttack > actionCooldown)
        {
            if (playerPhysics.linearVelocityY < 0 && movementVel.y < 0 && !isGrounded)
            {
                playerAnim.PlayAnim(3);
            }
            else
            {
                if (movementVel.y != 0 && !isGrounded)
                {
                    playerAnim.PlayAnim(2);
                }
                else
                {
                    if (movementVel.x > movementSpeed * .5f && playerPhysics.linearVelocityX > movementSpeed * 0.5f || movementVel.x < movementSpeed * -0.5f && playerPhysics.linearVelocityX < movementSpeed * -0.5f)
                    {
                        playerAnim.PlayAnim(1);
                    }
                    else
                    {
                        playerAnim.PlayAnim(0);
                    }
                }
            }
        }
        

        //Combo check
        if (currentAttackObject != null && !comboGainedFromCurrentAttack)
        {
            print("Combo Succeeded");
            PlayerAttack cur = currentAttackObject.GetComponent<ChildPointer>().child.GetComponent<PlayerAttack>();
            if (cur.enemyHit)
            {
                comboGainedFromCurrentAttack = true;
                currentCombo++;
            }
        }

        //Timers (Increment w/ frame time (delta time))
        timeSinceJump += Time.deltaTime;
        timeSinceDash += Time.deltaTime;
        timeSinceAttack += Time.deltaTime;
        timeSinceDashInput += Time.deltaTime;
        timeSinceStaminaUse+= Time.deltaTime;
        timeSinceBeingAttacked += Time.deltaTime;

        if (timeSinceStaminaUse >= staminaRechargeCooldown)
        {
            //Multiply by delta time so regen speed is frame-independent
            stamina += staminaRegenerationRate*Time.deltaTime;

            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
        }

        //Check for negative stamina
        if (stamina < 0)
        {
            stamina = 0;
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            health = 0;
            SceneManager.LoadScene("DeadScene");
            return;
        }

        //Input
        if (lastDashInput == KeyCode.D)
        {
            dir = dirData.Right;
            playerSprite.flipX = false;
        }
        else if (lastDashInput == KeyCode.A)
        {
            dir = dirData.Left;
            playerSprite.flipX = true;
        }

        if (isGrounded)
        {
            hasUsedAirAttack = false;
        }

        if (timeSinceAttack < actionCooldown)
        {
            return;
        }
        else
        {
            if (currentAttackObject != null)
            {
                if (!comboGainedFromCurrentAttack)
                {
                    currentCombo = 0;
                }
                Destroy(currentAttackObject.gameObject);
                comboGainedFromCurrentAttack = false;
            }

            if (timeSinceAttack > attackCooldown && Input.GetKey(KeyCode.Mouse0) || timeSinceAttack > attackCooldown && Input.GetKey(KeyCode.Mouse1))
            {

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (isGrounded)
                    {
                        if (stamina >= staminaPunch)
                        {
                            //PUNCH triggered here!
                            stamina -= staminaPunch;
                            print("Punching...");
                            lastDashInput = KeyCode.Mouse0;
                            timeSinceAttack = 0;
                            timeSinceStaminaUse = 0;
                            playerAnim.PlayAnim(4);

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
                            lastDashInput = KeyCode.Mouse0;
                            timeSinceAttack = 0;
                            timeSinceStaminaUse = 0;
                            playerAnim.PlayAnim(5);

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
                else if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    if (isGrounded)
                    {
                        if (stamina >= staminaAirKick)
                        {
                            //KICK triggered here!
                            stamina -= staminaKick;
                            print("Kicking...");
                            lastDashInput = KeyCode.Mouse1;
                            timeSinceAttack = 0;
                            timeSinceStaminaUse = 0;
                            playerAnim.PlayAnim(6);

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
                            lastDashInput = KeyCode.Mouse1;
                            timeSinceAttack = 0;
                            timeSinceStaminaUse = 0;
                            playerAnim.PlayAnim(7);

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

            if (timeSinceAttack < actionCooldown || timeSinceDash < actionCooldown)
            {
                return;
            }

            #region Dash Mechanic

            if (Input.GetKeyDown(KeyCode.D))
            {
                if (lastDashInput == KeyCode.D && timeSinceDashInput <= dashInputWindow)
                {
                    if (stamina >= staminaDash)
                    {
                        //Initiate Right Dash
                        movementVel.x = 0;
                        print("Dashing Right...");
                        timeSinceDash = 0;
                        stamina -= staminaDash;
                        additionalVel.x = dashForce;
                        timeSinceStaminaUse = 0;
                    }
                }
                lastDashInput = KeyCode.D;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (lastDashInput == KeyCode.A && timeSinceDashInput <= dashInputWindow)
                {
                    if (stamina >= staminaDash)
                    {
                        //Initiate Left Dash
                        movementVel.x = 0;
                        print("Dashing Left...");
                        timeSinceDash = 0;
                        stamina -= staminaDash;
                        additionalVel.x = -dashForce;
                        timeSinceStaminaUse = 0;
                    }
                }
                lastDashInput = KeyCode.A;
            }

            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
            {
                timeSinceDashInput = 0.0f;
            }

            #endregion

            if (Input.GetKeyDown(KeyCode.A))
            {
                lastDashInput = KeyCode.A;
            }

            if (Input.GetKey(KeyCode.D) && isGrounded)
            {
                movementVel.x = movementSpeed;
                lastDashInput = KeyCode.D;
            }
            if (Input.GetKey(KeyCode.A) && isGrounded)
            {
                movementVel.x = -movementSpeed;
                lastDashInput = KeyCode.A;
            }

            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A) && isGrounded || !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && isGrounded)
            {
                movementVel.x = 0.0f;
            }


            if (Input.GetKeyDown(KeyCode.W) && isGrounded && stamina >= staminaJump && timeSinceJump >= jumpCooldown)
            {
                //JUMP triggered here!
                lastDashInput = KeyCode.W;
                timeSinceJump = 0;
                stamina -= staminaJump;
                movementVel.y = jumpForce;
                timeSinceStaminaUse = 0;
            }
        }
    }

    public int GetCombo()
    {
        return currentCombo;
    }

    public void Damage(float amt)
    {
        health -= amt;
        if (health < 0)
        {
            health = 0;
        }
    }


    //Called on GroundCheck collider collision
    public void GroundCollider()
    {
        for (int i = 0; i < groundDetection.currentlyTouching.Count; i++)
        {
            if (groundDetection.currentlyTouching[i].tag == "Ground" || groundDetection.currentlyTouching[i].tag == "Enemy")
            {
                if (!isGrounded)
                {
                    //Just landed on ground
                    movementVel.y = 0;
                }
                isGrounded = true;
                return;
            }
        }

        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            if (timeSinceBeingAttacked >= iFrameTime)
            {
                timeSinceBeingAttacked = 0;
                Damage(20);

                Vector2 enem = collision.gameObject.transform.position;
                Vector2 play = this.transform.position;

                Vector2 kbDir = play - enem;

                kbVel += kbDir * kbMultiplier;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            Damage(10);
            Vector2 enem = collision.gameObject.transform.position;
            Vector2 play = this.transform.position;

            float kbDir = play.x - enem.x;

            if (kbDir <= .05f && kbDir >= -.05f)
            {
                kbDir = 2.5f;
            }

            kbVel += new Vector2(kbDir,0) * kbMultiplier;
            movementVel = new Vector2(0, 0);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            Vector2 enem = collision.gameObject.transform.position;
            Vector2 play = this.transform.position;

            float kbDir = play.x - enem.x;

            if (kbDir <= .05f && kbDir >= -.05f)
            {
                kbDir = 2.5f;
            }

            kbVel += new Vector2(kbDir, 0.1f) * kbMultiplier;

            movementVel = new Vector2(0, 0);
            //Damage(25);
        }
    }

}
