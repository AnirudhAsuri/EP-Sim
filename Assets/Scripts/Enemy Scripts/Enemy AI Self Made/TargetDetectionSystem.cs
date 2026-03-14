using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetectionSystem : MonoBehaviour
{
    public Vector3 currentTargetPosition;
    private Vector3 lastSeenTargetPosition;
    private Collider[] targets;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    [SerializeField] private bool showRadiusGizmo = false;
    [SerializeField] private float radius;

    // Peripheral Vision Settings
    [SerializeField] private float peripheralRadius = 10f; // Distance for the 360 persistence

    [Range(0, 360)]
    [SerializeField] private float angle;

    public bool targetInVision;
    public bool hasReachedLastSeenPosition = false;
    public bool isSearching = false;

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        targets = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (targets.Length != 0)
        {
            Transform target = targets[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // 1. Check Standard Cone Vision
            bool inCone = Vector3.Angle(transform.forward, directionToTarget) < angle / 2;

            // 2. Check Peripheral Persistence (Only if we ALREADY saw them and they are close)
            bool inPeripheralRange = targetInVision && (distanceToTarget <= peripheralRadius);

            // If in cone OR in peripheral range, check for line of sight (obstacles)
            if (inCone || inPeripheralRange)
            {
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    targetInVision = true;
                    currentTargetPosition = target.position;
                    lastSeenTargetPosition = currentTargetPosition;
                    return; // Exit early since we found them
                }
            }
        }

        // If we reach here, the target is either blocked, out of range, or out of view
        targetInVision = false;
        currentTargetPosition = lastSeenTargetPosition;
    }

    public void AlertFromAttack(Vector3 attackerPosition)
    {
        currentTargetPosition = attackerPosition;
        targetInVision = true;
        lastSeenTargetPosition = attackerPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (!showRadiusGizmo) return;

        // Main Vision Radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);

        // Peripheral Persistence Radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, peripheralRadius);

        // Cone Visualization
        Gizmos.color = Color.cyan;
        Vector3 forward = transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0f, -angle * 0.5f, 0f) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0f, angle * 0.5f, 0f) * forward;

        Gizmos.DrawRay(transform.position, leftBoundary * radius);
        Gizmos.DrawRay(transform.position, rightBoundary * radius);
    }
}