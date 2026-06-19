using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private Animator _animator;

    [SerializeField] private CinemachineVirtualCamera _normalCamera;
    [SerializeField] private CinemachineVirtualCamera _aimCamera;

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

    [SerializeField] private float _recoilKickDuration = 0.05f;
    [SerializeField] private float _recoilReturnDuration = 0.25f;
    [SerializeField] private Ease _kickEase = Ease.OutQuad;
    [SerializeField] private Ease _returnEase = Ease.InOutQuad;

    private float _cinemachineTargetPitch;
    private Vector2 _lookInput;

    private Vector3 _recoilOffset;
    private Sequence _recoilSequence;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _normalCamera.Priority = 10;
        _aimCamera.Priority = 5;
    }

    private void Update()
    {
        if (_lookInput != Vector2.zero)
        {
            transform.Rotate(Vector3.up * _lookInput.x * _sensitivityX * Time.deltaTime);

            _cinemachineTargetPitch -= _lookInput.y * _sensitivityY * Time.deltaTime;
            _lookInput = Vector2.zero;
        }

        _cinemachineTargetPitch = Mathf.Clamp(_cinemachineTargetPitch, _minPitch, _maxPitch);

        float finalPitch = _cinemachineTargetPitch + _recoilOffset.x;
        float finalYaw = _recoilOffset.y;
        _cameraTarget.localRotation = Quaternion.Euler(finalPitch, finalYaw, 0f);
    }

    private void OnDisable()
    {
        _recoilSequence?.Kill();
    }

    public void SetLookDelta(Vector2 lookDelta)
    {
        _lookInput = lookDelta;
    }

    public void StartAiming()
    {
        _aimCamera.Priority = _normalCamera.Priority + 1;
    }

    public void StopAiming()
    {
        _aimCamera.Priority = _normalCamera.Priority - 1;
    }

    public void ApplyRecoil(float pitchKick, float yawKick)
    {
        _recoilSequence?.Kill();

        Vector3 targetPeak = new Vector3(_recoilOffset.x - pitchKick, _recoilOffset.y + yawKick, 0f);

        _recoilSequence = DOTween.Sequence();

        _recoilSequence.Append(DOTween.To(() => _recoilOffset, x => _recoilOffset = x, targetPeak, _recoilKickDuration)
            .SetEase(_kickEase));

        _recoilSequence.Append(DOTween.To(() => _recoilOffset, x => _recoilOffset = x, Vector3.zero, _recoilReturnDuration)
            .SetEase(_returnEase));
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
}