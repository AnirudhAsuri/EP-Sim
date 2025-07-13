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

    [Range(0, 360)]
    [SerializeField] private float angle;

    public bool targetInVision;

    public bool hasReachedLastSeenPosition = false;
    public bool isSearching = false;
    [SerializeField] private float arrivalStatusThreshold;
    public Coroutine searchRoutine;

    /*private float searchTimer = 0f;
    private float searchInterval = 1.0f; // Time to look in each direction
    private int searchStep = 0;
    private readonly float[] searchAngles = { -90f, 90f, 180f, -90f, 90f, 180f }; // Do it twice
    private Quaternion startRotation;
    private Quaternion targetRotation;*/

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while(true)
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

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    targetInVision = true;
                    currentTargetPosition = target.position;
                    lastSeenTargetPosition = currentTargetPosition;
                }
                else
                {
                    targetInVision = false;
                    currentTargetPosition = lastSeenTargetPosition;
                }
            }
            else
            {
                targetInVision = false;
                currentTargetPosition = lastSeenTargetPosition;
            }
        }
        else if (targetInVision)
        {
            targetInVision = false;
            currentTargetPosition = transform.position;
        }
    }

    /*public void UpdatePositionArrivalStatus()
    {
        if(!hasReachedLastSeenPosition && !targetInVision)
        {
            float distance = Vector3.Distance(transform.position, lastSeenTargetPosition);

            if(distance < arrivalStatusThreshold)
            {
                hasReachedLastSeenPosition = true;
            }
        }

        else if(targetInVision)
        {
            hasReachedLastSeenPosition = false;
        }
    }

    public void SearchForTarget()
    {
        if (hasReachedLastSeenPosition)
            isSearching = true;
        else
            isSearching = false;

        if (!isSearching)
        {
            isSearching = true;
            searchTimer = 0f;
            searchStep = 0;
            startRotation = transform.rotation;
            targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + searchAngles[searchStep], 0f);
        }

        if(isSearching)
        {
            searchTimer += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 120f * Time.deltaTime);

            if (searchTimer >= searchInterval)
            {
                searchStep++;
                searchTimer = 0f;

                if (searchStep < searchAngles.Length)
                {
                    startRotation = transform.rotation;
                    targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + searchAngles[searchStep], 0f);
                }
                else
                {
                    isSearching = false;
                }
            }
        }
    }*/

    public void AlertFromAttack(Vector3 attackerPosition)
    {
        currentTargetPosition = attackerPosition;
        targetInVision = true;
        lastSeenTargetPosition = attackerPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (!showRadiusGizmo) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);

        Gizmos.color = Color.cyan;
        Vector3 forward = transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0f, -angle * 0.5f, 0f) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0f, angle * 0.5f, 0f) * forward;

        Gizmos.DrawRay(transform.position, leftBoundary * radius);
        Gizmos.DrawRay(transform.position, rightBoundary * radius);
        int steps = Mathf.Max(5, Mathf.CeilToInt(angle / 10f)); 
        Vector3 prevDir = leftBoundary;
        for (int i = 1; i <= steps; i++)
        {
            float stepAngle = -angle * 0.5f + (angle / steps) * i;
            Vector3 nextDir = Quaternion.Euler(0f, stepAngle, 0f) * forward;
            Gizmos.DrawLine(transform.position + prevDir * radius,
                            transform.position + nextDir * radius);
            prevDir = nextDir;
        }
    }
}