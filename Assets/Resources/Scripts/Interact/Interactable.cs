using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Interactable : MonoBehaviour {

    // Use this for initialization
    public Text infoText;
	void Start () {
		
	}
    public Text GetInfoText()
    {
        return infoText;
    }

}
