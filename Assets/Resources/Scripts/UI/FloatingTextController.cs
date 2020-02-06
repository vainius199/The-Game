using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextController : MonoBehaviour {

    private static FloatText popUpTextPrefab;
    private static GameObject canvas;
    public static void Initialize()
    {
        canvas = GameObject.Find("Canvas");
        if(!popUpTextPrefab)
        popUpTextPrefab = Resources.Load<FloatText>("Prefabs/PopUpTextParent");
    } 

	public static void CreateFloatingText(string text, Transform location)
    {
        FloatText instance = Instantiate(popUpTextPrefab);
     //   Vector2 screenPos = Camera.main.WorldToScreenPoint(new Vector2(location.position.x + Random.Range(-10f,-5f), location.position.y));
        instance.transform.SetParent(canvas.transform, false);
        instance.transform.position = new Vector2(instance.transform.position.x + Random.Range(-30f, 30f), instance.transform.position.y + Random.Range(-30f, 30f));
        instance.SetText(text);
    }
}
