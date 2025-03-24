using System.Collections.Generic;

namespace Textclub
{
    public class TestBridgeMock : IBridgeMock
    {
        public string playerId { get; }

        public string isRegistered { get; }

        private Dictionary<string, string> _events = new();
        private Dictionary<string, string> _playerValues = new();

        private List<string> _notifications = new();

        public TestBridgeMock(string playerId, bool isRegistered)
        {
            this.playerId = playerId;
            this.isRegistered = isRegistered.ToString();
        }

        public void CaptureEvent(string eventName, string properties)
        {
            _events.Add(eventName, properties);
        }

        public string GetPlayerValue(string key)
        {
            return _playerValues[key];
        }

        public void ScheduleNotification(string options)
        {
            _notifications.Add(options);
        }

        public void SetPlayerValue(string key, string value)
        {
            _playerValues[key] = value;
        }

        public string GetEvent(string eventName)
        {
            return _events[eventName];
        }

        public List<string> GetNotifications()
        {
            return _notifications;
        }
    }
}
