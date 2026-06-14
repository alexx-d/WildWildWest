using System;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _checkRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    private bool _isGrounded;
    public bool IsGrounded => _isGrounded;

    public event Action<bool> GroundedChanged;

    private void FixedUpdate()
    {
        bool wasGrounded = _isGrounded;
        _isGrounded = Physics.CheckSphere(_groundCheckPoint.position, _checkRadius, _groundLayer);

        if (_isGrounded != wasGrounded)
        {
            GroundedChanged?.Invoke(_isGrounded);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundCheckPoint == null) return;
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(_groundCheckPoint.position, _checkRadius);
    }
}