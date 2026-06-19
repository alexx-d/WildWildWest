using UnityEngine;

public class CrosshairUI : MonoBehaviour
{
    [SerializeField] private RectTransform _leftDash;
    [SerializeField] private RectTransform _rightDash;
    [SerializeField] private RectTransform _topDash;
    [SerializeField] private RectTransform _bottomDash;

    [SerializeField] private float _minSize = 10f;
    [SerializeField] private float _spreadMultiplier = 200f;
    [SerializeField] private float _smoothTime = 15f;

    private float _currentTargetSize;
    private float _currentSize;

    private void Awake()
    {
        _currentSize = _minSize;
    }

    private void Update()
    {
        if (Mathf.Approximately(_currentSize, _currentTargetSize))
        {
            return;
        }
            
        _currentSize = Mathf.Lerp(_currentSize, _currentTargetSize, Time.deltaTime * _smoothTime);

        ApplySize(_currentSize);
    }

    public void UpdateSpread(float currentSpread)
    {
        _currentTargetSize = _minSize + (currentSpread * _spreadMultiplier);
    }

    private void ApplySize(float size)
    {
        _leftDash.anchoredPosition = new Vector2(-size, 0f);
        _rightDash.anchoredPosition = new Vector2(size, 0f);
        _topDash.anchoredPosition = new Vector2(0f, size);
        _bottomDash.anchoredPosition = new Vector2(0f, -size);
    }
}