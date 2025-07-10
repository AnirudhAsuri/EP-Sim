using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAIManager : MonoBehaviour
{
    public Vector3 targetPosition;
    [SerializeField] private float radius = 20f;
    [SerializeField] private float agentColliderSize = 2.55f;

    private TargetDetectionSystem targetDetectionSystem;
    private ObstacleDetectionSystem obstacleDetectionSystem;

    private LayerMask playerMask;
    [SerializeField] private Collider playerCollider;
    
    private Vector3 directionToTarget;
    private Vector3 targetPositionCached;
    [SerializeField] private bool showInterestGizmo = false;

    public Vector3 movementDirection;
    private float[] interestMap;
    private float[] dangerMap;

    [SerializeField] private bool showMovementDirection = false;

    [SerializeField] private bool showObstacleGizmo = false;
    private float[] dangerMapTemp = null;
    private float[] interestMapTemp = null;

    void Start()
    {
        targetDetectionSystem = GetComponentInChildren<TargetDetectionSystem>();
        obstacleDetectionSystem = GetComponentInChildren<ObstacleDetectionSystem>();

        dangerMap = new float[Directions.eightDirections.Count];
        interestMap = new float[Directions.eightDirections.Count];
    }

    void FixedUpdate()
    {
        targetPosition = targetDetectionSystem.currentTargetPosition;

        if ((targetPosition - transform.position).magnitude > 0.1f)
        {
            directionToTarget = targetPosition - transform.position;

            obstacleDetectionSystem.HandleObstacleDetection();

            movementDirection = HandleContext();
        }

        else
            movementDirection = Vector3.zero;
    }

    public float[] HandleDangerMap()
    {
        float[] dangerMapTemp = new float[Directions.eightDirections.Count];
        foreach(Collider obstacleCollider in obstacleDetectionSystem.obstacleColliders)
        {
            Vector3 directionToObstacle = obstacleCollider.ClosestPoint(transform.position) - transform.position;
            float distanceToObstacle = directionToObstacle.magnitude;
            float weight = distanceToObstacle <= agentColliderSize ? 1 : (radius - distanceToObstacle)/radius;

            Vector3 directionToObstacleNormalized = directionToObstacle.normalized;

            for(int i = 0; i < Directions.eightDirections.Count; i++)
            {
                float result = Mathf.Max(0, Vector3.Dot(directionToObstacleNormalized, Directions.eightDirections[i]));
                
                float valueToPutIn = result * weight;
                
                if(valueToPutIn > dangerMapTemp[i])
                {
                    dangerMapTemp[i] = valueToPutIn;
                }
            }
        }

        this.dangerMapTemp = dangerMapTemp;

        return dangerMapTemp;
    }

    public float[] HandleInterestMap()
    {
        interestMapTemp = new float[Directions.eightDirections.Count];
        float distanceToTarget = directionToTarget.magnitude;

        for(int i = 0; i < interestMap.Length; i++)
        {
            float result = Vector3.Dot(directionToTarget.normalized, Directions.eightDirections[i]);

            if(result > 0)
            {
                float valueToPutIn = result;
                if(valueToPutIn > interestMapTemp[i])
                {
                    interestMapTemp[i] = valueToPutIn;
                }
            }
        }

        return interestMapTemp;
    }

    public Vector3 HandleContext()
    {
        dangerMap = HandleDangerMap();
        interestMap = HandleInterestMap();

        for(int i = 0; i < interestMap.Length; i++)
        {
            interestMap[i] = Mathf.Clamp01(interestMap[i] - dangerMap[i]);
        }

        Vector3 outputDirection = Vector3.zero;

        for(int i = 0; i < interestMap.Length; i++)
        {
            outputDirection += Directions.eightDirections[i] * interestMap[i];
        }

        outputDirection.Normalize();

        return outputDirection;
    }

    private void OnDrawGizmos()
    { 
        if (Application.isPlaying)
        {
            if (dangerMapTemp != null && showObstacleGizmo)
            {
                Gizmos.color = Color.red;
                for (int i = 0; i < dangerMapTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * dangerMapTemp[i] * 10);
                }
            }

            if (showInterestGizmo && interestMapTemp != null)
            {
                Gizmos.color = Color.blue;
                for (int i = 0; i < interestMapTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Directions.eightDirections[i] * interestMapTemp[i] * 10);
                }
            }

            if(showMovementDirection)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(transform.position, movementDirection * 10);
            }
        }
    }

    public static class Directions
    {
        public static List<Vector3> eightDirections = new List<Vector3> {
        new Vector3(0, 0, 1).normalized,
        new Vector3(1, 0, 1).normalized,
        new Vector3(1, 0, 0).normalized,
        new Vector3(1, 0, -1).normalized,
        new Vector3(0, 0, -1).normalized,
        new Vector3(-1, 0, -1).normalized,
        new Vector3(-1, 0, 0).normalized,
        new Vector3(-1, 0, 1).normalized
        };
    }    
}