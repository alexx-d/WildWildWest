using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Animator _animator;

    [Header("Sensitivity")]
    [SerializeField] private float _sensitivityX = 15f;
    [SerializeField] private float _sensitivityY = 15f;

    [Header("Limits")]
    [SerializeField] private float _minPitch = -40f;
    [SerializeField] private float _maxPitch = 60f;

    [Header("Spine Bending")]
    [Range(0f, 1f)]
    [Tooltip("Насколько сильно торс гнется за камерой (0.5 = наполовину)")]
    [SerializeField] private float _spineBendingWeight = 0.5f;

    private float _cinemachineTargetPitch;
    private Vector2 _lookInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (_lookInput != Vector2.zero)
        {
            transform.Rotate(Vector3.up * _lookInput.x * _sensitivityX * Time.deltaTime);

            _cinemachineTargetPitch -= _lookInput.y * _sensitivityY * Time.deltaTime;
            _cinemachineTargetPitch = Mathf.Clamp(_cinemachineTargetPitch, _minPitch, _maxPitch);

            _cameraTarget.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0f, 0f);

            _lookInput = Vector2.zero;
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (_animator == null || _cameraTarget == null)
        {
            return; 
        }

        _animator.SetLookAtWeight(1f, _spineBendingWeight, 1f, 0f, 0.5f);

        Vector3 lookPoint = _cameraTarget.position + _cameraTarget.forward * 10f;

        _animator.SetLookAtPosition(lookPoint);
    }

    public void SetLookDelta(Vector2 lookDelta)
    {
        _lookInput = lookDelta;
    }
}