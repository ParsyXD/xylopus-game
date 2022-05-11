using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.EditorTools;

[ExecuteInEditMode]
public class DrawCollider : MonoBehaviour
{
    [SerializeField] private Collider coll;
    [SerializeField] private Color color;
    [Range(0, 0.5f)][SerializeField] private float alpha;
    [SerializeField] private bool showWireFrame;
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(color.r, color.g, color.b, alpha);
        Gizmos.DrawCube(transform.position, coll.bounds.size);
        if(showWireFrame)
        {
            Gizmos.color = new Color(color.r, color.g, color.b, 0.5f);
            Gizmos.DrawWireCube(transform.position, coll.bounds.size);
        }
    }
}
