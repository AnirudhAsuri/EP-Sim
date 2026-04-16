using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VictorySnapshot : MonoBehaviour
{
    [SerializeField] private WinManager winManager;

    public Camera photoCamera;
    public RawImage uiDisplay;
    private GameObject winCanvas;
    public Image flashOverlay;

    [SerializeField] private AudioSource levelMusicPlayer;
    private string levelMusicTag = "Level Music Source";

    private void Awake()
    {
        photoCamera = GetComponentInChildren<Camera>();
        winManager = FindObjectOfType<WinManager>();
        winCanvas = winManager.gameObject;
        uiDisplay = winCanvas.GetComponentInChildren<RawImage>(true);
        flashOverlay = winManager.flashImage;
        levelMusicPlayer = GameObject.FindWithTag(levelMusicTag).GetComponent<AudioSource>();
    }

    public void TakeVictoryPicture()
    {
        levelMusicPlayer.Stop();

        flashOverlay.gameObject.SetActive(true);

        Texture2D staticPhoto = CaptureStaticImage(photoCamera);

        uiDisplay.texture = staticPhoto;

        if(winManager != null)
            winManager.EnableWinScreen();
    }

    Texture2D CaptureStaticImage(Camera cam)
    {
        RenderTexture activeRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;

        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height, TextureFormat.RGB24, false);

        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();

        RenderTexture.active = activeRT;
        return image;
    }
}