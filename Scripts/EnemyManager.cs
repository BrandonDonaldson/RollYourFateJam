using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    private List<GameObject> enemyList;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initialization
        System.Random rnd = new System.Random();
        enemyList = new List<GameObject>();
        rangeSize = mapSize / 4;
        eRange1 = rangeSize;
        eRange2 = rangeSize * 2;
        eRange3 = rangeSize * 3;

        //RandomX Assingment
        enemyX1 = (float)(rnd.Next(0,(int) rangeSize)) + eRange1;
        enemyX2 = (float)(rnd.Next(0, (int)rangeSize)) + eRange2;
        enemyX3 = (float)(rnd.Next(0, (int)rangeSize))   + eRange3;

        //Random Spawning
        //Enemies in Range 1
        CreateEnemy(enemyX1);

        //Enemies in Range 2
        for(int i = 0; i<2; i++)
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

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject enemy in enemyList)
        {
            enemyScript = enemy.GetComponent<Enemy>();
            if (enemy.transform.position.x < eRange2)
            {
                enemyScript.Walk(eRange1,eRange2);
            }
            else if (enemy.transform.position.x < eRange3)
            {
                enemyScript.Walk(eRange2, eRange3);
            }
            else if (enemy.transform.position.x < mapSize)
            {
                enemyScript.Walk(eRange3, mapSize);   
            }
        }
    }

    void CreateEnemy(float xValue)
    {
        enemyList.Add(
           Instantiate(
               enemyPrefab,
               new Vector2(xValue, startingY),
               Quaternion.identity
           )
       );
    }
}
