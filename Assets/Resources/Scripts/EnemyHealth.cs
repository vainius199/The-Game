using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public GameObject Bot;
    public float destroyTime = 10f;

    Animator anim;
    ParticleSystem hitPatricles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;

    [SerializeField]
    public float health = 100.0f;

     void Awake()
    {
        anim = GetComponent<Animator>();
        hitPatricles = GetComponentInChildren<ParticleSystem>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }


    private void Start()
    {
        FloatingTextController.Initialize();
    }


   void Update()
    {
        if (isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
        if (health <= 20)
        {
            GetComponent<EnemyInteraction>().searhForHealth();
        }
    }



    public void DoDamageToEnemy(float damage, Vector3 hitPoint)
    {
        if (isDead)
            return;

        PhotonView pView = GetComponent<PhotonView>();

        if (pView)
        {
            pView.RPC("takeDamage", PhotonTargets.All, damage);
        }
        
        FloatingTextController.CreateFloatingText(damage.ToString(), transform);
        health -= damage;

        hitPatricles.transform.position = hitPoint;
        hitPatricles.Play();

        if (health <= 0)
        {
            Death();
        }


    }

    void Death()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        isDead = true;

        capsuleCollider.isTrigger = true;
        
        anim.SetTrigger("Dead");
        StartCoroutine(DestroyEnemy(10f));

    }

    IEnumerator DestroyEnemy(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(Bot);
    }

    public void StartSinking()
    {
       
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        ScoreManager.score += scoreValue;

    }

    public void pickUpHealth()
    {
        health = 100f;
    }
}
