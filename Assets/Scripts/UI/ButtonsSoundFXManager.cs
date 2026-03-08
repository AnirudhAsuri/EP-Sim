using UnityEngine;

public class ButtonsSoundFXManager : MonoBehaviour
{
    private static ButtonsSoundFXManager instance;

    [SerializeField] private AudioClip buttonHoverClip;
    [SerializeField] private AudioClip buttonClickClip;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayHoverSoundEffect()
    {
        audioSource.PlayOneShot(buttonHoverClip);
    }

    public void PlayClickSoundEffect()
    {
        audioSource.PlayOneShot(buttonClickClip);
    }

}
