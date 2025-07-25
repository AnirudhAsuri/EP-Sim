using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<Levels> levels;

    public void HandleLevelsInitialisation()
    {
        for(int i = 0; i < levels.Count; i++)
        {
            if(levels[i].status != LevelStatus.Completed)
            {
                levels[i].status = LevelStatus.NotStarted;
            }
        }
    }

    public void StartLevel(int index)
    {
        levels[index].status = LevelStatus.Ongoing;
        levels[index].missionManager.HandleMissionsInitialization();
    }

    public void CheckLevelProgress(int index)
    {
        if(levels[index].missionManager.missionStack.Count == 0)
        {
            levels[index].status = LevelStatus.Completed;
        }
    }
}