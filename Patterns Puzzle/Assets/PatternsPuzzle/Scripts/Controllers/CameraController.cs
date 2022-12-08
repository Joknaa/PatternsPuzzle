using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController Instance => instance ??= FindObjectOfType<CameraController>();
    private static CameraController instance;

    private Camera _camera;
    private SpriteRenderer _target;
    private List<Vector3> _targetBounds;


    public void SetCameraTarget(Component target) {
        _target = target as SpriteRenderer;
        var targetPosition = _target.transform.position;
        targetPosition.z = -10;
        transform.position = targetPosition;

        AdjustCameraSize();
    }

    private void AdjustCameraSize() {
        if (_camera == null) _camera = GetComponent<Camera>();

        CalculateTargetBounds();
        
        var targetStillNotFullyVisible = !TargetIsFullyVisible();
        while (targetStillNotFullyVisible) {
            targetStillNotFullyVisible = !TargetIsFullyVisible();
            _camera.orthographicSize += 1f;
        }
        
    }

    private void CalculateTargetBounds() {
        Bounds bounds = _target.bounds;
        Vector3 size = bounds.size;
        Vector3 min = bounds.min;

        _targetBounds = new List<Vector3>(8) {
            min,
            min + new Vector3(0, 0, size.z),
            min + new Vector3(size.x, 0, size.z),
            min + new Vector3(size.x, 0, 0),
        };
        for (int i = 0; i < 4; i++) _targetBounds.Add(_targetBounds[i] + size.y * Vector3.up);
    }

    private bool TargetIsFullyVisible() {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

        for (int p = 0; p < planes.Length; p++) {
            foreach (var point in _targetBounds) {
                var pointIsNotVisible = !planes[p].GetSide(point);
                if (pointIsNotVisible) return false;
            }
        }

        return true;
    }

    /*
    public bool IsFullyVisible(Renderer renderer) {
        var camera = GetComponent<Camera>();
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        Bounds bounds = renderer.bounds;
        Vector3 size = bounds.size;
        Vector3 min = bounds.min;

        //Calculate the 8 points on the corners of the bounding box
        List<Vector3> boundsCorners = new List<Vector3>(8) {
            min,
            min + new Vector3(0, 0, size.z),
            min + new Vector3(size.x, 0, size.z),
            min + new Vector3(size.x, 0, 0),
        };
        for (int i = 0; i < 4; i++) boundsCorners.Add(boundsCorners[i] + size.y * Vector3.up);

        //Check each plane on every one of the 8 bounds' corners
        for (int p = 0; p < planes.Length; p++) {
            foreach (var point in boundsCorners) {
                var pointIsNotVisible = !planes[p].GetSide(point);
                if (pointIsNotVisible) return false;
            }
        }

        return true;
    }*/
}