using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChestScript : MonoBehaviour
{

    public GameObject[] drops;
    private Text infoText;
    // Start is called before the first frame update
    void Start()
    {
        infoText = GameObject.Find("InfoText").GetComponent<Text>();
    }



    private void OnTriggerEnter(Collider other)
    {
        infoText.text = "Press E to open Chest";
    }

    private void OnTriggerStay(Collider other)
    {

        if(Input.GetKeyUp(KeyCode.E))
        {
            int rand = (int)Random.Range(1, drops.Length);

            for(int i=0; i<rand; i++)
            {
                GameObject gg = Instantiate(drops[i], transform.position, transform.rotation);
                gg.AddComponent<Rigidbody>().AddForce(transform.up* 20);
                gg.GetComponent<BoxCollider>().isTrigger = false;

                
                
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        infoText.text = "";
    }


}
