using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class photonConnect : Photon.PunBehaviour
{
    public string versionName = "0.1";
    
    public GameObject panel, panel1, panel2;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);
        Debug.Log("COnnecting to photon");
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("masteR");
    }

    private void OnJoinedLobby()
    {
        Debug.Log("Onlobby");
        panel.SetActive(false);
        panel1.SetActive(true);
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.Log("disc");
        panel.SetActive(false);
        panel2.SetActive(true);
    }

}
