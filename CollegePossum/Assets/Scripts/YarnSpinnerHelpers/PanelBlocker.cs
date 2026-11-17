using UnityEngine;

public class PanelBlocker : MonoBehaviour
{
    public static int OpenCount { get; private set; }
    public static bool AnyOpen => OpenCount > 0;

    private void OnEnable() { OpenCount++; }
    private void OnDisable() { OpenCount = Mathf.Max(0, OpenCount - 1); }
}
