using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accuracy : MonoBehaviour
{
   

    
    private bool hasCollide = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
          //  if(hasCollide == false)
          //  {
              //  hasCollide = true;

               Debug.Log("Taiklumas pamažėjo");

               EnemyAttack.AttackDistance = 5f;
               EnemyAttack.FollowDistance = 10f;
               EnemyAttack.AttackProbability = 0.25f;
               EnemyAttack.HitAccuracy = 0.25f;
         //   }
           
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
          //  if (hasCollide == false)
         //   {
               // hasCollide = true;
                Debug.Log("Taiklumas padidėjo");


        EnemyAttack.AttackDistance = 15f;
        EnemyAttack.FollowDistance = 20f;
        EnemyAttack.AttackProbability = 0.65f;
        EnemyAttack.HitAccuracy = 0.65f;
   // }

}
    }
}
