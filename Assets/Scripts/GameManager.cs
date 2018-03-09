using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creates instance of GameManager. Gamamanger inherts from Singleton<GameManager>
public class GameManager : Singleton<GameManager> {
    
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private int maxEnemiesOnScreen;
    [SerializeField]
    private int totalEnemies;
    [SerializeField]
    private int enemiesPerSpawn;


    private int enemiesOnScreen = 0;
    const float spawnDelay = 0.5f;

    // Use this for initialization
    void Start () {
        //SpawnEnemy();
        StartCoroutine(Spawn());
	} 

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && enemiesOnScreen < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (enemiesOnScreen < maxEnemiesOnScreen)
                {
                    GameObject newEnemy = Instantiate(enemies[1]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;
                    enemiesOnScreen += 1;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
        
    }

    public void RemoveEnemyFromScreen()
    {
        if (enemiesOnScreen > 0)
        {
            enemiesOnScreen--;
        }


    }
}
