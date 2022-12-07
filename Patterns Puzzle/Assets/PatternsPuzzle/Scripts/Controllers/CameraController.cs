using System.Collections;
using System.Collections.Generic;
using OknaaEXTENSIONS;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController Instance => instance ??= FindObjectOfType<CameraController>();
    private static CameraController instance;
    
    public void SetCameraTarget(Transform target) {
        var targetPosition = target.position;
        targetPosition.z = -10;
        transform.position = targetPosition;
    }
}