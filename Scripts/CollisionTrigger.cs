using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class CollisionTrigger : MonoBehaviour
{
    public List<GameObject> currentlyTouching = new List<GameObject>();
    public UnityEvent onCollisionEnter;
    public UnityEvent onCollisionExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!currentlyTouching.Contains(collision.gameObject))
        {
            currentlyTouching.Add(collision.gameObject);
            onCollisionEnter.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!currentlyTouching.Contains(collision.gameObject))
        {
            currentlyTouching.Add(collision.gameObject);
            onCollisionEnter.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentlyTouching.Contains(collision.gameObject))
        {
            currentlyTouching.Remove(collision.gameObject);
            onCollisionExit.Invoke();
        }
    }
}
