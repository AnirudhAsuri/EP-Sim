using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetectionSystem : MonoBehaviour
{
    public Collider[] obstacleColliders;

    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float detectionRadius;

    [SerializeField] private bool showGizmo = false;

    public void HandleObstacleDetection()
    {
        obstacleColliders = Physics.OverlapSphere(transform.position, detectionRadius, obstacleMask);
    }

    private void OnDrawGizmos()
    {
        if (showGizmo == false)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}