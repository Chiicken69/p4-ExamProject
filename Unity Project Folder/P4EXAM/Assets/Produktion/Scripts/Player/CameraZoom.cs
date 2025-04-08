using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private float _zoom;
    private float _zoomMultiplier = 4f;
    private float _minZoom = 2f;
    private float _maxZoom = 8f;
    private float _velocity = 0f;
    private float _smoothTime = 0.25f;

    [SerializeField] private Camera _camera;

    void Start()
    {
        _zoom = _camera.orthographicSize;

    }

    private void Update()
    {
        _zoom -= InputHandler.Instance.PassInputFloatValue() * _zoomMultiplier;
        _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _zoom, ref _velocity, _smoothTime);
    }
}
