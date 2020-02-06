using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyNetwork : Photon.MonoBehaviour
{

    EnemyHealth enemyHealth;
    private float hp = 0;
    void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        hp = enemyHealth.health;
        gameObject.name = gameObject.name + photonView.viewID;
    }

    [PunRPC]
    public void takeDamage(float damage)
    {

        enemyHealth.health -= damage;
        Debug.Log(enemyHealth.health);
        if(enemyHealth.health <= 0)
        {
            if (photonView.isMine)
            {
                   PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(enemyHealth.health);
        }
        else
        {
            enemyHealth.health = (float)stream.ReceiveNext();
        }
    }


    void Update()
    {
          //  Debug.Log(hp);
    }

}
