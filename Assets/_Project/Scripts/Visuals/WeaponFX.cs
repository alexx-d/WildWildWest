using System.Collections;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [SerializeField] private HitscanShooter _shooter;

    [SerializeField] private ParticleSystemPool _bloodImpactPool;
    [SerializeField] private ParticleSystemPool _sparkImpactPool;
    [SerializeField] private LineRendererPool _tracerTrailPool;

    [SerializeField] private float _tracerDuration = 0.04f;

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

    private void OnShotHit(Vector3 start, Vector3 hitPoint, Vector3 normal, bool isCharacter)
    {
        var pool = isCharacter ? _bloodImpactPool : _sparkImpactPool;

        SpawnImpactEffect(pool, hitPoint, normal);
        SpawnBulletTracer(start, hitPoint);
    }

    private void OnShotMissed(Vector3 start, Vector3 endPoint)
    {
        SpawnBulletTracer(start, endPoint);
    }

    private void SpawnImpactEffect(ParticleSystemPool pool, Vector3 position, Vector3 normal)
    {
        if (pool == null)
        {
            return;
        }

        PoolableParticle effect = pool.Get();
        effect.gameObject.SetActive(true);
        effect.transform.position = position;
        effect.transform.rotation = Quaternion.LookRotation(normal);
        effect.Play();
    }

    private void SpawnBulletTracer(Vector3 start, Vector3 end)
    {
        if (_tracerTrailPool == null)
        {
            return;
        }

        LineRenderer tracer = _tracerTrailPool.Get();

        if (tracer != null)
        {
            StartCoroutine(TracerLifetimeRoutine(tracer, start, end));
        }
    }

    private IEnumerator TracerLifetimeRoutine(LineRenderer tracer, Vector3 start, Vector3 end)
    {
        tracer.gameObject.SetActive(true);
        tracer.SetPosition(0, start);
        tracer.SetPosition(1, end);

        yield return new WaitForSeconds(_tracerDuration);

        _tracerTrailPool.Release(tracer);
    }
}