using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelStatus
{
    NotStarted,
    Ongoing,
    Completed
}

public class Levels : MonoBehaviour
{
    public string levelName;
    public LevelStatus status;
    public MissionManager missionManager;
}