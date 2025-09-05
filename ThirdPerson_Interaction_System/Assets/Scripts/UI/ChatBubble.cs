using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] private GameObject _bubblePanel;
    [SerializeField] private TextMeshProUGUI _bubbleText;
    [TextArea]
    [SerializeField] private string _message = "Try interact with chest and Door, if already done then press \"Reset Room\" Button at the top right corner of the screen.";

    private void Start()
    {
        ShowMessage();
    }

    public void ShowMessage()
    {
        if (_bubblePanel != null && _bubbleText != null)
        {
            _bubblePanel.SetActive(true);
            _bubbleText.text = _message;
        }
    }

    public void HideMessage()
    {
        if (_bubblePanel != null)
            _bubblePanel.SetActive(false);
    }
}