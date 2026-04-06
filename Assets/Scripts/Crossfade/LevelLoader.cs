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

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;

        float timer = 0;

        Resources.UnloadUnusedAssets();

        while (timer < transitionTime || operation.progress < 0.9f)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}