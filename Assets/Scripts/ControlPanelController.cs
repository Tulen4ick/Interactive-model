using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelController : MonoBehaviour
{
    private SelectionManager selectionManager;
    public bool AllSelected;
    public bool AllView;
    public Transform SelectAllButton;
    public Transform ViewAllButton;
    

    public void Initialize(SelectionManager manager)
    {
        selectionManager = manager;
        AllSelected = false;
        AllView = true;
        SelectAllButton = transform.Find("AddDeleteAll");
        ViewAllButton = transform.Find("ShowHideAll");
    }

    public void SelectAllButtonClick()
    {
        selectionManager.selectedObjects.Clear();
        if (AllSelected)
        {
            AllSelected = false;
            SelectAllButton.GetComponent<Image>().sprite = selectionManager.SelectInactive;
        }
        else
        {
            GameObject[] selectableObjects = GameObject.FindGameObjectsWithTag("Selectable");
            foreach (GameObject obj in selectableObjects)
            {
                selectionManager.selectedObjects.Add(obj);
            }
            AllSelected = true;
            SelectAllButton.GetComponent<Image>().sprite = selectionManager.SelectActive;
        }
        selectionManager.OnOffAllObjectsInScroll(AllSelected);
    }

    public void ViewAllButtonClick()
    {
        GameObject[] selectableObjects = GameObject.FindGameObjectsWithTag("Selectable");
        AllView = !AllView;
        ViewAllButton.GetComponent<Image>().sprite = AllView
            ? selectionManager.ViewAllActive
            : selectionManager.ViewAllInactive;
        foreach (GameObject obj in selectableObjects)
        {
            obj.SetActive(AllView);
        }
        selectionManager.OnOffAllViewInScroll(AllView);
    }
}
