using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] weapons;
    [SerializeField]
    private float switchDelay;
    private int index;
    private bool isSwitching = false;
    public GameObject[] interactableWeapon;
    // Use this for initialization
    public Image deathUI;
    bool youreDead;

    void Start () {
        InitializeWeapons();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Mouse ScrollWheel") > 0 && !isSwitching)
        {
            if(++index >= weapons.Length)
            {
                index = 0;
                SwitchWeapons(index);
            }
            StartCoroutine(SwitchAfterDelay(index));
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && !isSwitching)
        {
            if (--index <= 0)
            {
                index = weapons.Length - 1;
                SwitchWeapons(index);
            }
            StartCoroutine(SwitchAfterDelay(index));
        }
        else if(Input.GetKeyDown(KeyCode.G))
        {

            GameObject throwWeapon = Instantiate(interactableWeapon[index], transform.position, Quaternion.identity);
            throwWeapon.transform.parent = GameObject.Find("Interactable").transform;


            throwWeapon.GetComponent<BoxCollider>().isTrigger = false;
            throwWeapon.AddComponent<Rigidbody>();     
            throwWeapon.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);
            weapons[index].SetActive(false);
        }


        if (youreDead == true)
        {
            Debug.Log("mirei");

            deathUI.CrossFadeAlpha(255, 0.5f, false);

        }
    }


    private IEnumerator SwitchAfterDelay(int index)
    {
        isSwitching = true;
        yield return new WaitForSeconds(switchDelay);

        isSwitching = false;
        SwitchWeapons(index);
    }
    private void InitializeWeapons()
    {
        for(int i=0; i<weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[0].SetActive(true);
    }

    private void SwitchWeapons(int newIndex)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }
        weapons[newIndex].SetActive(true);
    }

    public void PickUpWeapon(string weaponName)
    {
        switch(weaponName)
        {
            case "AK-47":
                weapons[0].SetActive(true);
                weapons[1].SetActive(false);
                break;
            case "M4":
                weapons[1].SetActive(true);
                weapons[0].SetActive(false);
                break;
        }
    }

    public void DeathUI()
    {
        //sorry kad cia idejau, bet kitur butu buve daugiau darbo idet funkcija
        youreDead = true;
        
    }

}
