using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Textclub
{
    [CreateAssetMenu(fileName = "ScriptableMock", menuName = "Textclub/ScriptableMock")]
    public class ScriptableMock : ScriptableObject, IBridgeMock
    {
        [SerializeField] private string _playerId;
        [SerializeField] private bool _isRegistered;
        [SerializeField] private List<Notifications.Options> _notifications;
        [SerializeField] private List<ValuePair> _values;
        [SerializeField] private List<ValuePair> _events;

        public string playerId => _playerId;

        public string isRegistered => _isRegistered.ToString();


#if UNITY_EDITOR
        private void OnEnable()
        {
            Debug.Log("OnEnable");
            JsBridge.SetMock(this);
        }

        private void OnDisable()
        {
            Debug.Log("OnEnable");
            JsBridge.SetMock(this);
        }
#endif
        public void CaptureEvent(string eventName, string properties)
        {
            _events.Add(new ValuePair { key = eventName, value = properties });
        }

        public string GetPlayerValue(string key)
        {
            return _values.Find(vp => vp.key == key).value;
        }

        public void SetPlayerValue(string key, string value)
        {
            int index = _values.FindIndex(vp => vp.key == key);
            var pair = new ValuePair { key = key, value = value };
            if (index >= 0)
            {
                _values[index] = pair;
            }
            else
            {
                _values.Add(pair);
            }
        }

        public void ScheduleNotification(string options)
        {
            _notifications.Add(JsonUtility.FromJson<Notifications.Options>(options));
        }

        public string GetEvent(string eventName)
        {
            return _events.Find(e => e.key == eventName).value;
        }

        public List<string> GetNotifications()
        {
            return _notifications.Select(n => JsonUtility.ToJson(n)).ToList();
        }

        [System.Serializable]
        public struct ValuePair
        {
            public string key;
            public string value;
        }
    }
}
