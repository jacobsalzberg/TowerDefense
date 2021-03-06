﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameStatus
{
    next,play,gameover,win
};

//Creates instance of GameManager. Gamamanger inherts from Singleton<GameManager>
public class GameManager : Singleton<GameManager> {
    [SerializeField]
    private int totalWaves = 10;
    [SerializeField]
    private Text totalMoneyLbl; //label, upper left corner
    [SerializeField]
    private Text currentWaveLbl;
    [SerializeField]
    private Text totalEscapedLbl;
    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private Enemy[] enemies;
    [SerializeField]
    private int totalEnemies=3;
    [SerializeField]
    private int enemiesPerSpawn;
    [SerializeField]
    private Text playBtnLbl;
    [SerializeField]
    private Button playBtn;

    private int waveNumber = 0;
    private int totalMoney = 10;
    private int totalEscaped = 0;
    private int roundEscaped = 0;
    private int totalKilled = 0;
    private int whichEnemiesToSpawn = 0;
    private int enemiesToSpawn = 0; //between 0 and 2, as we have 3 enemies
    private GameStatus currentState = GameStatus.play;
    private AudioSource audioSource;

    public List<Enemy> EnemyList = new List<Enemy>();

    const float spawnDelay = 0.5f;

    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }
    }

    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }

    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }


    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLbl.text = totalMoney.ToString();
        }
    }

    public AudioSource AudioSource
    {
        get
        {
            return audioSource;
        }
    }

    // Use this for initialization
    void Start () {
        //Hides the button
        playBtn.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        ShowMenu();
        //SpawnEnemy();
        //StartCoroutine(Spawn());

	}

    private void Update()
    {
        HandleEscape();
    }

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    Enemy newEnemy = Instantiate(enemies[Random.Range(0,2)]) as Enemy;
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }        
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public void DestroyAllEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public void AddMoney(int amount)
    {
        TotalMoney += amount;
    }

    public void SubtractMoney (int amount)
    {
        TotalMoney -= amount;
    }

    public void IsWaveOver()
    {
        totalEscapedLbl.text = "Escaped" + TotalEscaped + "/10";
        if ((RoundEscaped + TotalKilled) == totalEnemies)
        {
            if (waveNumber <= enemies.Length)
            {
                //wave 0-> spawn enemy 1, wave 1-> spawn enemy 2
                enemiesToSpawn = waveNumber;
            }
            SetCurrentGameState();
            ShowMenu();
        }
    }

    public void SetCurrentGameState()
    {
        if (TotalEscaped >= 10) //stuck to 10, magic numbering or something (forgot the term)
        {
            currentState = GameStatus.gameover;
        } else if (waveNumber==0 && (TotalKilled + RoundEscaped) == 00)
        {
            currentState = GameStatus.play;
        } else if (waveNumber >= totalWaves)
        {
            currentState = GameStatus.win;
        } else
        {
            currentState = GameStatus.next;
        }
    }

    public void ShowMenu()
    {
        switch (currentState)
        {
            //lost game
            case GameStatus.gameover:
                playBtnLbl.text = "Play Again!";
                AudioSource.PlayOneShot(SoundManager.Instance.GameOver);
                break;
            // just completed a wave
            case GameStatus.next:
                playBtnLbl.text = "Next Wave";
                break;
            case GameStatus.play:
                playBtnLbl.text = "Play";
                break;
            case GameStatus.win:
                playBtnLbl.text = "Play";
                break;          

        }
        playBtn.gameObject.SetActive(true);
    }

    public void PlayBtnPressed()
    {
        switch(currentState)
        {
            case GameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;
            // play game for first time or gameover
            default:
                totalEnemies = 3;
                TotalEscaped = 0;
                TotalMoney = 10;
                //skully spawns at begging
                enemiesToSpawn = 0;
                TowerManager.Instance.DestroyAllTower();
                TowerManager.Instance.RenameTagsBuildSite();
                totalMoneyLbl.text = TotalMoney.ToString();
                totalEscapedLbl.text = "Escaped" + TotalEscaped + "/10";
                audioSource.PlayOneShot(SoundManager.Instance.NewGame);
                break;
        }

        DestroyAllEnemies();
        TotalKilled = 0; // only cares for this during a wave
        RoundEscaped = 0; //only cares for this during a wave
        currentWaveLbl.text = "Wave " + (waveNumber + 1);
        //need to spawn enemies:
        StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableDragSprite();
            TowerManager.Instance.towerBtnPresed = null;
        }
    }
}
