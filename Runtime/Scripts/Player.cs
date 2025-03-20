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
