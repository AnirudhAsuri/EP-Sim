using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class IgnoreObstacles : MonoBehaviour
{
    private Transform playerTransform;
    public LayerMask obstacleLayers;
    [Range(0, 1)] public float targetAlpha = 0.3f;
    public float fadeSpeed = 5f;

    private string playerTag = "Player";

    private Dictionary<Renderer, MaterialData> trackedMaterials = new Dictionary<Renderer, MaterialData>();

    private class MaterialData
    {
        public Color originalColor;
        public int originalRenderQueue;
        public float currentAlpha;
    }

    private void Start()
    {
        foreach (var transform in FindObjectsOfType<Transform>())
        {
            if (transform.CompareTag(playerTag))
            {
                playerTransform = transform;
                break;
            }
        }
    }

    void LateUpdate()
    {
        if (playerTransform == null)
            return;

        Vector3 dir = playerTransform.position - transform.position;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, dir, dir.magnitude, obstacleLayers);
        HashSet<Renderer> hitsThisFrame = new HashSet<Renderer>();

        foreach (var hit in hits)
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend == null) continue;
            hitsThisFrame.Add(rend);

            if (!trackedMaterials.ContainsKey(rend))
            {
                // Initial setup: Change Opaque -> Fade mode
                SetupFadeMaterial(rend);
            }

            // Smoothly fade down
            UpdateAlpha(rend, targetAlpha);
        }

        // Restore materials no longer being hit
        List<Renderer> toRemove = new List<Renderer>();
        foreach (var rend in trackedMaterials.Keys)
        {
            if (!hitsThisFrame.Contains(rend))
            {
                UpdateAlpha(rend, 1.0f);
                if (trackedMaterials[rend].currentAlpha >= 0.99f)
                {
                    ResetMaterial(rend);
                    toRemove.Add(rend);
                }
            }
        }
        foreach (var r in toRemove) trackedMaterials.Remove(r);
    }

    void SetupFadeMaterial(Renderer rend)
    {
        Material mat = rend.material; // Creates a runtime instance
        MaterialData data = new MaterialData
        {
            originalColor = mat.color,
            originalRenderQueue = mat.renderQueue,
            currentAlpha = 1.0f
        };

        // Standard Shader "Fade" mode settings
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.renderQueue = 3000;

        trackedMaterials.Add(rend, data);
    }

    void UpdateAlpha(Renderer rend, float goal)
    {
        MaterialData data = trackedMaterials[rend];
        data.currentAlpha = Mathf.MoveTowards(data.currentAlpha, goal, fadeSpeed * Time.deltaTime);
        Color c = rend.material.color;
        c.a = data.currentAlpha;
        rend.material.color = c;
    }

    void ResetMaterial(Renderer rend)
    {
        Material mat = rend.material;
        // Restore to Opaque mode
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.renderQueue = -1;
        mat.color = trackedMaterials[rend].originalColor;
    }
}