using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthController : MonoBehaviour {

    [SerializeField]
    private float health = 100.0f;


    private void Start()
    {
    }
    public void ApplyDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
        Debug.Log(health);
    }

    public float GetHealth()
    {
        return health;
    }
}
