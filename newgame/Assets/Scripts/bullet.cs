using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class bullet : MonoBehaviour
{
    [SerializeField] private float force;
    [SerializeField] private int damage;
    private Player player;
    private GameObject playerObj;

    void Awake()
    {
        transform.parent = null;
        GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.VelocityChange);
        player = Player.Instance;
        playerObj = player.gameObject;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject == playerObj)
        {
            player.GetDamage(damage);
        }
        Destroy(gameObject);
    }
}
