using UnityEngine;
using UnityEngine.UI;

public class ControlPanelController : MonoBehaviour
{
    public bool AllSelected { get; set; }
    public bool AllView { get; set; }

    private SelectionManager selectionManager;
    private Button selectAllButton;
    private Button viewAllButton;

    public void Initialize(SelectionManager manager)
    {
        selectionManager = manager;
        AllSelected = false;
        AllView = true;

        selectAllButton = transform.Find("AddDeleteAll")?.GetComponent<Button>();
        viewAllButton = transform.Find("ShowHideAll")?.GetComponent<Button>();

        if (selectAllButton != null)
        {
            selectAllButton.onClick.RemoveAllListeners();
            selectAllButton.onClick.AddListener(HandleSelectAll);
        }

        if (viewAllButton != null)
        {
            viewAllButton.onClick.RemoveAllListeners();
            viewAllButton.onClick.AddListener(HandleViewAll);
        }

        UpdateSelectAllButton();
        UpdateViewAllButton();
    }

    private void HandleSelectAll()
    {
        selectionManager.selectedObjects.Clear();
        AllSelected = !AllSelected;

        if (AllSelected)
        {
            var selectableObjects = GameObject.FindGameObjectsWithTag("Selectable");
            selectionManager.selectedObjects.AddRange(selectableObjects);
        }
        selectionManager.SetObjectsSelectionState(AllSelected);
        selectionManager.TransparencySectionHide();
        UpdateSelectAllButton();
    }

    private void HandleViewAll()
    {
        AllView = !AllView;
        selectionManager.SetObjectsVisibilityState(AllView);
        UpdateViewAllButton();
    }

    private void UpdateSelectAllButton()
    {
        if (selectAllButton != null)
        {
            selectAllButton.image.sprite = AllSelected
                ? selectionManager.SelectActive
                : selectionManager.SelectInactive;
        }
    }

    private void UpdateViewAllButton()
    {
        if (viewAllButton != null)
        {
            /*viewAllButton.image.sprite = AllView
                ? selectionManager.ViewAllActive
                : selectionManager.ViewAllInactive;*/
            if(AllView)
            {
                viewAllButton.image.sprite = selectionManager.ViewAllActive;
            }else
            {
                viewAllButton.image.sprite = selectionManager.ViewAllInactive;
            }
        }
    }
}