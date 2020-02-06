using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChestAmmo : MonoBehaviour
{
    // Start is called before the first frame update
    Text infoText;

    void Start()
    {
        infoText = GameObject.Find("InfoText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        infoText.enabled = true;
        infoText.text = "Press E to open chest";
    }
    private void OnTriggerStay(Collider other)
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            if(other.tag == "Player")
            {
                other.GetComponentInChildren<Weapon>().pickUpAmmo(50);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        infoText.enabled = false;
    }
}
