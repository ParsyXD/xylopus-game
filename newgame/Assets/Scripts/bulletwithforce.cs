using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletwithforce : MonoBehaviour
{
  public float damage;
  public float shootForce;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);
    }

    

    void OnCollisionEnter(Collision other)
    {
      
      Ihealth health = other.transform.GetComponentInParent<Ihealth>();    //Sucht in getroffenen Objekt nach IHealth und der Variable health
         if (health != null)       // ob getroffenes Objekt helth von IHealth enthält
      {
        health.GetDamage(damage);     // führt in Enemy Damage mit dem Parameter damage aus (wie viele Schaden)
        
      }
              Destroy(gameObject);
    }
}
