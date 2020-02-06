using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{

    public float timeBetweenAttacks = 1.2f;
    public int attackDamage = 30;
    public int currentAmmo = 10;
    public int allAmmo = 30;

    Transform playerPosition;

    public static int Testas;
    
    Animator anim;
    GameObject player;  
    PlayerHealth playerHealth;
    bool playerInRange; // to hit with melee
    bool searchingForAmmo = false;
    float timer;
    public GameObject SphereCollider;

    NavMeshAgent navMeshAgent;
    public static float AttackDistance = 10.0f;
    public static float FollowDistance = 20.0f;
   // [Range(0.0f, 1.0f)]
    public static float AttackProbability = 0.5f;
  //  [Range(0.0f, 1.0f)]
    public static float HitAccuracy = 0.5f;
    public int ShootDamage = 10;



    void Start()
    {        
        player = GameObject.FindWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerHealth = player.GetComponent<PlayerHealth>();        
        anim = GetComponent<Animator>();

        playerPosition = GameObject.FindWithTag("Player").transform;
    }


     void OnTriggerEnter(Collider SphereCollider)
    {        
        if (SphereCollider.gameObject == player)
        {            
             playerInRange = true;
        }

    }

     void OnTriggerExit(Collider SphereCollider)
    {        
        if (SphereCollider.gameObject == player)
        {          
            playerInRange = false;

            anim.SetTrigger("Walk");
            
        }
    }

   

    void Update()
    {
      
            timer += Time.deltaTime;
        if (timer >= timeBetweenAttacks && playerInRange)
        {
            attack(); // kerta is ginklo
            Debug.Log("Kerta tau");
        }    
        //jei tu esi salia ir kulku neturi tada ju neieskos o tave is ginklu pabandys musti
        else if (Vector3.Distance(gameObject.transform.position, player.transform.position) >= 15.0f && allAmmo <= 0 && GetComponent<EnemyHealth>().health > 20) //Jei nera playeris in range tai ieskos ammo
        {
            anim.SetTrigger("Chase");
            searchingForAmmo = true;
            GetComponent<EnemyInteraction>().searchForAmmo();

        }
        //jei tu neesi salia tada bandys ieskot kulku
        else if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= 15.0f && currentAmmo <= 0 && GetComponent<EnemyHealth>().health > 20)
        {
            navMeshAgent.SetDestination(player.transform.position);
            searchingForAmmo = false;
            GetComponent<EnemyInteraction>().searching = false;
        }
        else if (navMeshAgent.enabled && !searchingForAmmo && GetComponent<EnemyHealth>().health > 20)
        {
            navMeshAgent.SetDestination(playerPosition.position);

            //Jei esi jo range, bot pradeda tave sekti ir saudyti
            float dist = Vector3.Distance(player.transform.position, this.transform.position);

            bool following = (dist < FollowDistance); 
            if (following)
            {
                //I tave nukreipia ginkla ir pats sukasi
                Vector3 direction = player.transform.position - navMeshAgent.transform.position;
                navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, Quaternion.LookRotation(direction), 0.1f);


              
             //   anim.SetTrigger("Walk");

                    float random = Random.Range(0.0f, 1.0f);
                       if (random > (1.0f - AttackProbability) && dist < AttackDistance)
                       {
                             navMeshAgent.speed = 0;
                            shooting();            
                       }
                
            }

            //pabegi is jo range, pradeda tave sekti po map;
             if(!following)
            {               
                anim.SetTrigger("Chase");
                navMeshAgent.speed = 5;

            }     
        }

    }
   
    void reload()
    {
        if (allAmmo <= 0) return;

        int bulletsToLoad = 10 - currentAmmo;
        int bulletsToDeduct = (allAmmo >= bulletsToLoad) ? bulletsToLoad : allAmmo;

        allAmmo -= bulletsToDeduct;
        currentAmmo += bulletsToDeduct;

    }


    void attack()
    {
        timer = 0f;

        if(playerHealth.currentHealth > 0)
        {
            anim.SetTrigger("Attack");            
        }

        if (playerHealth.currentHealth <= 0)
        {
            navMeshAgent.speed = 0;
            anim.SetTrigger("PlayerDead");
        }
    }

    void shooting()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 100))
        {
            if (hit.collider.tag == "Player")
            {
                if (currentAmmo > 0 && playerHealth.currentHealth > 0)
                {

                    anim.SetTrigger("Shoot");
                }

                if (playerHealth.currentHealth <= 0)
                {
                    navMeshAgent.speed = 0;
                    anim.SetTrigger("PlayerDead");
                }
                if (currentAmmo <= 0)
                {
                    anim.SetTrigger("Walk");
                }
            }
        }
    
    }

    void DamageWithGun()
    {
        //kerta is ginklo
        playerHealth.TakeDamage(attackDamage);
    }


    public void ShootEvent()
    {
    //Sauna is ginklo

    /* if (m_Audio != null)
     {
         m_Audio.PlayOneShot(GunSound);
     }
     */

        if (currentAmmo > 0)
        {
            currentAmmo--;
            float random = Random.Range(0.0f, 1.0f);

            // The higher the accuracy is, the more likely the player will be hit
            bool isHit = random > 1.0f - HitAccuracy;

            if (isHit)
            {

                playerHealth.TakeDamage(ShootDamage);
            }
        }
        else if (allAmmo > 0)
        {
            reload();
        }
        else if(allAmmo <= 0)
        {
            searchingForAmmo = true;
            GetComponent<EnemyInteraction>().searchForAmmo();
        }
    }
    public void pickUpAmmo()
    {
        searchingForAmmo = false;
        allAmmo += 30;
        reload();
    }

}
