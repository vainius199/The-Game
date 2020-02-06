using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PhotonButtons : MonoBehaviour {

    public InputField createRoom, joinRoom;
    public MenuLogic menuLogic;

    public void OnClickCreateRoom()
    {
        menuLogic.CreateNewRoom();
    }
    public void OnClickJoinRoom()
    {
        menuLogic.JoinOrCreateRoom();
    }
    


    //veliau
    private void OnLeftRoom()
    {

    }
        
}
