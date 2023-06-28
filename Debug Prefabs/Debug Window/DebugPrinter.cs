using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IXRE.Scripts.Diverse
{
    /// <summary>
    ///     Utility script for writing logs onto a Canvas for debugging in a VR app.
    /// </summary>
    public class DebugPrinter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI debugText;

        private ScrollRect scrollRect;

        private void Awake()
        {
            // Cache references
            scrollRect = GetComponentInChildren<ScrollRect>();

            // Subscribe to log message events
            // Application.logMessageReceived += HandleLog;
            Application.logMessageReceivedThreaded += HandleLog;

            // Set the starting text
            debugText.text +=
                "Debug messages will appear here.\n";
        }

        private void OnDestroy()
        {
            // Application.logMessageReceived -= HandleLog;
            Application.logMessageReceivedThreaded -= HandleLog;
        }

        private void HandleLog(string message, string stackTrace, LogType type)
        {
            if (debugText.text.Length >= 5000)
                debugText.text = message + "\n( " +
                                 stackTrace.Substring(0, stackTrace.Length > 250 ? 250 : stackTrace.Length) + " )";
            else
                debugText.text += message + "\n" +
                                  stackTrace.Substring(0, stackTrace.Length > 250 ? 250 : stackTrace.Length) + " ";

            debugText.text += "\n";
            Canvas.ForceUpdateCanvases();
            // scrollRect.verticalScrollbar.value = 0;
            scrollRect.verticalNormalizedPosition = 0;
        }
    }
}