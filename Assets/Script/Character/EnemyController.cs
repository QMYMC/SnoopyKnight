using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public enum EnemyStates {GUARD,PATROL,CHASE,DEAD }
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]  
public class EnemyController : MonoBehaviour,IEndGameObsever
{
    private EnemyStates enemyStates;
    private NavMeshAgent agent;
    private Animator anim;
    private Collider coll;

    protected CharacterStats characterStats;

    [Header("Basic Setting")]
    public float sightRadius;
    public bool isGuard;
     
    public float speed;
    protected GameObject attackTarget;
    public float lookAtTime;
    private float remainLookAtTime;

    [Header("Patrol State")]
    public float patrolRange;
    private Vector3 wayPoint;
    private Vector3 gurdPos;
    private float lastAttackTime;
    private Quaternion guardRotation;

    bool isWalk;
    bool isChase;
    bool isFollow;
    bool isDead;
    bool playerDead;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<Collider>();
        speed = agent.speed;
        gurdPos = transform.position;
        guardRotation = transform.rotation;
        remainLookAtTime = lookAtTime;
    }

    void Start()
    {
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates=EnemyStates.PATROL; 
            GetNewWayPoint();

        }
        //³¡¾°ÇÐ»»ºóÐÞ¸Ä
        GameManager.Instance.AddObserver(this);
    }

    //void OnEnable()
    //{
        
    //}

    void OnDisable()
    {
        if (!GameManager.IsIntialized) return;
        GameManager.Instance.RemoveObserver(this);
    }

    void Update()
    {
        if (characterStats.CurrentHealth <= 0)
            isDead = true;

        if (!playerDead)
        {
            SwithStates();
            SwitchAnimator();
            lastAttackTime -= Time.deltaTime;
        }
        
        
    } 
    void SwitchAnimator()
    {
        anim.SetBool("Walk", isWalk);
        anim.SetBool("Chase", isChase);
        anim.SetBool("Follow",isFollow);
        anim.SetBool("Critical", characterStats.isCritical);
        anim.SetBool("Death", isDead);
    }

    void SwithStates()
    {
        if (isDead) 
            enemyStates = EnemyStates.DEAD;

        //Èç¹û·¢ÏÖplayer ÇÐ»»CHASE
        else if (FoundPlayer())
        {
            enemyStates = EnemyStates.CHASE;
            //Debug.Log(attackTarget.gameObject.name);
        }

        switch(enemyStates)
        {
            case EnemyStates.GUARD:
                isChase = false;
                 
                if (transform.position != gurdPos)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = gurdPos;

                    if (Vector3.SqrMagnitude(gurdPos - transform.position) <= agent.stoppingDistance)
                    {
                        isWalk=false;
                        transform.rotation = Quaternion.Lerp(transform.rotation,guardRotation,0.2f);
                    }
                }
                break;

            case EnemyStates.PATROL:
                isChase = false;
                agent.speed = speed*0.5f;

                //ÅÐ¶ÏÊÇ·ñµ½ÁËËæ¼´Ñ²Âßµã
                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if(remainLookAtTime>0)
                        remainLookAtTime-=Time.deltaTime;
                    else
                    GetNewWayPoint();
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }

                break; 
            case EnemyStates.CHASE:
                isWalk = false;
                isChase = true;
                
                agent.speed = speed;
                if (!FoundPlayer())
                {
                    isFollow = false;
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else if (isGuard)
                        enemyStates = EnemyStates.GUARD;
                    else
                        enemyStates = EnemyStates.PATROL;
                    
                }
                else  
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                    
                }

                if (TargetInAttackRange() || TargetInSkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;
                    if (lastAttackTime < 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;
                        //±©»÷ÅÐ¶Ï
                        characterStats.isCritical = Random.value<characterStats.attackData.criticalChance;
                        //if (characterStats.isCritical) { Debug.Log("baoji"); }
                        
                        //Ö´ÐÐ¹¥»÷
                        Attack();

                    }
                }
                break;
            case EnemyStates.DEAD:

                //agent.enabled = false;
                coll.enabled = false;
                agent.radius = 0;
                Destroy(gameObject,2f);

                break;
        }
    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform);
        if (TargetInAttackRange())
        {
            anim.SetTrigger("Attack");
        }

        if (TargetInSkillRange())
        {
            anim.SetTrigger("Skill"); 
        }
    }

    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (var target in colliders) 
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                //agent.destination = attackTarget.transform.position;
                return true;
            }
        }

        attackTarget = null;
        return false;
    }

    bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position)<=characterStats.attackData.attackRange;
        else return false;
    }


    bool TargetInSkillRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterStats.attackData.skillRange;
        else return false;
    }

    void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;

        float randomX = Random.Range(-patrolRange,patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        Vector3 randomPoint = new Vector3(gurdPos.x+randomX,transform.position.y,gurdPos.z+randomZ);
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
        //wayPoint = randomPoint;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }

    void Hit()
    {
        if (attackTarget != null&&transform.IsFacingTarget(attackTarget.transform))
        {
            var targetStates = attackTarget.GetComponent<CharacterStats>();

            targetStates.TakeDamage(characterStats, targetStates);
        }
       
    }
    
    public void EndNotify()
    {
        //»ñÊ¤¶¯»­
        //Í£Ö¹ËùÓÐÒÆ¶¯
        //Í£Ö¹agent
        anim.SetBool("Win", true);
        playerDead = true;
        isChase = false;
        isWalk = false;
        attackTarget = null;
    }
}
