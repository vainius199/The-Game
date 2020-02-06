using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuLogic : MonoBehaviour {


    public PhotonButtons photonB;
    public GameObject player;
    public float spawnTime = 3f;
    private List<Transform> spawn;

    private void Awake()
    {
        DontDestroyOnLoad(this.transform);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void MoveScene()
    {
        PhotonNetwork.LoadLevel("Resources/Scenes/scene01");
    }
    public void CreateNewRoom()
    {        
        PhotonNetwork.CreateRoom(photonB.createRoom.text, new RoomOptions() { MaxPlayers = 4 }, null);
    }
    public void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(photonB.joinRoom.text, roomOptions, TypedLobby.Default);
    }

    private void OnJoinedRoom()
    {
        MoveScene();    
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
  
        if(scene.name == "scene01")
        {
            spawnPlayer();
            spawn = new List<Transform>();
            for(int i=0; i<4; i++)
            {
                if (i == 0)
                    spawn.Add(GameObject.Find("SpawnPoint").transform);
                else
                {
                    spawn.Add(GameObject.Find("SpawnPoint (" + i.ToString() + ")").transform);
                }
            }
            InvokeRepeating("Spawn", spawnTime, spawnTime);
        }
    }

    private void spawnPlayer()
    {
        PhotonNetwork.Instantiate(player.name, player.transform.position, player.transform.rotation, 0);

    }

    void Spawn()
    {

        int spawnPointIndex = Random.Range(0, spawn.Count);
        //  Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        PhotonNetwork.Instantiate("Mr BOT", spawn[0].position, spawn[0].rotation, 0);

    }



}
