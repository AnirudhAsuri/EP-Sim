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

    private Dictionary<Renderer, MaterialData[]> trackedMaterials = new Dictionary<Renderer, MaterialData[]>();

    private class MaterialData
    {
        public Color originalColor;
        public int originalRenderQueue;
        public float currentAlpha;
    }

    private void Start()
    {
        foreach (var t in FindObjectsOfType<Transform>())
        {
            if (t.CompareTag(playerTag))
            {
                playerTransform = t;
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
                SetupFadeMaterial(rend);
            }

            UpdateAlpha(rend, targetAlpha);
        }

        List<Renderer> toRemove = new List<Renderer>();
        foreach (var rend in trackedMaterials.Keys)
        {
            if (!hitsThisFrame.Contains(rend))
            {
                UpdateAlpha(rend, 1.0f);

                // Check if the first material has returned to opaque to decide when to reset
                if (trackedMaterials[rend][0].currentAlpha >= 0.99f)
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
        Material[] mats = rend.materials; // Accesses all 5 materials
        MaterialData[] dataArray = new MaterialData[mats.Length];

        for (int i = 0; i < mats.Length; i++)
        {
            dataArray[i] = new MaterialData
            {
                originalColor = mats[i].color,
                originalRenderQueue = mats[i].renderQueue,
                currentAlpha = 1.0f
            };

            // Standard Shader "Fade" mode settings for each material
            mats[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mats[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mats[i].SetInt("_ZWrite", 0);
            mats[i].DisableKeyword("_ALPHATEST_ON");
            mats[i].EnableKeyword("_ALPHABLEND_ON");
            mats[i].renderQueue = 3000;
        }

        trackedMaterials.Add(rend, dataArray);
    }

    void UpdateAlpha(Renderer rend, float goal)
    {
        MaterialData[] dataArray = trackedMaterials[rend];
        Material[] mats = rend.materials;

        for (int i = 0; i < mats.Length; i++)
        {
            dataArray[i].currentAlpha = Mathf.MoveTowards(dataArray[i].currentAlpha, goal, fadeSpeed * Time.deltaTime);
            Color c = dataArray[i].originalColor;
            c.a = dataArray[i].currentAlpha;
            mats[i].color = c;
        }
    }

    void ResetMaterial(Renderer rend)
    {
        Material[] mats = rend.materials;
        MaterialData[] dataArray = trackedMaterials[rend];

        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mats[i].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mats[i].SetInt("_ZWrite", 1);
            mats[i].DisableKeyword("_ALPHABLEND_ON");
            mats[i].renderQueue = -1;
            mats[i].color = dataArray[i].originalColor;
        }
    }
}