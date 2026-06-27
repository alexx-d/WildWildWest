using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ScreenTransitionUI : MonoBehaviour
{
    [SerializeField] private Image _screenFader;
    [SerializeField] private TextMeshProUGUI _timerText;

    private void Start()
    {
        _screenFader.color = new Color(0, 0, 0, 0);
        _timerText.gameObject.SetActive(false);
    }

    public void UpdateTimerText(int secondsRemaining)
    {
        if (!_timerText.gameObject.activeSelf)
        {
            _timerText.gameObject.SetActive(true);
        }
        _timerText.text = $"Следующая волна через: {secondsRemaining}";
    }

    public void HideTimer()
    {
        _timerText.gameObject.SetActive(false);
    }

    public Tween FadeToBlack(float duration)
    {
        _screenFader.DOKill();
        return _screenFader.DOFade(1f, duration).SetUpdate(true);
    }

    public Tween FadeFromBlack(float duration)
    {
        _screenFader.DOKill();
        return _screenFader.DOFade(0f, duration).SetUpdate(true);
    }

    public void ResetUI()
    {
        _screenFader.DOKill();
        _screenFader.color = new Color(0, 0, 0, 0);

        HideTimer();
    }
}