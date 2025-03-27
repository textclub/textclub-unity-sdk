using System.Collections.Generic;
using System.Linq;
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

        internal List<Options> GetNotifications()
        {
            return JsBridge.GetNotifications().
                            Select(n => JsonUtility.FromJson<Options>(n)).ToList();
        }

        [System.Serializable]
        public class Options : ISerializationCallbackReceiver
        {
            [SerializeField] private string scheduledAt;
            public string message;
            public bool attemptPushNotification;
            [SerializeField] private string metadata;
            public string deduplicationKey;

            public readonly Dictionary<string, object> data = new();

            [System.NonSerialized] public System.DateTime date;

            public void OnAfterDeserialize()
            {
                date = DateTimeExtensions.FromJsString(scheduledAt);
                var newData = Convert.FromString<Dictionary<string, object>>(metadata);
                data.Clear();
                foreach (var item in newData)
                {
                    data.Add(item.Key, item.Value);
                }
            }

            public void OnBeforeSerialize()
            {
                scheduledAt = date.ToJsString();
                metadata = Convert.ToString(data);
            }
        }
    }
}
