using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public List<Missions> missions;
    public Stack<Missions> missionStack;

    public void HandleMissionsInitialization()
    {
        for(int i = 0; i < missions.Count; i++)
        {
            missions[i].status = MissionStatus.NotStarted;
            missionStack.Push(missions[i]);
        }

        StartMission(0);
    }

    public void StartMission(int index)
    {
        missions[index].status = MissionStatus.Ongoing;
    }

    public void CheckMissionProgress(int index)
    {
        if(missions[index].status == MissionStatus.Completed)
        {
            missionStack.Pop();
            index++;
            missions[index].status = MissionStatus.Ongoing;
        }
    }
}