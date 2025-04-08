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
            if (_playerValues.TryGetValue(key, out var value))
            {
                return value;
            }
            return "";
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
            if (_events.TryGetValue(eventName, out var evnt))
            {
                return evnt;
            }

            return "";
        }

        public List<string> GetNotifications()
        {
            return _notifications;
        }
    }
}
