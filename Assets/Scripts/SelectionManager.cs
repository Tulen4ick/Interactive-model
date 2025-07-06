using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    [Header("Selection Sprites")]
    public Sprite SelectActive;
    public Sprite SelectInactive;

    [Header("View Sprites")]
    public Sprite ViewSingleActive;
    public Sprite ViewSingleInactive;
    public Sprite ViewAllActive;
    public Sprite ViewAllInactive;

    [Header("UI References")]
    public RectTransform prefab;
    public ScrollRect scrollView;
    public Transform controlPanel;


    public List<GameObject> selectedObjects { get; } = new List<GameObject>();
    private ControlPanelController panelController;
    private Transform transparentSection;

    private void Start()
    {
        Initialized();
    }

    public void Initialized()
    {
        if (controlPanel != null)
        {
            panelController = controlPanel.GetComponent<ControlPanelController>();
            panelController?.Initialize(this);

            InitializeTransparencySection();
            InitializeMaterialSwitcher();
        }

        PopulateScrollView();
        UpdateAllViewState();
        TransparencySectionHide();
    }

    private void InitializeTransparencySection()
    {
        transparentSection = controlPanel.Find("TransparentSection");
        if (transparentSection == null) return;

        foreach (Transform child in transparentSection)
        {
            var controller = child.GetComponentInChildren<TransparencyController>();
            controller?.Initialize(this, transparentSection, child);
        }
    }

    private void InitializeMaterialSwitcher()
    {
        var switchColorsButton = controlPanel.Find("ChangeColor");
        var switchColorsScript = switchColorsButton?.GetComponent<SwitchMaterialController>();
        switchColorsScript?.Initialize(this);
    }

    private void PopulateScrollView()
    {
        foreach (Transform child in scrollView.content)
        {
            Destroy(child.gameObject);
        }

        foreach (var obj in GameObject.FindGameObjectsWithTag("Selectable"))
        {
            if (!string.IsNullOrEmpty(obj.name))
            {
                CreateListItem(obj);
            }
        }
    }

    private void CreateListItem(GameObject obj)
    {
        var instance = Instantiate(prefab.gameObject, scrollView.content.transform, false);
        var controller = instance.GetComponent<PrefabController>();
        controller?.Initialize(obj, this);

        var textComponent = instance.transform.Find("ObjectName")?.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = obj.name;
        }
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

    public void UpdateAllSelectionState()
    {
        bool allSelected = IsAllSelected();
        panelController.AllSelected = allSelected;
        UpdateSelectionUI(allSelected);
    }

    public void UpdateAllViewState()
    {
        bool allVisible = IsAllView();
        panelController.AllView = allVisible;
        UpdateVisibilityUI(allVisible);
    }

    private bool IsAllSelected()
    {
        var selectableObjects = GameObject.FindGameObjectsWithTag("Selectable");
        foreach (var obj in selectableObjects)
        {
            if (!selectedObjects.Contains(obj)) return false;
        }
        return true;
    }

    private bool IsAllView()
    {
        foreach (Transform item in scrollView.content)
        {
            var controller = item.GetComponent<PrefabController>();
            MeshRenderer renderer = controller.targetObject.GetComponent<MeshRenderer>();
            if (controller != null && !renderer.enabled)
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateSelectionUI(bool allSelected)
    {
        var selectAllButton = controlPanel.Find("AddDeleteAll")?.GetComponent<Image>();
        if (selectAllButton != null)
        {
            selectAllButton.sprite = allSelected ? SelectActive : SelectInactive;
        }
    }

    private void UpdateVisibilityUI(bool allVisible)
    {
        var viewAllButton = controlPanel.Find("ShowHideAll")?.GetComponent<Image>();
        if (viewAllButton != null)
        {
            viewAllButton.sprite = allVisible ? ViewAllActive : ViewAllInactive;
        }
    }

    public void SetObjectsSelectionState(bool selected)
    {
        foreach (Transform child in scrollView.content)
        {
            var selectButton = child.Find("Select")?.GetComponent<Image>();
            var outline = child.GetComponentInChildren<UnityEngine.UI.Outline>();

            if (selectButton != null)
            {
                selectButton.sprite = selected ? SelectActive : SelectInactive;
            }

            if (outline != null)
            {
                outline.enabled = selected;
            }
        }
    }

    public void SetObjectsVisibilityState(bool visible)
    {
        foreach (Transform child in scrollView.content)
        {
            var controller = child.GetComponent<PrefabController>();
            var viewButton = child.Find("Visibility")?.GetComponent<Image>();

            if (controller != null)
            {
                MeshRenderer renderer = controller.targetObject.GetComponent<MeshRenderer>();
                renderer.enabled = visible;
            }

            if (viewButton != null)
            {
                viewButton.sprite = visible ? ViewSingleActive : ViewSingleInactive;
            }
        }
    }

    public void TransparencySectionHide()
    {
        foreach (Transform child in transparentSection)
        {
            if (child.GetComponent<UnityEngine.UI.Outline>() != null)
            {
                child.GetComponent<UnityEngine.UI.Outline>().enabled = false;
            }
        }
    }
}