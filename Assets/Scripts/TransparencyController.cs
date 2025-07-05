using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Transform transparentSection;
    public Transform currentButton;

    private SelectionManager selectionManager;

    public void Initialize(SelectionManager manager, Transform section, Transform button)
    {
        selectionManager = manager;
        transparentSection = section;
        currentButton = button;
    }

    public void TransparencyClick(float alpha)
    {
        foreach (Transform child in transparentSection)
        {
            if (child.GetComponent<UnityEngine.UI.Outline>() != null)
            {
                child.GetComponent<UnityEngine.UI.Outline>().enabled = false;
            }
        }
        if (currentButton.GetComponent<UnityEngine.UI.Outline>() != null)
        {
            currentButton.GetComponent<UnityEngine.UI.Outline>().enabled = true;
        }
        foreach (GameObject obj in selectionManager.selectedObjects)
        {
            Material material = obj.GetComponent<Renderer>().material;
            material.SetColor("Color", new Color(material.color.r, material.color.g, material.color.b, alpha));
        }
    }
}
