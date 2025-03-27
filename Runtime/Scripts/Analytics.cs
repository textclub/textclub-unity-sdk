using System.Collections.Generic;
using UnityEngine;

namespace Textclub
{
    public class Analytics
    {
        internal Analytics() { }

        public void CaptureEvent<T>(string name, T properties) where T : struct
        {
            JsBridge.CaptureEvent(name, JsonUtility.ToJson(properties));
        }

        public void CaptureEvent(string name, Dictionary<string, object> properties)
        {
            JsBridge.CaptureEvent(name, Convert.ToString(properties));
        }

        internal T GetEvent<T>(string name) where T : struct
        {
            return JsonUtility.FromJson<T>(JsBridge.GetEvent(name));
        }

        public Dictionary<string, object> GetEvent(string name)
        {
            return Convert.FromString<Dictionary<string, object>>(JsBridge.GetEvent(name));
        }

    }
}
