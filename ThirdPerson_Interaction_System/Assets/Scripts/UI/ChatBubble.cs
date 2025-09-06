using UnityEngine;
using TMPro;

/// <summary>
/// Displays a chat bubble with a predefined message when interacting with an NPC.
/// </summary>
public class ChatBubble : MonoBehaviour
{
    [SerializeField] private GameObject _bubblePanel;
    [SerializeField] private TextMeshProUGUI _bubbleText;
    [TextArea]
    [SerializeField] private string _message = "Try interact with chest and Door.";

    private void Start()
    {
        HideMessage();
    }

    /// <summary>
    /// Displays the chat bubble with the predefined message.
    /// </summary>
    public void ShowMessage()
    {
        if (_bubblePanel != null && _bubbleText != null)
        {
            _bubblePanel.SetActive(true);
            _bubbleText.text = _message;
        }
    }

    /// <summary>
    /// Hides the chat bubble.
    /// </summary>
    public void HideMessage()
    {
        if (_bubblePanel != null)
            _bubblePanel.SetActive(false);
    }
}