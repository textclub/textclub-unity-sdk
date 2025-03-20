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
    }
}
