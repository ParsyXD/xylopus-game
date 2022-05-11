using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGrabber3D;

public class Player : MonoBehaviour, Ihealth
{
    public static Player Instance { get; private set; }
    public float health { get; set; }
    public float currentHealth;
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

    }
 void Start()
 {
     health = currentHealth;
 }

 void Update()
 {
     if (health <= 0)
     {
         Die();
     }
 }
    public void SetControllsActive(bool active)
    {
        GetComponentInChildren<FirstPersonMovement>().enabled = active;
        GetComponentInChildren<FirstPersonLook>().enabled = active;
        GetComponentInChildren<FirstPersonAudio>().enabled = active;
        GetComponentInChildren<Crouch>().enabled = active;
        GetComponentInChildren<Jump>().enabled = active;
        GetComponentInChildren<Zoom>().enabled = active;
        GetComponentInChildren<MyGrabber>().enabled = active;
        canvas.enabled = active;

        if (active)
        {
            Cursor.lockState = CursorLockMode.Locked;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
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
}
