using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamagePopup : MonoBehaviour, IPoolable<DamagePopup>
{
    [SerializeField] private TMP_Text _text;

    [SerializeField] private float _duration = 0.6f;
    [SerializeField] private float _moveUpDistance = 1.5f;
    [SerializeField] private float _punchScale = 1.3f;

    private Camera _mainCamera;

    public event System.Action<DamagePopup> Died;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void Setup(int damageAmount)
    {
        _text.alpha = 1f;
        transform.localScale = Vector3.zero;

        _text.text = damageAmount.ToString();

        if (_mainCamera != null)
        {
            transform.forward = _mainCamera.transform.forward;
        }

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(Vector3.one * _punchScale, _duration * 0.2f).SetEase(Ease.OutBack));

        sequence.Join(transform.DOMoveY(transform.position.y + _moveUpDistance, _duration).SetEase(Ease.OutCubic));

        sequence.Insert(_duration * 0.4f, _text.DOFade(0f, _duration * 0.6f));

        sequence.OnComplete(() => Died?.Invoke(this));
    }
}