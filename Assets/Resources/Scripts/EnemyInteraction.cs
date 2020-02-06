using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using System.Linq;

public class EnemyInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    NavMeshAgent navMeshAgent;
    public bool searching = false;
    private bool searchingAmmo = false;
    private bool searchingHealth = false;
    private EnemyHealth enemyHealth;
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth.health > 0 && searching)
        {
            if (!navMeshAgent.pathPending)
            {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        navMeshAgent.speed = 0;
                        if (searchingAmmo)
                        {
                            searchingAmmo = false;
                            GetComponent<EnemyAttack>().pickUpAmmo();
                        }
                        else if(searchingHealth)
                        {
                            GetComponent<EnemyHealth>().pickUpHealth();
                            searchingHealth = false;
                        }
                        searching = false;
                    }
                }
            }
        }
    }

    public void searchForAmmo()
    {
        if (enemyHealth.health <= 0) return;
        searchingAmmo = true;
        GameObject[] ammoBoxPosition = GameObject.FindGameObjectsWithTag("ammoBox");
        List<float> dist = new List<float>();
        foreach (GameObject pos in ammoBoxPosition)
        {
            dist.Add(Vector3.Distance(pos.GetComponent<Transform>().position, this.transform.position));
        }
        for(int i=0; i<dist.Count; i++)
        {
            double smallest_distance = dist.Min();
            if(dist[i] == smallest_distance)
            {
                searching = true;
                Vector3 direction = ammoBoxPosition[i].GetComponent<Transform>().position - navMeshAgent.transform.position;
                navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

                navMeshAgent.SetDestination(ammoBoxPosition[i].GetComponent<Transform>().position);
                navMeshAgent.speed = 5;
            }
        }
       
    }

    public void searhForHealth()
    {
        if (enemyHealth.health <= 0) return;
        GameObject[] healthBoxPosition = GameObject.FindGameObjectsWithTag("healthBox");
        if (healthBoxPosition != null)
        {
            List<float> dist = new List<float>();
            foreach (GameObject pos in healthBoxPosition)
            {
                dist.Add(Vector3.Distance(pos.GetComponent<Transform>().position, this.transform.position));
            }
            for (int i = 0; i < dist.Count; i++)
            {

                double smallest_distance = dist.Min();
                if (dist[i] == smallest_distance)
                {
                    searching = true;
                    searchingHealth = true;
                    Vector3 direction = healthBoxPosition[i].GetComponent<Transform>().position - navMeshAgent.transform.position;
                    navMeshAgent.transform.rotation = Quaternion.Slerp(navMeshAgent.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

                    navMeshAgent.SetDestination(healthBoxPosition[i].GetComponent<Transform>().position);
                    navMeshAgent.speed = 5;
                }
            }
        }
        else Debug.Log("Could not find health Boxes");

    }
    public void runFromGrenade(Transform grenadePos)
    {
     //   Rigidbody rb = grenadePos.GetComponent<Rigidbody>();
       // navMeshAgent.SetDestination(new Vector3(navMeshAgent.destination.x - rb.velocity.x, navMeshAgent.destination.y, navMeshAgent.destination.z - rb.velocity.z));
    }
}
