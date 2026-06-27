using Cinemachine;
using UnityEngine;

public class MenuCameraRotator : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _menuVirtualCamera;
    [SerializeField] private float _rotationSpeed = 10f;

    private CinemachineOrbitalTransposer _orbitalTransposer;
    private bool _isRotating = false;

    private void Awake()
    {
        if (_menuVirtualCamera != null)
        {
            _orbitalTransposer = _menuVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }
    }

    private void Update()
    {
        if (!_isRotating || _orbitalTransposer == null)
        {
            return;
        }

        _orbitalTransposer.m_XAxis.Value += _rotationSpeed * Time.unscaledDeltaTime;
    }

    public void StartRotation()
    {
        _isRotating = true;
    }

    public void StopRotation()
    {
        _isRotating = false;
    }
}