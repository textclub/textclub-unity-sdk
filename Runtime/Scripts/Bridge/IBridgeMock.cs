using System.Collections.Generic;

namespace Textclub
{
    public interface IBridgeMock
    {
        string playerId { get; }

        string isRegistered { get; }

        string GetPlayerValue(string key);

        void SetPlayerValue(string key, string value);

        void ScheduleNotification(string options);

        void CaptureEvent(string eventName, string properties);

        string GetEvent(string eventName);

        List<string> GetNotifications();
    }
}
