using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
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

        [DllImport("__Internal")]
        private static extern void JS_testAsync(System.IntPtr ptr, System.Action<System.IntPtr> successCallback,
                                         System.Action<System.IntPtr, string> errorCallback);

#else
        private static string JS_getPlayerId() { return _bridgeMock.playerId; }

        private static string JS_getIsRegistered() { return _bridgeMock.isRegistered; }

        private static string JS_getPlayerValue(string key) { return _bridgeMock.GetPlayerValue(key); }

        private static void JS_setPlayerValue(string key, string value) { _bridgeMock.SetPlayerValue(key, value); }

        private static void JS_scheduleNotification(string options) { _bridgeMock.ScheduleNotification(options); }

        private static void JS_captureEvent(string eventName, string properties) { _bridgeMock.CaptureEvent(eventName, properties); }

        private static void JS_testAsync(System.IntPtr ptr, System.Action<System.IntPtr> successCallback,
                                         System.Action<System.IntPtr, string> errorCallback)
        { successCallback(ptr); }

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

        internal static string GetEvent(string eventName)
        {
            return _bridgeMock.GetEvent(eventName);
        }

        internal static List<string> GetNotifications()
        {
            return _bridgeMock.GetNotifications();
        }

        internal static TextclubTask TestAsync()
        {
            return new TextclubTask((System.IntPtr ptr) => { JS_testAsync(ptr, HandleSuccess, HandleError); });
        }

        [MonoPInvokeCallback(typeof(System.Action<System.IntPtr>))]
        public static void HandleSuccess(System.IntPtr taskPtr)
        {
            GCHandle handle = GCHandle.FromIntPtr(taskPtr);
            var task = (TextclubTask)handle.Target;
            task.SetResult();
            handle.Free();
        }

        [MonoPInvokeCallback(typeof(System.Action<System.IntPtr, string>))]
        public static void HandleError(System.IntPtr taskPtr, string error)
        {
            GCHandle handle = GCHandle.FromIntPtr(taskPtr);
            var task = (TextclubTask)handle.Target;
            task.SetException(new System.Exception(error));
            handle.Free();
        }
    }
}
