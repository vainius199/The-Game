using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : MonoBehaviour
{

    Animator anim;
    PlayerHealth playerHealth;
  public  GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        // playerHealth = player.GetComponent<PlayerHealth>(); // cia blogai parasyta
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            anim.SetTrigger("Open");
           playerHealth.HealUp();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            anim.SetTrigger("CLose");
        }
    }
}
