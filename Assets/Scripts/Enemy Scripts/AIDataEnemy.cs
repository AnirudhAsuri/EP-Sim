using System.Collections.Generic;
using UnityEngine;

public class AIDataEnemy : MonoBehaviour
{
    public List<Transform> targets = null;
    public Collider[] obstacles = null;

    public Transform currentTarget;

    public int GetTargetsCount() => targets == null ? 0 : targets.Count;
}