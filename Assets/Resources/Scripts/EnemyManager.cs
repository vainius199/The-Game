using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public int turns = 3;
    public int turn = 1;
    public int MaxEnemies = 1;
    private int spawned = 0;
    public Text YouWonText;
    private float leftTime = 100f;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;
    string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
        YouWonText = GameObject.Find("YouWon").GetComponent<Text>();
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }


    private void NextRound()
    {
        leftTime = 100f;
        MaxEnemies = MaxEnemies = MaxEnemies + 5;
        turn++;
        ScoreManager.score = 0;
        spawned = 0;
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    void Update()
    {
        if (ScoreManager.score/10 == MaxEnemies)
        {
            NextRound();
        }
        else if (spawned == MaxEnemies)
        {
            CancelInvoke("Spawn");
        }
        if (leftTime > 0)
        {
            leftTime -= Time.deltaTime;
        }
        else
        {
            GameOver();
        }

        if(turn == 4 && ScoreManager.score >= 250 && sceneName != "scene02")
        {
            GameWon();
        }
        if (turn == 2 && ScoreManager.score >= 190 && sceneName == "scene02")
        {
            GameWonLevel2();
        }
    }

    public void GameWon()
    {
        YouWonText.text = "Misija įvygdyta";
        SceneManager.LoadScene("scene02", LoadSceneMode.Single);
        Destroy(this);
       
    }

    public void GameWonLevel2()
    {
        YouWonText.text = "Misija įvygdyta";
        Destroy(this);

    }

    public void GameOver()
    {
        playerHealth.Death();
    }

    private void OnGUI()
    {
        GUILayout.Label("Vawe: " + turn);
        GUILayout.Label("Bots left: " + ScoreManager.score / 10 + "/" + MaxEnemies);
        GUILayout.Label("Time left: " + Mathf.Round(leftTime));
        GUILayout.Label("Spawned: " + spawned);
    }

    void Spawn()
    {
        spawned++;
        if (playerHealth.currentHealth <= 0f)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);


        Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        //  PhotonNetwork.Instantiate("Mr BOT", spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation, 0);

    }
}
