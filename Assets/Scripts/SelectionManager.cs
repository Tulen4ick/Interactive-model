using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public Sprite SelectActive;
    public Sprite SelectInactive;

    public Sprite ViewSingleActive;
    public Sprite ViewSingleInactive;

    public Sprite ViewAllActive;
    public Sprite ViewAllInactive;

    public RectTransform prefab;
    public ScrollRect scrollView;
    public Transform controlPanel;

    public Material firstMaterial;
    public Material secondMaterial;

    public List<GameObject> selectedObjects = new List<GameObject>();
    void Start()
    {
        if (controlPanel != null && controlPanel.GetComponent<ControlPanelController>() != null)
        {
            controlPanel.GetComponent<ControlPanelController>().Initialize(this);
            var transparentSection = controlPanel.Find("TransparentSection");
            if (transparentSection!= null)
            {
                foreach (Transform child in transparentSection)
                {
                    if (child.GetComponentInChildren<TransparencyController>() != null)
                    {
                        child.GetComponentInChildren<TransparencyController>().Initialize(this, transparentSection, child);
                    }
                }
            }
        }
        PopulateScrollView();
    }

    private void PopulateScrollView()
    {
        foreach (Transform child in scrollView.content)
        {
            Destroy(child.gameObject);
        }

        GameObject[] selectableObjects = GameObject.FindGameObjectsWithTag("Selectable");
        foreach (GameObject obj in selectableObjects)
        {

            if (!string.IsNullOrEmpty(obj.name))
            {
                CreateListItem(obj);
            }
        }
    }

    void CreateListItem(GameObject obj)
    {
        var instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
        instance.transform.SetParent(scrollView.content.transform, false);
        PrefabController prefabConntroller = instance.GetComponent<PrefabController>();
        if (prefabConntroller != null )
        {
            prefabConntroller.Initialize(obj, this, controlPanel.GetComponent<ControlPanelController>());
        }
        TextMeshProUGUI textComponent = instance.transform.Find("ObjectName").GetComponent<TextMeshProUGUI>();
        textComponent.text = obj.name;
    }

    public void ToggleObjectSelection(GameObject obj)
    {
        if (selectedObjects.Contains(obj))
        {
            selectedObjects.Remove(obj);
        }
        else
        {
            selectedObjects.Add(obj);
        }
    }

    public void OnOffAllObjectsInScroll(bool AllSelected)
    {
        foreach (Transform child in scrollView.content)
        {
            Transform SelectButton = child.Find("Select");
            if (AllSelected)
            {
                SelectButton.GetComponent<Image>().sprite = SelectActive;
                if (child.GetComponentInChildren<UnityEngine.UI.Outline>() != null)
                {
                    child.GetComponentInChildren<UnityEngine.UI.Outline>().enabled = true;
                }
            }
            else
            {
                SelectButton.GetComponent<Image>().sprite = SelectInactive;
                if (child.GetComponentInChildren<UnityEngine.UI.Outline>() != null)
                {
                    child.GetComponentInChildren<UnityEngine.UI.Outline>().enabled = false;
                }
            }
        }
    }

    public void OnOffAllViewInScroll(bool AllView)
    {
        foreach (Transform child in scrollView.content)
        {
            PrefabController prefabController = child.GetComponent<PrefabController>();
            Transform ViewButton = child.Find("Visibility");
            if (prefabController != null && ViewButton.GetComponent<Image>() != null)
            {
                if (AllView)
                {
                    prefabController.targetObject.SetActive(true);
                    ViewButton.GetComponent<Image>().sprite = ViewSingleActive;
                }
                else
                {
                    prefabController.targetObject.SetActive(false);
                    ViewButton.GetComponent<Image>().sprite = ViewSingleInactive;
                }
            }
        }
    }

    public bool IsAllSelected()
    {
        GameObject[] selectableObjects = GameObject.FindGameObjectsWithTag("Selectable");
        Transform SelectAllButton = controlPanel.Find("AddDeleteAll");
        foreach (GameObject obj in selectableObjects)
        {
            if (!selectedObjects.Contains(obj))
            {
                if (controlPanel.GetComponent<ControlPanelController>() != null)
                {
                    controlPanel.GetComponent<ControlPanelController>().AllSelected = false;
                }
                SelectAllButton.GetComponent<Image>().sprite = SelectInactive;
                return false;
            }
        }
        if (controlPanel.GetComponent<ControlPanelController>() != null)
        {
            controlPanel.GetComponent<ControlPanelController>().AllSelected = true;
        }
        SelectAllButton.GetComponent<Image>().sprite = SelectActive;
        return true;
    }

    public bool IsAllView()
    {
        Transform ViewAllButton = controlPanel.Find("ShowHideAll");
        foreach (Transform obj in scrollView.content)
        {
            if (obj.GetComponent<PrefabController>() != null)
            {
                if (!obj.GetComponent<PrefabController>().targetObject.activeInHierarchy)
                {
                    if (controlPanel.GetComponent<ControlPanelController>() != null)
                    {
                        controlPanel.GetComponent<ControlPanelController>().AllView = false;
                    }
                    ViewAllButton.GetComponent<Image>().sprite = ViewAllInactive;
                    return false;
                }
            }
        }
        if (controlPanel.GetComponent<ControlPanelController>() != null)
        {
            controlPanel.GetComponent<ControlPanelController>().AllView = true;
        }
        ViewAllButton.GetComponent<Image>().sprite = ViewAllActive;
        return true;
    }

}
