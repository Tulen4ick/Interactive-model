using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public CameraData cameraData;
    public List<ObjectData> objects = new List<ObjectData>();
}
