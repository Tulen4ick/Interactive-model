using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMaterialController : MonoBehaviour
{
    private SelectionManager selectionManager;

    public void Initialize(SelectionManager manager)
    {
        selectionManager = manager;
    }

    public void SwitchMaterialOnSelected()
    {
        foreach (GameObject obj in selectionManager.selectedObjects)
        {
            MaterialInstancer materialInstancer = obj.GetComponent<MaterialInstancer>();
            if (materialInstancer != null)
            {
                materialInstancer.SwitchMaterial();
            }
        }
    }
}
