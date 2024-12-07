using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 25;

    public GameObject rockPrefab;

    public Transform handPos;

    void KickOff()
    {
        if (attackTarget != null && transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStates = attackTarget.GetComponent<CharacterStats>();

            Vector3 direction = (attackTarget.transform.position-transform.position).normalized;

            targetStates.GetComponent<NavMeshAgent>().isStopped=true; 
            targetStates.GetComponent<NavMeshAgent>().velocity=kickForce*direction;
            targetStates.GetComponent<Animator>().SetTrigger("Dizzy");
            targetStates.TakeDamage(characterStats, targetStates);
        }

    }


    public void ThrowRock()
    {
        if (attackTarget != null)
        {
            var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
            rock.GetComponent<Rock>().target = attackTarget;
        }
    }
}
