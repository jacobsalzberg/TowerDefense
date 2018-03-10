﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField]
    private int target = 0;
    [SerializeField]
    private Transform exitPoint; //transform is a location
    [SerializeField]
    private Transform[] waypoints;
    //compared to deltatime, used for different computer clock speeds
    [SerializeField]
    private float navigationUpdate;

    private Transform enemy;
    private float navigationTime = 0;


    // Use this for initialization
    void Start () {
        enemy = GetComponent<Transform>();
        GameManager.Instance.RegisterEnemy(this);
	}
	
	// Update is called once per frame
	void Update () {
		if (waypoints != null)
        {
            navigationTime += Time.deltaTime; //vai somando o deltatime
            //se for maior que o update
            if (navigationTime > navigationUpdate)
            {
                if (target < waypoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, waypoints[target].position, navigationTime);
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
                }
                navigationTime = 0; //resets timer
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Checkpoint")
        {
            target++;
        }
        else if (other.tag == "Finish")
        {
            GameManager.Instance.UnregisterEnemy(this);            
        }
    }
}
