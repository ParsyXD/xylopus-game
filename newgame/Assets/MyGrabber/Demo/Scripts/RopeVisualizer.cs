using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeVisualizer : MonoBehaviour
{
    private LineRenderer lr;
    private Rigidbody rb;
    private SpringJoint sj;

    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        sj = GetComponentInChildren<SpringJoint>();
        lr.positionCount = 2;
    }

    void Update()
    {
        Vector3[] positions = new Vector3[2];
        positions[0] = transform.position + Vector3.up * -.45f;
        positions[1] = rb.position + rb.transform.up * .5f;
        lr.SetPositions(positions);
    }
}
