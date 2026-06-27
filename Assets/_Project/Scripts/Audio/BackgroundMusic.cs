using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip _musicTrack;
    [SerializeField][Range(0f, 1f)] private float _volume = 0.4f;

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private float _transitionDuration = 0.4f;

    [SerializeField] private string _pitchParameter = "MusicPitch";
    [SerializeField] private string _lowpassParameter = "MusicLowpass";

    [SerializeField] private float _defaultPitch = 1f;
    [SerializeField] private float _defaultLowpass = 22000f;

    [SerializeField] private float _pausePitch = 0.65f;
    [SerializeField] private float _pauseLowpass = 800f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        _audioSource.clip = _musicTrack;
        _audioSource.loop = true;
        _audioSource.volume = _volume;
        _audioSource.spatialBlend = 0f;
        _audioSource.playOnAwake = true;

        ResetMixerEffects();
    }

    private void Start()
    {
        _audioSource.Play();
    }

    public void ApplyPauseEffects()
    {
        DOTween.To(() => GetMixerFloat(_pitchParameter),
                   x => _mixer.SetFloat(_pitchParameter, x),
                   _pausePitch, _transitionDuration).SetUpdate(true);

        DOTween.To(() => GetMixerFloat(_lowpassParameter),
                   x => _mixer.SetFloat(_lowpassParameter, x),
                   _pauseLowpass, _transitionDuration).SetUpdate(true);
    }

    public void RemovePauseEffects()
    {
        DOTween.To(() => GetMixerFloat(_pitchParameter),
                   x => _mixer.SetFloat(_pitchParameter, x),
                   _defaultPitch, _transitionDuration).SetUpdate(true);

        DOTween.To(() => GetMixerFloat(_lowpassParameter),
                   x => _mixer.SetFloat(_lowpassParameter, x),
                   _defaultLowpass, _transitionDuration).SetUpdate(true);
    }

    private void ResetMixerEffects()
    {
        _mixer.SetFloat(_pitchParameter, _defaultPitch);
        _mixer.SetFloat(_lowpassParameter, _defaultLowpass);
    }

    private float GetMixerFloat(string parameterName)
    {
        _mixer.GetFloat(parameterName, out float value);
        return value;
    }
}