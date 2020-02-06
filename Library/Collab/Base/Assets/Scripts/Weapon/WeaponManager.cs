using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] weapons;
    [SerializeField]
    private float switchDelay;
    private int index;


	// Use this for initialization
	void Start () {
        InitializeWeapons();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if(++index >= weapons.Length)
            {
                index = 0;
                SwitchWeapons(index);
            }
            SwitchWeapons(index);
        }
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
}
