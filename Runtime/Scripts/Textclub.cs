using System.Runtime.InteropServices;
using UnityEngine;

namespace Textclub
{
    public class Textclub
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern string JS_getPlayerId();

        [DllImport("__Internal")]
        private static extern string JS_getIsRegistered();

        [DllImport("__Internal")]
        private static extern string JS_getPlayerValue(string key);

        [DllImport("__Internal")]
        private static extern void JS_setPlayerValue(string key, string value);
#else
        private string JS_getPlayerId() { Debug.Log("JS_getPlayerId"); return ""; }
        private string JS_getIsRegistered() { Debug.Log("JS_getIsRegistered"); return ""; }
        private string JS_getPlayerValue(string key) { Debug.Log($"JS_getPlayerValue {key}"); return ""; }
        private void JS_setPlayerValue(string key, string value) { Debug.Log($"JS_setPlayerValue {key} | {value}"); }

#endif
        public string playerId => JS_getPlayerId();

        public bool isRegistered
        {
            get
            {
                var isReg = JS_getIsRegistered();
                if (bool.TryParse(isReg, out bool result))
                {
                    return result;
                }
                else
                {
                    Debug.LogError($"Could not parse JS_getIsRegistered. Value: {isReg}");
                    return false;
                }
            }
        }

        public string GetPlayerValue(string key)
        {
            return JS_getPlayerValue(key);
        }

        public T GetPlayerValue<T>(string key)
        {
            var str = JS_getPlayerValue(key);
            return Convert.FromString<T>(str);
        }

        public void SetPlayerValue<T>(string key, T value)
        {
            JS_setPlayerValue(key, Convert.ToString(value));
        }

        public void SetPlayerValue(string key, string value)
        {
            JS_setPlayerValue(key, value);
        }
    }
}
