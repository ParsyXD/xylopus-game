using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour, Ihealth
{

    public float currentHealth;

    public NavMeshAgent agent;
    public Transform player;
    public Transform head;
    public float sightRange;
    public bool playerInSightRange;
    private float currentDistance;
    public Rigidbody bullet;
    public Transform shootpoint;
    public float maxSight;
    public float tooNearRange;
    public float attackRange;
    float shootCooldown;
    public float shootCooldownTime;

    public float health { get; set; }



    // Start is called before the first frame update
    void Start()
    {
        health = currentHealth;
        shootCooldown = shootCooldownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 1)
        {
            Die();
        }

        float distance = Vector3.Distance(player.transform.position, transform.position);


        if (distance <= maxSight && distance > tooNearRange) ChasePlayer();
        if (distance < tooNearRange) TooClose();
        if (distance < attackRange) Attack();

        shootCooldown -= Time.deltaTime;

    }

    private void ChasePlayer()
    {
        gameObject.GetComponent<NavMeshAgent>().isStopped = false;
        agent.SetDestination(player.position);
        head.LookAt(player.position);

    }

    private void TooClose()
    {
        gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        head.LookAt(player.position);
    }

    private void Attack()
    {
        if (shootCooldown <= 0)
        {
            Instantiate(bullet, shootpoint.position, shootpoint.rotation, null);

            shootCooldown = shootCooldownTime;
        }

    }


    public void GetDamage(float damage)
    {
        health -= damage;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(head.position, player.position);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxSight);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, tooNearRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
