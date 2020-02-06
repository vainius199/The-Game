using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthController : MonoBehaviour {

    [SerializeField]
    private float health = 100.0f;
    private void Start()
    {
        FloatingTextController.Initialize();
    }

    public void ApplyDamage(float damage)
    {
        FloatingTextController.CreateFloatingText(damage.ToString(), transform);
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
      
    }


}
