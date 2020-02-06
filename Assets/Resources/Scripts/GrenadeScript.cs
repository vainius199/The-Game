using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GrenadeScript : MonoBehaviour
{
    public GameObject explosionEffect;
    public float delay = 3.0f;
    private float countDown;
    public float radius = 8.0f;
    public float explosionForce = 700.0f;
    bool hasExploded = false;
    // Start is called beforethe first frame update
    void Start()
    {
        countDown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in colliders)
        {
            if(nearbyObject.GetComponent<EnemyInteraction>())
             nearbyObject.GetComponent<EnemyInteraction>().runFromGrenade(transform);
    
        }
        if (countDown <= 0 && !hasExploded)
        {
            Explode();
        }
    }

    void Explode()
    {
        hasExploded = true;

        GameObject gg;
        if (!PhotonNetwork.connected)
            gg = Instantiate(explosionEffect, transform.position, transform.rotation);
        else gg = PhotonNetwork.Instantiate("Explosion", transform.position, transform.rotation, 0);



        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        //Paskui dabar addForce neveikia and navmesh
        foreach(Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

            if(rb != null && nearbyObject.GetComponent<EnemyHealth>())
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius);
                nearbyObject.GetComponent<EnemyHealth>().DoDamageToEnemy(100.0f, transform.position);
            
            }
        }
        if (!PhotonNetwork.connected)
        {
            Destroy(gameObject);
            Destroy(gg, 1.9f);
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
            StartCoroutine(destroy(gg));
        }

    }

    IEnumerator destroy(GameObject gg)
    {
        StartCoroutine("wait");
        PhotonNetwork.Destroy(gg);
        yield return new WaitForSeconds(0);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.9f);
    }
}
