using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _groundedStickyForce = -2f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _crouchSpeed = 2.5f;

    private CharacterController _characterController;
    private Transform _mainCameraTransform;

    private float _verticalVelocity;
    private Vector2 _moveInput;
    private bool _isCrouching;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        if (Camera.main != null)
        {
            _mainCameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        ApplyGravity();
        Move();
    }

    public void SetDirection(Vector2 direction)
    {
        _moveInput = direction;
    }

    public void SetCrouchState(bool isCrouching)
    {
        _isCrouching = isCrouching;
    }

    public void Jump()
    {
        _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = _groundedStickyForce;
        }
        else
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }
    }

    private void Move()
    {
        Vector3 movementDirection = Vector3.zero;

        if (_moveInput != Vector2.zero)
        {
            Vector2 input = _moveInput;

            Vector3 forward = _mainCameraTransform.forward;
            Vector3 right = _mainCameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            movementDirection = (forward * input.y + right * input.x).normalized;
        }

        float currentSpeed = _isCrouching ? _crouchSpeed : _moveSpeed;

        Vector3 motion = movementDirection * currentSpeed;
        motion.y = _verticalVelocity;

        _characterController.Move(motion * Time.deltaTime);
    }
}