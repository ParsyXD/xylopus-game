using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class homingbulletvsp : MonoBehaviour
{
    public float starthomingspeed;
    public float activationhomingspeed;
    public Transform enemy;
    public Transform bullet;
    public float maxEnemySightRange;
    GameObject enemy1;
    public float damage;

    public float curDist = 100;
    GameObject[] enemyA;
    GameObject enemyToTarget;
    public GameObject player;
    
    
    // Start is called before the first frame update
    void Start()
    {
                          
       //enemy1 = GameObject.FindWithTag("Enemy");
       // enemy = enemy1.transform;
        transform.parent = null;                        
        

        StartCoroutine(search());
        
    }

    // Update is called once per frame
    void Update()
    {
       
         transform.Translate(Vector3.forward * starthomingspeed * Time.deltaTime);

         float distance = Vector3.Distance(enemy.transform.position,transform.position);
        if (distance <= maxEnemySightRange) ChaseEnemy();
          
    }
    void ChaseEnemy()
    {
        bullet.LookAt(enemy.position);

    }
    
     
    void OnCollisionEnter(Collision other)
    {
        Explode();
        Debug.Log("coll" + other.gameObject.name);
         if (other.transform.CompareTag("Player"))
         {

         Ihealth health = other.transform.GetComponentInParent<Ihealth>();    //Sucht in getroffenen Objekt nach IHealth und der Variable health
         if (health != null)       // ob getroffenes Objekt health von IHealth enthält
      {
        health.GetDamage(damage);     // führt in Enemy Damage mit dem Parameter damage aus (wie viele Schaden)
        
      }
         }
    }    
     
    IEnumerator search()
    {
      yield return new WaitForSeconds(1);
      starthomingspeed = activationhomingspeed;
      // die rakete sucht den nächsten gegner beim spawn und such keinen neuen !!!wenn sie immer den nahesten suchen soll muss es in update!!!
           enemyA = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject enemys in enemyA)
            {
                float dist = Vector3.Distance(bullet.transform.position, enemys.transform.position);
                if (dist < curDist)
                {
                    curDist = dist;
                    enemyToTarget = enemys;
                    
                }
            }
            if (enemyToTarget != null) {
                enemy = enemyToTarget.transform;
            }
            yield return new WaitForSeconds(5);
            Explode();
    }

    void Explode ()
    {
        Destroy(gameObject);
       

    }
     
     
     
     void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere (transform.position, maxEnemySightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere (transform.position, curDist);
    }

     

    
}
