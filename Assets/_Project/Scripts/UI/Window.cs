using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class Window : MonoBehaviour
{
    [SerializeField] private CanvasGroup _windowGroup;

    protected CanvasGroup WindowGroup => _windowGroup;

    public bool IsOpen => _windowGroup != null && _windowGroup.blocksRaycasts;

    protected virtual void Awake()
    {
        if (_windowGroup == null)
        {
            _windowGroup = GetComponent<CanvasGroup>();
        }
    }

    public virtual void Open()
    {
        _windowGroup.alpha = 1f;
        _windowGroup.interactable = true;
        _windowGroup.blocksRaycasts = true;
    }

    public virtual void Close()
    {
        _windowGroup.alpha = 0f;
        _windowGroup.interactable = false;
        _windowGroup.blocksRaycasts = false;
    }
}