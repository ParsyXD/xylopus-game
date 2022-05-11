using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectenemy : MonoBehaviour
{

    public homingbulletvse homingbullet;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Enemy")
    {
        homingbullet.enemy = coll.gameObject.transform;
    }
    }
}
