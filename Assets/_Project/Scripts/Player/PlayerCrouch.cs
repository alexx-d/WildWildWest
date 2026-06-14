using UnityEngine;
using DG.Tweening;

public class PlayerCrouch : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Transform _cameraTarget;

    [SerializeField] private float _crouchHeight = 1.5f;
    [SerializeField] private float _crouchDuration = 0.25f;

    [SerializeField] private float _crouchZOffset = 0.5f;

    private float _normalHeight;
    private float _lowestPointY;
    private Vector3 _initialCameraLocalPosition;

    private Tween _crouchTween;

    private void Awake()
    {
        _normalHeight = _characterController.height;
        _lowestPointY = _characterController.center.y - (_normalHeight * 0.5f);
        _initialCameraLocalPosition = _cameraTarget.localPosition;
    }

    public void SetCrouchState(bool isCrouching)
    {
        float targetHeight = isCrouching ? _crouchHeight : _normalHeight;

        _crouchTween?.Kill();

        _crouchTween = DOVirtual.Float(_characterController.height, targetHeight, _crouchDuration, value =>
        {
            _characterController.height = value;

            Vector3 newCenter = _characterController.center;
            newCenter.y = _lowestPointY + (value * 0.5f);
            _characterController.center = newCenter;

            if (_cameraTarget != null)
            {
                float crouchProgress = Mathf.InverseLerp(_normalHeight, _crouchHeight, value);
                float heightDifference = _normalHeight - value;

                Vector3 newCameraPos = _initialCameraLocalPosition;

                newCameraPos.y -= heightDifference;

                newCameraPos.z += Mathf.Lerp(0f, _crouchZOffset, crouchProgress);

                _cameraTarget.localPosition = newCameraPos;
            }
        })
        .SetEase(Ease.OutQuad);
    }

    private void OnDestroy()
    {
        _crouchTween?.Kill();
    }
}