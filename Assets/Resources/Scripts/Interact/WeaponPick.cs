using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponPick : MonoBehaviour {

    // Use this for initialization
    public string weaponName;
    private Text infoText;
	void Start () {
        infoText = GetComponentInParent<Interactable>().GetInfoText();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<HealthController>())
        {
            collision.transform.GetComponent<HealthController>().ApplyDamage(50);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
      /*  Debug.Log("Stay");
        if (gameObject.GetComponent<Rigidbody>() && gameObject.GetComponent<Rigidbody>().IsSleeping())
        {
            Debug.Log("XDXDDS");
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
            gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 0.5f,1);
            Destroy(gameObject.GetComponent<Rigidbody>());
        }*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            infoText.text = "Press E to pick up " + weaponName;
            infoText.enabled = true;

        }
    }

    private void Update()
    {
        if (GetComponent<Rigidbody>())
        {
            float speed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            if (speed < 0.5)
            {
                gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                //Or
                gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                gameObject.GetComponent<BoxCollider>().isTrigger = true;
                gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 0.5f, 1);
                Destroy(gameObject.GetComponent<Rigidbody>());
            }
        }
    
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            infoText.text = "Press E to pick up " + weaponName;
            infoText.enabled = true;
            if(Input.GetKeyDown(KeyCode.E))
            {                
                other.GetComponentInChildren<WeaponManager>().PickUpWeapon(weaponName);
                Destroy(gameObject);
                infoText.enabled = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            infoText.enabled = false;
        }
    }
}
