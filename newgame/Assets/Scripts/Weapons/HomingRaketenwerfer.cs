using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingRaketenwerfer : MonoBehaviour
{
    [Header("Key")]
    public KeyCode shootKey = KeyCode.Mouse0;
    public KeyCode reloadKey = KeyCode.R;

    [Header("Transform")]
    public Transform shootpoint;

    [Header("GameObjects")]
    public Rigidbody bullet;
    

    [Header("Values")]
    public float bulletSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (Input.GetKeyDown(shootKey))
        {
            Shoot();
            
            
        }

        
    }
    void Shoot ()
    {
           Instantiate(bullet, shootpoint.position, shootpoint.rotation, null);
         
         

        
        

        
        
         
    }

}
