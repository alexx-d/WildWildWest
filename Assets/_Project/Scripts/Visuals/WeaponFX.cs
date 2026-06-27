using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponFX : MonoBehaviour
{
    [SerializeField] private HitscanShooter _shooter;

    [SerializeField] private PoolableParticle _bloodImpactPrefab;
    [SerializeField] private PoolableParticle _sparkImpactPrefab;
    [SerializeField] private PoolableParticle _muzzleFlashPrefab;
    [SerializeField] private LineRenderer _tracerTrailPrefab;
    [SerializeField] private DamagePopup _damagePopupPrefab;
    [SerializeField] private AudioSource _3dSoundPrefab;

    [SerializeField] private float _tracerSpeed = 150f;
    [SerializeField] private float _tailLengthFactor = 0.15f;
    [SerializeField] private float _damageScatterRadius = 0.2f;

    [SerializeField] private AudioSource _shootAudioSource;
    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _environmentHitClip;
    [SerializeField] private AudioClip _fleshHitClip;
    [SerializeField][Range(0f, 0.2f)] private float _pitchRandomness = 0.05f;

    private Transform _fxContainer;
    private ComponentPool<PoolableParticle> _bloodImpactPool;
    private ComponentPool<PoolableParticle> _sparkImpactPool;
    private ComponentPool<PoolableParticle> _muzzleFlashPool;
    private ComponentPool<LineRenderer> _tracerTrailPool;
    private ComponentPool<DamagePopup> _damagePopupPool;
    private ComponentPool<AudioSource> _3dSoundPool;

    private float _baseShootPitch;

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

    public void Initialize(Transform worldFXContainer)
    {
        _fxContainer = worldFXContainer;

        _bloodImpactPool = new ComponentPool<PoolableParticle>(_bloodImpactPrefab, _fxContainer);
        _sparkImpactPool = new ComponentPool<PoolableParticle>(_sparkImpactPrefab, _fxContainer);
        _muzzleFlashPool = new ComponentPool<PoolableParticle>(_muzzleFlashPrefab, _fxContainer);
        _tracerTrailPool = new ComponentPool<LineRenderer>(_tracerTrailPrefab, _fxContainer);
        _damagePopupPool = new ComponentPool<DamagePopup>(_damagePopupPrefab, _fxContainer);
        _3dSoundPool = new ComponentPool<AudioSource>(_3dSoundPrefab, _fxContainer);

        _baseShootPitch = _shootAudioSource.pitch;
    }

    private void OnShotHit(Vector3 start, Vector3 hitPoint, Vector3 normal, bool isCharacter, int damage)
    {
        ComponentPool<PoolableParticle> pool = isCharacter ? _bloodImpactPool : _sparkImpactPool;
        AudioClip hitClip = isCharacter ? _fleshHitClip : _environmentHitClip;

        SpawnImpactEffect(pool, hitPoint, normal);
        SpawnMuzzleFlash(start);
        SpawnBulletTracer(start, hitPoint);

        PlayShootSound();
        Play3DHitSound(hitClip, hitPoint);

        if (isCharacter && damage > 0)
        {
            SpawnDamagePopup(hitPoint, damage);
        }
    }

    private void OnShotMissed(Vector3 start, Vector3 endPoint)
    {
        SpawnMuzzleFlash(start);
        SpawnBulletTracer(start, endPoint);
        PlayShootSound();
    }

    private void PlayShootSound()
    {
        _shootAudioSource.pitch = _baseShootPitch + Random.Range(-_pitchRandomness, _pitchRandomness);
        _shootAudioSource.PlayOneShot(_shootClip);
    }

    private void Play3DHitSound(AudioClip clip, Vector3 position)
    {
        AudioSource audioSource = _3dSoundPool.Get();

        audioSource.transform.position = position;
        audioSource.clip = clip;
        audioSource.pitch = 1f + Random.Range(-_pitchRandomness, _pitchRandomness);
        audioSource.Play();

        StartCoroutine(AudioLifetimeRoutine(audioSource, clip.length));
    }

    private IEnumerator AudioLifetimeRoutine(AudioSource audioSource, float duration)
    {
        yield return new WaitForSeconds(duration);

        audioSource.Stop();
        _3dSoundPool.Release(audioSource);
    }

    private void SpawnImpactEffect(ComponentPool<PoolableParticle> pool, Vector3 position, Vector3 normal)
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
        float distance = Vector3.Distance(start, end);
        float duration = distance / _tracerSpeed;
        float time = 0f;

        tracer.SetPosition(0, start);
        tracer.SetPosition(1, start);

        while (time < duration)
        {
            time += Time.deltaTime;
            float progress = time / duration;

            Vector3 currentEnd = Vector3.Lerp(start, end, progress);
            tracer.SetPosition(1, currentEnd);

            float startProgress = Mathf.Max(0f, progress - _tailLengthFactor);
            Vector3 currentStart = Vector3.Lerp(start, end, startProgress);
            tracer.SetPosition(0, currentStart);

            yield return null;
        }

        tracer.SetPosition(0, end);
        tracer.SetPosition(1, end);

        tracer.SetPosition(0, Vector3.zero);
        tracer.SetPosition(1, Vector3.zero);

        _tracerTrailPool.Release(tracer);
    }
}