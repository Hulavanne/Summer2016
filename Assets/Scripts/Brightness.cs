using System;
using UnityEngine;

[ExecuteInEditMode]
//[AddComponentMenu("Image Effects/Color Adjustments/Brightness")]
public class Brightness : MonoBehaviour
{
    // Provides a shader property that is set in the inspector
    // and a material instantiated from the shader
    public Shader shader;
    Material shaderMaterial;

    [Range(0.0f, 10.0f)]
    public float brightness = 1f;

    void Start()
    {
        // Disable if we don't support image effects
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }

        // Disable the image effect if the shader can't
        // run on the users graphics card
        if (!shader || !shader.isSupported)
        {
            enabled = false;
        }
    }

    Material material
    {
        get
        {
            if (shaderMaterial == null)
            {
                shaderMaterial = new Material(shader);
                shaderMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return shaderMaterial;
        }
    }

    void OnDisable()
    {
        if (shaderMaterial)
        {
            DestroyImmediate(shaderMaterial);
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_Brightness", brightness);
        Graphics.Blit(source, destination, material);
    }
}