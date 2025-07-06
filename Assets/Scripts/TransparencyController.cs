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
        selectionManager.TransparencySectionHide();
        if (selectionManager.selectedObjects.Count > 0)
        {
            if (currentButton.GetComponent<UnityEngine.UI.Outline>() != null)
            {
                currentButton.GetComponent<UnityEngine.UI.Outline>().enabled = true;
            }
            foreach (GameObject obj in selectionManager.selectedObjects)
            {
                MaterialInstancer materialInstancer = obj.GetComponent<MaterialInstancer>();
                if (materialInstancer != null)
                {
                    materialInstancer.SetAlpha(alpha);
                }
            }
        }
    }
    
}
