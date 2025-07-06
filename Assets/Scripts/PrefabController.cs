using UnityEngine;
using UnityEngine.UI;

public class PrefabController : MonoBehaviour
{
    public GameObject targetObject { get; private set; }

    private SelectionManager selectionManager;
    private Button selectButton;
    private Button viewButton;
    private UnityEngine.UI.Outline outline;

    public void Initialize(GameObject obj, SelectionManager manager)
    {
        targetObject = obj;
        selectionManager = manager;

        selectButton = transform.Find("Select")?.GetComponent<Button>();
        viewButton = transform.Find("Visibility")?.GetComponent<Button>();
        outline = GetComponentInChildren<UnityEngine.UI.Outline>();

        if (selectButton != null)
        {
            selectButton.onClick.AddListener(HandleSelectClick);
        }

        if (viewButton != null)
        {
            viewButton.onClick.AddListener(HandleViewClick);
        }

        MeshRenderer meshRenderer = targetObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null && !meshRenderer.enabled)
        {
            viewButton.image.sprite = selectionManager.ViewSingleInactive;
        }
    }

    private void HandleSelectClick()
    {
        selectionManager.ToggleObjectSelection(targetObject);
        bool isSelected = selectionManager.selectedObjects.Contains(targetObject);

        if (selectButton != null)
        {
            selectButton.image.sprite = isSelected
                ? selectionManager.SelectActive
                : selectionManager.SelectInactive;
        }

        if (outline != null)
        {
            outline.enabled = isSelected;
        }

        selectionManager.UpdateAllSelectionState();
        selectionManager.TransparencySectionHide();
    }

    private void HandleViewClick()
    {
        MeshRenderer renderer = targetObject.GetComponent<MeshRenderer>();
        renderer.enabled = !renderer.enabled;

        if (viewButton != null)
        {
            viewButton.image.sprite = renderer.enabled
                ? selectionManager.ViewSingleActive
                : selectionManager.ViewSingleInactive;
        }

        selectionManager.UpdateAllViewState();
    }
}