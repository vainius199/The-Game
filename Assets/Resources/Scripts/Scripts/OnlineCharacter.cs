using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class OnlineCharacter : Photon.MonoBehaviour
{

    private Vector3 realPosition = Vector3.zero;
    private Quaternion realRotation = Quaternion.identity;
    FirstPersonController controllerScript;

    void Awake()
    {
        controllerScript = GetComponent<FirstPersonController>();
        controllerScript.characterState = AnimState.Idle;
        if (photonView.isMine)
        {
            controllerScript.enabled = true;
        }
        else
        {
            controllerScript.enabled = true;
            controllerScript.isControllable = false;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<FirstPersonController>().testing)
        {
            if (!photonView.isMine)
            {
                //Update remote player (smooth this, this looks good, at the cost of some accuracy)
                transform.position = Vector3.Lerp(transform.position, realPosition, Time.deltaTime * 5);
                transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, Time.deltaTime * 5);
            }
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data
            stream.SendNext((int)controllerScript.characterState);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            controllerScript.characterState = (AnimState)(int)stream.ReceiveNext();
            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
