using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VictorySnapshot : MonoBehaviour
{
    [SerializeField] private WinManager winManager;

    public Camera photoCamera;
    public RawImage uiDisplay;
    public GameObject winCanvas;
    public Image flashOverlay;

    [SerializeField] private AudioSource levelMusicPlayer;

    public void TakeVictoryPicture()
    {
        levelMusicPlayer.gameObject.SetActive(false);

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