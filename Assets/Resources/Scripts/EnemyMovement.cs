using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    Transform player;
    NavMeshAgent nav;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<NavMeshAgent>().enabled)
        {
            nav.SetDestination(player.position);
        }          
        
    }




}
