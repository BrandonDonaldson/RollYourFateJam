using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    private List<GameObject> enemyList;
    private List<GameObject> destructionList;
    [SerializeField]
    private float startingY = 0;
    [SerializeField]
    private float mapSize = 200;
    private float rangeSize;
    private float eRange1;
    private float eRange2;
    private float eRange3;
    private float enemyX1;
    private float enemyX2;
    private float enemyX3;
    private Enemy enemyScript;
    private PlayerController playerScript;
    [SerializeField]
    private GameObject player;
    private float timer;
    System.Random rnd = new System.Random();
    [SerializeField]

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {//Initialization
        timer = 0f;
        playerScript = player.GetComponent<PlayerController>();
        enemyList = new List<GameObject>();
        destructionList = new List<GameObject>();
        enemyList.Add(
             Instantiate(
               enemyPrefab,
               new Vector2(0, 1),
               Quaternion.identity   //TEST ENEMY
           ));
        rangeSize = mapSize / 4;
        eRange1 = rangeSize;
        eRange2 = rangeSize * 2;
        eRange3 = rangeSize * 3;

        //RandomX Assingment
        enemyX1 = (float)(rnd.Next(0, (int)rangeSize)) + eRange1;
        enemyX2 = (float)(rnd.Next(0, (int)rangeSize)) + eRange2;
        enemyX3 = (float)(rnd.Next(0, (int)rangeSize)) + eRange3;

        //Random Spawning
        //Enemies in Range 1
        CreateEnemy(enemyX1);

        //Enemies in Range 2
        for (int i = 0; i < 2; i++)
        {
            CreateEnemy(enemyX2);
            enemyX2 = (float)(rnd.Next(0, (int)rangeSize)) + eRange2;
        }
        //Enemies in Range 3
        for (int i = 0; i < 3; i++)
        {
            CreateEnemy(enemyX3);
            enemyX3 = (float)(rnd.Next(0, (int)rangeSize)) + eRange3;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Update");
        //Update for every enemy
        foreach (GameObject enemy in enemyList)
        {
            //Gets Script
            enemyScript = enemy.GetComponent<Enemy>();
            enemyScript.indicator.GetComponent<SpriteRenderer>().enabled = false;
            //Attack timer
            if (enemyScript.IsAttacking && enemyScript.attackTimer > .25f)
            {
                enemyScript.IsAttacking = false;
                
            }

            //Enemy is dead
            if(enemyScript.HP <= 0)
            {
                destructionList.Add(enemy);
                PointManager.Instance.UpdateScore(100,playerScript.CurrentCombo);

            }
            if(Math.Abs(player.transform.position.x - enemy.transform.position.x) < .5f)
            {
                enemyScript.Stop();
            }
            //Check if player is in range to follow
            else if (Math.Abs(player.transform.position.x - enemy.transform.position.x) <= 4.0f && Math.Abs(player.transform.position.x - enemy.transform.position.x) > .5f)
            {
                enemyScript.indicator.GetComponent<SpriteRenderer>().enabled = true;
                enemyScript.Restart();
                enemyScript.Follow(player.transform.position);
            }

            //Test Enemies
            else if (enemy.transform.position.x < 10)
            {
                enemyScript.indicator.GetComponent<SpriteRenderer>().enabled = false;
                enemyScript.Restart();
                enemyScript.Walk(-4.0f, 4.0f);
            }

            //Moves among bounding boxes
            else if (enemy.transform.position.x < eRange2 && Math.Abs(player.transform.position.x - enemy.transform.position.x) > .5f)
            {
                enemyScript.indicator.GetComponent<SpriteRenderer>().enabled = false;
                enemyScript.Restart();
                enemyScript.Walk(eRange1, eRange2);
            }
            else if (enemy.transform.position.x < eRange3 && Math.Abs(player.transform.position.x - enemy.transform.position.x) > .5f)
            {
                enemyScript.indicator.GetComponent<SpriteRenderer>().enabled = false;
                enemyScript.Restart();
                enemyScript.Walk(eRange2, eRange3);
            }
            else if (enemy.transform.position.x < mapSize && Math.Abs(player.transform.position.x - enemy.transform.position.x) > .5f)
            {
                enemyScript.indicator.GetComponent<SpriteRenderer>().enabled = false;
                enemyScript.Restart();
                enemyScript.Walk(eRange3, mapSize);
            }

            // Debug.Log(timer);
            //check if in range to attack
            if (Math.Abs(player.transform.position.x - enemy.transform.position.x) <= 1.0f)
            {
                enemyScript.attackTimer += Time.deltaTime;
                //attack after 1 second in range
                if (enemyScript.attackTimer > 1.0f)
                {
                    Debug.Log("Attacking");
                    enemyScript.Attack(player.transform.position);
                    enemyScript.attackTimer = 0f;
                }
            }

            //reset timer once player leaves range
            else if (Math.Abs(player.transform.position.x - enemy.transform.position.x) > 1.0f)
            {
                enemyScript.attackTimer = 0;
            }
        }

        //Destroy "Dead" enemies
        foreach (GameObject enemy in destructionList)
        {
            enemyScript = enemy.GetComponent<Enemy>();
            enemyList.Remove(enemy); //remove from active list
            Destroy(enemyScript.attackObj); //Deletes current attackObj if active while enemy is "dead"
            Destroy(enemy); //Delete enemy
        }
        destructionList.Clear();
    }

    /// <summary>
    /// Instantiates an enemy at a specific x coordinate
    /// </summary>
    /// <param name="xValue">the randomized x coordinate to spawn the enemy at</param>
    void CreateEnemy(float xValue)
    {
        //append to enemyList
        enemyList.Add(
           Instantiate(
               enemyPrefab,
               new Vector2(xValue, startingY),
               Quaternion.identity
           )
       );
    }
}
