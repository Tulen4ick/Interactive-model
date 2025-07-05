using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstancer : MonoBehaviour
{
    [SerializeField] private Material[] sourceMaterials;
    [SerializeField] private string alphaProperty = "_BaseColor";

    private Material[] instanceMaterials;
    private Renderer objectRenderer;
    private int currentMaterialIndex = 0;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        instanceMaterials = new Material[sourceMaterials.Length];
        for (int i = 0; i < sourceMaterials.Length; i++)
        {
            instanceMaterials[i] = new Material(sourceMaterials[i]);
        }
        ApplyMaterial(0);
    }

    public void SwitchMaterial()
    {
        int nextIndex = (currentMaterialIndex + 1) % instanceMaterials.Length;
        ApplyMaterial(nextIndex);
    }
    private void ApplyMaterial(int newIndex)
    {
        float currentAlpha = objectRenderer.material.GetColor(alphaProperty).a;

        currentMaterialIndex = newIndex;
        objectRenderer.material = instanceMaterials[newIndex];

        Color newColor = objectRenderer.material.GetColor(alphaProperty);
        newColor.a = currentAlpha;
        objectRenderer.material.SetColor(alphaProperty, newColor);
    }

    public void SetAlpha(float alphaValue)
    {
        Color currentColor = objectRenderer.material.GetColor(alphaProperty);
        currentColor.a = Mathf.Clamp01(alphaValue);
        objectRenderer.material.SetColor(alphaProperty, currentColor);
    }
}
