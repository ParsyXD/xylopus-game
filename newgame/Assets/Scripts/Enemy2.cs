using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{

    public float currentHealth;

    public UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    public Transform head;
    public float sightRange;
    public bool playerInSightRange;
    private float currentDistance;
    public Rigidbody bullet;
    public Transform shootpoint;
    public float bulletSpeed;
    public float maxReach;

    public float health { get; set; }



    // Start is called before the first frame update
    void Start()
    {
        health = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 1)
        {
            Die();
        }

        float distance = Vector3.Distance(player.transform.position, transform.position);


        if (distance <= maxReach) ChasePlayer();
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        head.LookAt(player.position);

        //attack
        //Instantiate(bullet, shootpoint);
        //bullet.velocity = transform.forward * bulletSpeed;
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
        Gizmos.color = Color.red;
        Gizmos.DrawLine(head.position, player.position);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxReach);
    }
}
