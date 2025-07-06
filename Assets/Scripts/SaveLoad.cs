using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class SaveLoad : MonoBehaviour
{
    public Transform panel;
    public void SaveScene(string fileName)
    {
        SceneData data = new SceneData();

        Camera mainCamera = Camera.main;
        CameraController controller = mainCamera.GetComponent<CameraController>();
        data.cameraData = new CameraData
        {
            cameraPosition = mainCamera.transform.position,
            focusPoint = controller.focusPoint,
            currentVerticalAngle = controller.currentVerticalAngle,
            currentHorizontalAngle = controller.currentHorizontalAngle,
            targetObjectPath = controller.target == null ? null : GetObjectPath(controller.target),
            currentDistance = controller.currentDistance
        };
        foreach (var obj in GameObject.FindGameObjectsWithTag("Selectable"))
        {
            MaterialInstancer materialInstancer = obj.GetComponent<MaterialInstancer>();
            data.objects.Add(new ObjectData
            {
                objectPath = GetObjectPath(obj.transform),
                selectedMaterialIndex = materialInstancer.GetCurrentMaterialIndex(),
                alpha = materialInstancer.GetAlpha(),
                rendererIsEnabled = obj.GetComponent<MeshRenderer>().enabled
            });
        }

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, fileName), json);
    }

    public void LoadScene(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        SceneData data = JsonUtility.FromJson<SceneData>(json);

        Camera mainCamera = Camera.main;
        CameraController controller = mainCamera.GetComponent<CameraController>();
        mainCamera.transform.position = data.cameraData.cameraPosition;
        controller.focusPoint = data.cameraData.focusPoint;
        controller.currentDistance = data.cameraData.currentDistance;
        controller.currentHorizontalAngle = data.cameraData.currentHorizontalAngle;
        controller.currentVerticalAngle = data.cameraData.currentVerticalAngle;
        controller.UpdateCameraPosition();
        if (controller.target != null)
        {
            controller.target.gameObject.GetComponent<Outline>().enabled = false;
        }
        if (!string.IsNullOrEmpty(data.cameraData.targetObjectPath))
        {
            controller.target = GameObject.Find(data.cameraData.targetObjectPath).transform;
            controller.AddOutline(controller.target);
        }
        else
        {
            controller.target = null;
        }
        foreach (ObjectData objData in data.objects)
        {
            Transform objTransform = GameObject.Find(objData.objectPath)?.transform;
            if (objTransform == null) continue;
            MaterialInstancer materialInstancer = objTransform.GetComponent<MaterialInstancer>();
            materialInstancer.ApplyMaterial(objData.selectedMaterialIndex);
            materialInstancer.SetAlpha(objData.alpha);
            MeshRenderer renderer = objTransform.GetComponent<MeshRenderer>();
            renderer.enabled = objData.rendererIsEnabled;
        }

        SelectionManager manager = panel.GetComponent<SelectionManager>();
        manager.Initialized();
    }

    private static string GetObjectPath(Transform tr)
    {
        if (tr.parent == null)
        {
            return tr.name;
        }
        return GetObjectPath(tr.parent) + "/" + tr.name;
    }
}
