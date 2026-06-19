using System;
using UnityEngine;

public class HitscanShooter : MonoBehaviour
{
    [SerializeField] private Transform _muzzleFlashPoint;
    [SerializeField] private LayerMask _shootableLayers;

    public event Action<Vector3, Vector3, Vector3, bool, int> ShotHit;
    public event Action<Vector3, Vector3> ShotMissed;

    private Camera _mainCamera;

    public void Fire(float damage, float range, float spread, Vector3 recoilForce)
    {
        Vector3 rayOrigin = _mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
        Vector3 targetDirection = _mainCamera.transform.forward;

        if (spread > 0f)
        {
            Vector2 randomSpread = UnityEngine.Random.insideUnitCircle * spread;
            targetDirection += _mainCamera.transform.right * randomSpread.x + _mainCamera.transform.up * randomSpread.y;
            targetDirection.Normalize();
        }

        Vector3 muzzlePosition = _muzzleFlashPoint.position;

        if (Physics.Raycast(rayOrigin, targetDirection, out RaycastHit hit, range, _shootableLayers))
        {
            bool isCharacter = false;
            int damageDealt = 0;

            if (hit.collider.TryGetComponent<Hitbox>(out Hitbox hitbox))
            {
                isCharacter = true;

                if (hitbox.ParentDamageable != null)
                {
                    damageDealt = Mathf.RoundToInt(hitbox.ParentDamageable.TakeDamage(damage, hitbox.Type));
                }
            }

            ShotHit?.Invoke(muzzlePosition, hit.point, hit.normal, isCharacter, damageDealt);
        }
        else
        {
            Vector3 endPoint = rayOrigin + targetDirection * range;
            ShotMissed?.Invoke(muzzlePosition, endPoint);
        }
    }

    public void Initialize(Camera mainCamera)
    {
        _mainCamera = mainCamera;
    }
}