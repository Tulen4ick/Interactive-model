using UnityEngine;

[System.Serializable]
public class CameraData 
{
    public Vector3 cameraPosition;
    public Vector3 focusPoint;
    public float currentVerticalAngle;
    public float currentHorizontalAngle;
    public string targetObjectPath;
    public float currentDistance;
}
