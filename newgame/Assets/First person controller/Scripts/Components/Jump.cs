﻿using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField]
    GroundCheck groundCheck;
    Rigidbody _rigidbody;
    public float jumpStrength = 2;
    public event System.Action Jumped;


    void Reset()
    {
        groundCheck = GetComponentInChildren<GroundCheck>();
        if (!groundCheck)
            groundCheck = GroundCheck.Create(transform);
    }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if (Input.GetButtonDown("Jump") && groundCheck.isGrounded)
        {
            _rigidbody.AddForce(Vector3.up * 100 * jumpStrength);
            Jumped?.Invoke();
        }
    }
}
