using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator crossfadeAnimator;
    private string crossfadeStartTrigger = "Start";

    private float transitionTime = 1f;

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadCrossfade(sceneName));
    }

    IEnumerator LoadCrossfade(string sceneName)
    {
        crossfadeAnimator.SetTrigger(crossfadeStartTrigger);

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }
}