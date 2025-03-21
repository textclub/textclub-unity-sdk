using System.Runtime.InteropServices;

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
        private static string JS_getPlayerId() { return _bridgeMock.playerId; }

        private static string JS_getIsRegistered() { return _bridgeMock.isRegistered; }

        private static string JS_getPlayerValue(string key) { return _bridgeMock.GetPlayerValue(key); }

        private static void JS_setPlayerValue(string key, string value) { _bridgeMock.SetPlayerValue(key, value); }

        private static void JS_scheduleNotification(string options) { _bridgeMock.ScheduleNotification(options); }

        private static void JS_captureEvent(string eventName, string properties) { _bridgeMock.CaptureEvent(eventName, properties); }
#endif

        private static IBridgeMock _bridgeMock = new DebugBridgeMock("playerId", true);

        internal static void SetMock(IBridgeMock mock)
        {
            _bridgeMock = mock;
        }

        internal static void ClearMock(IBridgeMock mock)
        {
            if (_bridgeMock == mock)
            {
                _bridgeMock = new DebugBridgeMock("playerId", true);
            }
        }

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
