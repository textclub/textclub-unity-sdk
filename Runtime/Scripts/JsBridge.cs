using System.Runtime.InteropServices;
using UnityEngine;

namespace Textclub
{
    internal static class JsBridge
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

        [DllImport("__Internal")]
        private static extern void JS_scheduleNotification(string options);

        [DllImport("__Internal")]
        private static extern void JS_captureEvent(string eventName, string properties);
#else
        private static string JS_getPlayerId() { Debug.Log("JS_getPlayerId"); return ""; }

        private static string JS_getIsRegistered() { Debug.Log("JS_getIsRegistered"); return ""; }

        private static string JS_getPlayerValue(string key) { Debug.Log($"JS_getPlayerValue {key}"); return ""; }

        private static void JS_setPlayerValue(string key, string value) { Debug.Log($"JS_setPlayerValue {key} | {value}"); }

        private static void JS_scheduleNotification(string options) { Debug.Log($"JS_scheduleNotification {options}"); }

        private static void JS_captureEvent(string eventName, string properties) { Debug.Log($"JS_captureEvent{eventName} {properties}"); }
#endif

        internal static string GetPlayerId()
        {
            return JS_getPlayerId();
        }

        internal static string GetIsRegistered()
        {
            return JS_getIsRegistered();
        }

        internal static string GetPlayerValue(string key)
        {
            return JS_getPlayerValue(key);
        }

        internal static void SetPlayerValue(string key, string value)
        {
            JS_setPlayerValue(key, value);
        }

        internal static void ScheduleNotification(string options)
        {
            JS_scheduleNotification(options);
        }

        internal static void CaptureEvent(string eventName, string properties)
        {
            JS_captureEvent(eventName, properties);
        }
    }
}
