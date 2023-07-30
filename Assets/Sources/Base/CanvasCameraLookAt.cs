using UnityEngine;

public sealed class CanvasCameraLookAt : MonoBehaviour
{
    [SerializeField] private Transform _camera;

    private void Start()
    {
        if (_camera == null)
            _camera = Camera.main.gameObject.transform;
    }

    private void LateUpdate()
    {
        transform.rotation = _camera.rotation;
    }
}