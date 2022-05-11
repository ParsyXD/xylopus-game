using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Enemy_new : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;

    enum Enemy_State
    {
        idle,
        chasingPLayer,
        attackingPLayer,
        reloading
    }



    private Enemy_State state;

    [SerializeField] private GameObject player;
    [Header("General")]
    [SerializeField] private float initialChaseDistance;
    private float chaseDistance;
    [SerializeField][Range(0.1f, 0.9f)] float initialAttackDistance;
    private float attackDistance;
    private bool showChaseDistanceGizmos = false;
    [Header("Attacking")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootpoint;
    [SerializeField] private int maxShotsPerMagazine;
    private bool moving;
    private bool hasSpottedPlayer = false;
    private AudioSource shootingSound;
    private int firedShots = 0;

    private void Awake()
    {
        chaseDistance = initialChaseDistance;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        shootingSound = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        player = Player.Instance.gameObject;
    }

    private void Update() 
    {
        RefreshState();
        agent.isStopped = !moving;

        if (state == Enemy_State.chasingPLayer || state == Enemy_State.attackingPLayer)
        {
            ChaseUpdate();
        }

        agent.speed = animator.deltaPosition.magnitude / Time.deltaTime;
    }

    private void RefreshState()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        attackDistance = chaseDistance * initialAttackDistance;

        Enemy_State newState = Enemy_State.idle;

        if (distance <= chaseDistance && (hasSpottedPlayer || CheckSeeingPlayer()))
        {
            hasSpottedPlayer = true;
            chaseDistance = initialChaseDistance + 5f;
            showChaseDistanceGizmos = true;
            newState = Enemy_State.chasingPLayer;
        }
        if (distance <= attackDistance && (CheckSeeingPlayer()))
        {
            hasSpottedPlayer = true;
            chaseDistance = initialChaseDistance + 5f;
            showChaseDistanceGizmos = true;
            newState = Enemy_State.attackingPLayer;
        }
        if(state == Enemy_State.attackingPLayer && firedShots >= maxShotsPerMagazine)
        {
            newState = Enemy_State.reloading;
            firedShots = 0;
        }
        SetState(newState);
    }

    private void SetState(Enemy_State state)
    {
        this.state = state;
        if (state == Enemy_State.chasingPLayer)
        {
            moving = true;
            animator.SetTrigger("Running");
            return;
        }
        else
        {
            moving = false;
        }
        if (state == Enemy_State.attackingPLayer)
        {
            animator.SetTrigger("Attacking");
            return;
        }
        if (state == Enemy_State.idle)
        {
            animator.SetTrigger("Idle");
            return;
        }
        if(state == Enemy_State.reloading)
        {
            animator.SetTrigger("Reloading");
            return;
        }
    }

    public void Attack()
    {
        Instantiate(bulletPrefab, shootpoint.position, shootpoint.rotation, null);
        shootingSound.Play();
        firedShots++;
    }

    private void ChaseUpdate()
    {
        agent.destination = player.transform.position;
        if (state == Enemy_State.attackingPLayer)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
    }

    private bool CheckSeeingPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, maxDistance: chaseDistance, hitInfo: out hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            if(hit.collider.gameObject.tag == "Player")
            {
                Debug.Log("True");
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if(chaseDistance == 0f)
        {
            chaseDistance = initialChaseDistance;
        }


        Gizmos.color = new Color(0f, 1f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, initialChaseDistance);

        Gizmos.color = new Color(0f, 1f, 0.5f, 0.1f);
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Gizmos.color = new Color(0f, 0.5f, 1f);
        Gizmos.DrawWireSphere(transform.position, initialChaseDistance * initialAttackDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawRay(player.transform.position, transform.position - player.transform.position);
    }
}
