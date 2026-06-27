public static class AudioUtils
{
    public const float MinDb = -80f;
    public const float MaxDb = 0f;
    public const float MinSliderValue = 0.0001f;
    public const float LogMultiplier = 20f;

    public static float LinearToDecibels(float linear)
    {
        return UnityEngine.Mathf.Log10(UnityEngine.Mathf.Clamp(linear, MinSliderValue, 1f)) * LogMultiplier;
    }
}