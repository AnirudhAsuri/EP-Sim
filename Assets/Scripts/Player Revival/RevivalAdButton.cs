using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RevivalAdButton : MonoBehaviour
{
    private Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();

        if(RevivalState.Instance != null && RevivalState.Instance.hasRevived)
        {
            button.interactable = false;
        }
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        /*PlayerRevivalManager revival = FindObjectOfType<PlayerRevivalManager>();
        revival.RevivePlayer();*/

        if(MyAdsManager.Instance != null)
        {
            MyAdsManager.Instance.ShowRewarded();
        }
    }
}