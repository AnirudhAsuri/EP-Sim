using Cinemachine;
using System.Collections;
using UnityEngine;
public class PowerUpEffects : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook thirdPersonCamera;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Renderer playerRenderer;
    private PlayerHealth playerHealth;
    private PlayerAttacking playerAttacking;
    private PlayerUI playerUI;

    [Header("Giant Power Up")]
    [SerializeField] private float giantScaleMultiplier = 2.5f;
    [SerializeField] private float growthDuration = 5f;
    private Vector2 originalTopRig;
    private Vector2 originalMiddleRig;
    private Vector2 originalBottomRig;
    [SerializeField] private Vector2 giantTopRig;
    [SerializeField] private Vector2 giantMiddleRig;
    [SerializeField] private Vector2 giantBottomRig;
    public float giantDamage = 14f;
    public float giantHealth = 175f;
    private float giantDuration = 30f;
    private Coroutine giantTimer;
    [SerializeField] private AudioClip giantToNormalAudio;
    public bool isGiant = false;

    [Header("Speed Power Up")]
    public float powerUpSpeedMultiplier = 2f;
    public float powerUpSpeedLimitMultiplier = 2f;
    [SerializeField] private GameObject speedCirclet;
    private float speedDuration = 30f;
    private Coroutine speedTimer;
    public bool isSpedUp = false;

    [Header("Invulnerability")]
    [SerializeField] private GameObject invulnerabilityShield;
    private float invulnerabilityDuration = 30f;
    private Coroutine invulnerabilityTimer;
    public bool isInvulnerable = false;

    [Header("Health Regen")]
    [SerializeField] private float healthRegenValue = 50f;

    [Header("Terrain Negations")]
    private Material defaultMat;
    [SerializeField] private Material sandMat;
    [SerializeField] private Material iceMat;
    private float ignoreTerrainDuration = 30f;
    private Coroutine ignoreTerrainTimer;
    public bool ignoreSand = false;
    public bool ignoreIce = false;

    private Coroutine currentScaleRoutine;

    private void Start()
    {
        originalTopRig = new Vector2(thirdPersonCamera.m_Orbits[0].m_Height, thirdPersonCamera.m_Orbits[0].m_Radius);
        originalMiddleRig = new Vector2(thirdPersonCamera.m_Orbits[1].m_Height, thirdPersonCamera.m_Orbits[1].m_Radius);
        originalBottomRig = new Vector2(thirdPersonCamera.m_Orbits[2].m_Height, thirdPersonCamera.m_Orbits[2].m_Radius);

        playerHealth = GetComponentInParent<PlayerHealth>();
        playerAttacking = GetComponentInParent<PlayerAttacking>();
        playerUI = GetComponentInParent<PlayerUI>();
        speedCirclet.SetActive(false);
        invulnerabilityShield.SetActive(false);

        defaultMat = playerRenderer.material;
    }

    public void ActivateSpeedPowerUp()
    {
        isSpedUp = true;
        speedCirclet.SetActive(true);
        speedTimer = StartCoroutine(PowerUpTimer(speedDuration, () =>
        {
            DeactivateSpeedPowerUp();
            speedTimer = null;
        }));
    }

    private void DeactivateSpeedPowerUp()
    {
        isSpedUp = false;
        speedCirclet.SetActive(false);
    }

    public void ActivateInvulnerability()
    {
        isInvulnerable = true;
        invulnerabilityShield.SetActive(true);
        invulnerabilityTimer = StartCoroutine(PowerUpTimer(invulnerabilityDuration, () =>
        {
            DeactivateInvulnerability();
            invulnerabilityTimer = null;
        }));
    }

    private void DeactivateInvulnerability()
    {
        isInvulnerable = false;
        invulnerabilityShield.SetActive(false);
    }

    public void ActivateIgnoreSand()
    {
        if(ignoreIce == true)
        {
            DeactivateIgnoreIce();
        }

        ignoreSand = true;
        playerRenderer.material = sandMat;
        ignoreTerrainTimer = StartCoroutine(PowerUpTimer(ignoreTerrainDuration, () =>
        {
            DeactivateIgnoreSand();
            ignoreTerrainTimer = null;
        }));
    }

    private void DeactivateIgnoreSand()
    {
        ignoreSand = false;
        playerRenderer.material = defaultMat;
    }

    public void ActivateIgnoreIce()
    {
        if(ignoreSand == true)
        {
            DeactivateIgnoreSand();
        }

        ignoreIce = true;
        playerRenderer.material = iceMat;
        ignoreTerrainTimer = StartCoroutine(PowerUpTimer(ignoreTerrainDuration, () =>
        {
            DeactivateIgnoreIce();
            ignoreTerrainTimer = null;
        }));
    }

    private void DeactivateIgnoreIce()
    {
        ignoreIce = false;
        playerRenderer.material = defaultMat;
    }

    public void RegenHealth()
    {
        playerHealth.HealthRegenPowerUp(healthRegenValue);
    }

    public void MakeGiant()
    {
        ChangeSize(playerTransform, Vector3.one * giantScaleMultiplier, true);
        isGiant = true;

        playerAttacking.HandleGiantDamage(giantDamage);
        playerHealth.HandleGiantHealth(giantHealth);
        playerUI.UpdateHealthSliderTotalHealth(giantHealth);
        giantTimer = StartCoroutine(PowerUpTimer(giantDuration, () =>
        {
            ResetSize();
            giantTimer = null;
        }));
    }

    private void ResetSize()
    {
        ChangeSize(playerTransform, Vector3.one, false);
        isGiant = false;

        SoundFXManager.instance.PlaySoundEffect(giantToNormalAudio, transform, 0.5f);

        playerAttacking.HandleGiantDamage(playerAttacking.defaultDamage);
        playerHealth.HandleGiantHealth(playerHealth.defaultHealth);
        playerUI.UpdateHealthSliderTotalHealth(playerHealth.defaultHealth);
    }

    private void ChangeSize(Transform target, Vector3 targetScale, bool becomingGiant)
    {
        if (currentScaleRoutine != null) StopCoroutine(currentScaleRoutine);
        currentScaleRoutine = StartCoroutine(ScalePlayerRoutine(target, targetScale, becomingGiant));
    }

    private IEnumerator ScalePlayerRoutine(Transform target, Vector3 endScale, bool becomingGiant)
    {
        Vector3 startScale = target.localScale;

        Vector2 startTop = becomingGiant ? originalTopRig : giantTopRig;
        Vector2 endTop = becomingGiant ? giantTopRig : originalTopRig;

        Vector2 startMid = becomingGiant ? originalMiddleRig : giantMiddleRig;
        Vector2 endMid = becomingGiant ? giantMiddleRig : originalMiddleRig;

        Vector2 startBot = becomingGiant ? originalBottomRig : giantBottomRig;
        Vector2 endBot = becomingGiant ? giantBottomRig : originalBottomRig;

        float elapsedTime = 0;

        while (elapsedTime < growthDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / growthDuration;

            float smoothT = Mathf.SmoothStep(0, 1, t);

            // Scale the Player
            target.localScale = Vector3.Lerp(startScale, endScale, smoothT);

            // Top Rig
            thirdPersonCamera.m_Orbits[0].m_Height = Mathf.Lerp(startTop.x, endTop.x, smoothT);
            thirdPersonCamera.m_Orbits[0].m_Radius = Mathf.Lerp(startTop.y, endTop.y, smoothT);

            // Middle Rig
            thirdPersonCamera.m_Orbits[1].m_Height = Mathf.Lerp(startMid.x, endMid.x, smoothT);
            thirdPersonCamera.m_Orbits[1].m_Radius = Mathf.Lerp(startMid.y, endMid.y, smoothT);

            // Bottom Rig
            thirdPersonCamera.m_Orbits[2].m_Height = Mathf.Lerp(startBot.x, endBot.x, smoothT);
            thirdPersonCamera.m_Orbits[2].m_Radius = Mathf.Lerp(startBot.y, endBot.y, smoothT);

            yield return null;
        }

        target.localScale = endScale;
        currentScaleRoutine = null;
    }

    private IEnumerator PowerUpTimer(float duration, System.Action onComplete)
    {
        yield return new WaitForSecondsRealtime(duration);
        onComplete?.Invoke();
    }
}