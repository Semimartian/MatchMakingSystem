using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostUI : MonoBehaviour
{
    [SerializeField] private GameObject UIElements;
    [SerializeField] private TMPro.TextMeshProUGUI matchAccessibilityButtonText;
    public static HostUI instance;

    private void Start()
    {
        instance = this;
        ShowUI(false);
    }

    public void SwitchMatchAccessibility()
    {
        Player.localPlayer.SwitchMatchAccessibility();
    }

    public void UpdateMatchAccessibilityText(Match.StateFlags state)
    {
        matchAccessibilityButtonText.text =
            ((state & Match.StateFlags.Public) != 0) ? "Public" : "Private";
    }

    public void ShowUI(bool value)
    {
        UIElements.SetActive(value);
    }
}
