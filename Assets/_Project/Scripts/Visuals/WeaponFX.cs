using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponFX : MonoBehaviour
{
    [SerializeField] private HitscanShooter _shooter;

    [SerializeField] private ParticleSystemPool _bloodImpactPool;
    [SerializeField] private ParticleSystemPool _sparkImpactPool;
    [SerializeField] private ParticleSystemPool _muzzleFlashPool;
    [SerializeField] private LineRendererPool _tracerTrailPool;

    [SerializeField] private DamagePopupPool _damagePopupPool;

    [SerializeField] private float _tracerDuration = 0.04f;
    [SerializeField] private float _damageScatterRadius = 0.2f;

    private void OnEnable()
    {
        _shooter.ShotHit += OnShotHit;
        _shooter.ShotMissed += OnShotMissed;
    }

    private void OnDisable()
    {
        _shooter.ShotHit -= OnShotHit;
        _shooter.ShotMissed -= OnShotMissed;
    }

    private void OnShotHit(Vector3 start, Vector3 hitPoint, Vector3 normal, bool isCharacter, int damage)
    {
        var pool = isCharacter ? _bloodImpactPool : _sparkImpactPool;

        SpawnImpactEffect(pool, hitPoint, normal);
        SpawnMuzzleFlash(start);
        SpawnBulletTracer(start, hitPoint);

        if (isCharacter && damage > 0)
        {
            SpawnDamagePopup(hitPoint, damage);
        }
    }

    private void OnShotMissed(Vector3 start, Vector3 endPoint)
    {
        SpawnMuzzleFlash(start);
        SpawnBulletTracer(start, endPoint);
    }

    private void SpawnImpactEffect(ParticleSystemPool pool, Vector3 position, Vector3 normal)
    {
        PoolableParticle effect = pool.Get();
        effect.transform.position = position;
        effect.transform.rotation = Quaternion.LookRotation(normal);
        effect.Play();
    }

    private void SpawnMuzzleFlash(Vector3 start)
    {
        PoolableParticle flash = _muzzleFlashPool.Get();

        flash.transform.position = start;
        flash.transform.localRotation = Quaternion.identity;

        flash.Play();
    }

    private void SpawnBulletTracer(Vector3 start, Vector3 end)
    {
        LineRenderer tracer = _tracerTrailPool.Get();

        if (tracer != null)
        {
            StartCoroutine(TracerLifetimeRoutine(tracer, start, end));
        }
    }

    private void SpawnDamagePopup(Vector3 hitPoint, int damage)
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-_damageScatterRadius, _damageScatterRadius),
            Random.Range(-_damageScatterRadius * 0.5f, _damageScatterRadius * 0.5f),
            Random.Range(-_damageScatterRadius, _damageScatterRadius)
        );

        DamagePopup popup = _damagePopupPool.Get();
        popup.transform.SetParent(null);
        popup.transform.position = hitPoint + randomOffset;

        popup.Setup(damage);
    }

    private IEnumerator TracerLifetimeRoutine(LineRenderer tracer, Vector3 start, Vector3 end)
    {
        tracer.SetPosition(0, start);
        tracer.SetPosition(1, end);

        yield return new WaitForSeconds(_tracerDuration);

        _tracerTrailPool.Release(tracer);
    }
}