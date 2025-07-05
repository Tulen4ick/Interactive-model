using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PrefabController : MonoBehaviour
{

    public GameObject targetObject;
    private SelectionManager selectionManager;
    private ControlPanelController panelController;
    private Transform SelectButton;
    private Transform ViewButton;

    public void Initialize(GameObject obj, SelectionManager manager, ControlPanelController controller)
    {
        targetObject = obj;
        selectionManager = manager;
        panelController = controller;

        SelectButton = transform.Find("Select");
        ViewButton = transform.Find("Visibility");

        if (SelectButton != null)
        {
            SelectButton.GetComponent<Button>().onClick.AddListener(SelectClick);
        }
        if (ViewButton != null)
        {
            ViewButton.GetComponent<Button>().onClick.AddListener(ViewClick);
        }
    }

    private void SelectClick()
    {
        selectionManager.ToggleObjectSelection(targetObject);
        if (selectionManager.selectedObjects.Contains(targetObject))
        {
            SelectButton.GetComponent<Image>().sprite = selectionManager.SelectActive;
            if (transform.GetComponentInChildren<UnityEngine.UI.Outline>() != null)
            {
                transform.GetComponentInChildren<UnityEngine.UI.Outline>().enabled = true;
            }
        }
        else
        {
            SelectButton.GetComponent<Image>().sprite = selectionManager.SelectInactive;
            if (transform.GetComponentInChildren<UnityEngine.UI.Outline>() != null)
            {
                transform.GetComponentInChildren<UnityEngine.UI.Outline>().enabled = false;
            }
        }
        selectionManager.IsAllSelected();
    }

    private void ViewClick()
    {
        if (targetObject.activeInHierarchy)
        {
            targetObject.SetActive(false);
            ViewButton.GetComponent<Image>().sprite = selectionManager.ViewSingleInactive;
        }
        else
        {
            targetObject.SetActive(true);
            ViewButton.GetComponent<Image>().sprite = selectionManager.ViewSingleActive;
        }
        selectionManager.IsAllView();
    }
}
