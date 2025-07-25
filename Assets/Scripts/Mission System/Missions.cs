using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionStatus
{
    NotStarted,
    Ongoing,
    Completed
}

public abstract class Missions
{
    public string missionName;
    public MissionStatus status;

    public abstract void HandleMissionDetails();
}