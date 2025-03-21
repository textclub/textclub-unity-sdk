using UnityEngine;

namespace Textclub
{
    public class DebugBridgeMock : IBridgeMock
    {
        public string playerId { get; }

        public string isRegistered { get; }

        public DebugBridgeMock(string playerId, bool isRegistered)
        {
            this.playerId = playerId;
            this.isRegistered = isRegistered.ToString();
        }

        public void CaptureEvent(string eventName, string properties)
        {
            Debug.Log($"[Textclub] CaptureEvent {eventName} {properties}");
        }

        public string GetPlayerValue(string key)
        {
            Debug.Log($"[Textclub] GetPlayerValue {key}");
            return key;
        }

        public void ScheduleNotification(string options)
        {
            Debug.Log($"[Textclub] ScheduleNotification {options}");
        }

        public void SetPlayerValue(string key, string value)
        {
            Debug.Log($"[Textclub] SetPlayerValue {key} {value}");
        }
    }
}
