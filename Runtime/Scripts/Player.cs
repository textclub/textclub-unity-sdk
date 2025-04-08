using UnityEngine;

namespace Textclub
{
    public sealed class Player
    {
        public string id => JsBridge.GetPlayerId();

        public bool isRegistered
        {
            get
            {
                var isReg = JsBridge.GetIsRegistered();
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

        internal Player() { }

        public string Get(string key)
        {
            return JsBridge.GetPlayerValue(key);
        }

        public bool TryGet(string key, out string value)
        {
            value = JsBridge.GetPlayerValue(key);
            return !string.IsNullOrEmpty(value);
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default;
            if (TryGet(key, out string val))
            {
                value = Convert.FromString<T>(val);
                return true;
            }

            return false;
        }

        public T Get<T>(string key)
        {
            var str = JsBridge.GetPlayerValue(key);
            return Convert.FromString<T>(str);
        }

        public void Set<T>(string key, T value)
        {
            JsBridge.SetPlayerValue(key, Convert.ToString(value));
        }

        public void Set(string key, string value)
        {
            JsBridge.SetPlayerValue(key, value);
        }
    }
}
