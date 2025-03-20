using System;
using UnityEngine;

namespace Textclub
{
    public class Notifications
    {
        internal Notifications() { }

        public void ScheduleNotification(Options options)
        {
            JsBridge.ScheduleNotification(JsonUtility.ToJson(options));
        }

        [System.Serializable]
        public class Options : ISerializationCallbackReceiver
        {
            [SerializeField] private string scheduledAt;
            public string message;
            public bool attemptPushNotification;
            public string deduplicationKey;

            [System.NonSerialized] public System.DateTime date;

            public void OnAfterDeserialize()
            {
                date = DateTimeExtensions.FromJsString(scheduledAt);
            }

            public void OnBeforeSerialize()
            {
                scheduledAt = date.ToJsString();
            }
        }
    }
}
