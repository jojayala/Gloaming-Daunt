using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Based on Dave/GameDevelopment's 3D enemy AI tutorial, https://www.youtube.com/watch?v=UjkSFoLxesw
public class EnemyScript : MonoBehaviour
{
    [HideInInspector] public bool isDead;
    Animator animator;
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    public Transform spawnPoint;

    public float projectileSpeed = 5f;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        isDead = false;
    }

    private void Update()
    {
        
        if (!isDead)
        {
            
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
            if (!playerInSightRange && !playerInAttackRange) Patrolling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange)
            {
                AttackPlayer();
            } else
            {
                animator.SetBool("isThrow", false);
            }
        } else
        {
            animator.SetBool("isDead", true);
            StartCoroutine(decayTime());
        }
        
    }
    private IEnumerator decayTime()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    private void Patrolling()
    {
        
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        animator.SetBool("isRunning", true);
        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isRunning", true);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        animator.SetBool("isRunning", false);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            animator.SetBool("isThrow", true);
            SoundManager.S.MakeThrowSound();
            ///Attack code here
            Quaternion rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x, this.transform.eulerAngles.y - 90, this.transform.eulerAngles.z);

            GameObject cS = Instantiate(projectile, spawnPoint.position, rotation);
            Rigidbody rig = cS.GetComponent<Rigidbody>();
            rig.AddForce(spawnPoint.forward * projectileSpeed, ForceMode.Impulse);
            rig.AddForce(spawnPoint.up * (projectileSpeed/20), ForceMode.Impulse);
            ///End of attack code
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

