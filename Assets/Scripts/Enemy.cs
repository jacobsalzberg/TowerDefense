using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    
    [SerializeField]
    private Transform exitPoint; //transform is a location
    [SerializeField]
    private Transform[] waypoints;
    //compared to deltatime, used for different computer clock speeds
    [SerializeField]
    private float navigationUpdate;
    [SerializeField]
    private int healthPoints;
    [SerializeField]
    private int rewardAmt;

    private int target = 0;
    private Transform enemy;
    private Collider2D enemyCollider;
    private float navigationTime = 0;
    private bool isDead = false;
    private Animator anim;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }


    // Use this for initialization
    void Start () {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        GameManager.Instance.RegisterEnemy(this);
	}
	
	// Update is called once per frame
	void Update () {
		if (waypoints != null && !isDead)
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
            //if enemies hit the end, add to both roundescaped and totalescaped
            GameManager.Instance.RoundEscaped += 1;
            GameManager.Instance.TotalEscaped += 1;
            GameManager.Instance.UnregisterEnemy(this);            
        } else if (other.tag == "Projectile")
        {
            //get the projectile component so we can access inside the script
            Projectile newP = other.gameObject.GetComponent<Projectile>();
            // public getter on projectile that will work
            EnemyHit(newP.AttackStrength);
            Destroy(other.gameObject);
        }
    }

    public void EnemyHit(int hitpoints)
    {
        if (healthPoints - hitpoints > 0)
        {
            healthPoints -= hitpoints;
            anim.Play("Hurt");
        } else {
            anim.SetTrigger("didDie");
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        enemyCollider.enabled = false;
        GameManager.Instance.TotalKilled += 1;
    }

}
