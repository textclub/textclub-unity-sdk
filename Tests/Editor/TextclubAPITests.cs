using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Textclub.Tests
{
    public class TextclubAPITests
    {
        private Textclub _textclub;
        private TestBridgeMock _mock;

        private string testId = "test-player-id";

        [SetUp]
        public void Setup()
        {
            _mock = new TestBridgeMock(testId, true);
            JsBridge.SetMock(_mock);
            _textclub = new Textclub();
        }

        [Test]
        public void Player_GetId_ReturnsCorrectId()
        {
            Assert.That(_textclub.player.id, Is.EqualTo(testId));
        }

        [Test]
        public void Player_IsRegistered_ReturnsCorrectValue()
        {
            Assert.That(_textclub.player.isRegistered, Is.True);
        }

        [Test]
        public void Player_SetAndGetValue_WorksWithPrimitives()
        {
            _textclub.player.Set("bool", true);
            _textclub.player.Set("int", 42);
            _textclub.player.Set("float", 3.14f);
            _textclub.player.Set("string", "test");

            Assert.That(_textclub.player.Get<bool>("bool"), Is.True);
            Assert.That(_textclub.player.Get<int>("int"), Is.EqualTo(42));
            Assert.That(_textclub.player.Get<float>("float"), Is.EqualTo(3.14f));
            Assert.That(_textclub.player.Get<string>("string"), Is.EqualTo("test"));
        }

        [Test]
        public void Player_TryGet_ReturnsCorrectValue()
        {
            const string key = "key";
            const string value = "value";
            const string anotherKey = "anotherKey";

            _textclub.player.Set(key, value);

            Assert.That(_textclub.player.TryGet(key, out string val) && val == value);
            Assert.That(!_textclub.player.TryGet(anotherKey, out string anotherVal));
        }

        [Test]
        public void Player_SetAndGetValue_WorksWithVectors()
        {
            var vector2 = new Vector2(1, 2);
            var vector3 = new Vector3(1, 2, 3);

            _textclub.player.Set("vector2", vector2);
            _textclub.player.Set("vector3", vector3);

            Assert.That(_textclub.player.Get<Vector2>("vector2"), Is.EqualTo(vector2));
            Assert.That(_textclub.player.Get<Vector3>("vector3"), Is.EqualTo(vector3));
        }

        [Test]
        public void GetEntryPayload_ReturnsCorrectValue()
        {
            var expectedPayload = new Dictionary<string, object>
            {
                { "key1", "value1" },
                { "key2", 42 }
            };
            _mock.SetEntryPayload(expectedPayload);

            var actualPayload = _textclub.GetEntryPayload();

            Assert.That(actualPayload, Is.EqualTo(expectedPayload));
        }

        [Test]
        public void Analytics_CaptureEvent_StoresEventData()
        {
            var eventName = "test-event";
            var eventData = new TestEventData { stringValue = "test", numberValue = 42 };
            _textclub.analytics.CaptureEvent(eventName, eventData);

            var parsedEvent = _textclub.analytics.GetEvent<TestEventData>(eventName);

            Assert.AreEqual(eventData, parsedEvent);
        }

        [Test]
        public void Analytics_CaptureEvent_StoresEventData_Dictionary()
        {
            var eventName = "test-event";
            var eventData = new Dictionary<string, object> { { "stringValue", "test" }, { "numberValue", 42 } };
            _textclub.analytics.CaptureEvent(eventName, eventData);

            var parsedData = _textclub.analytics.GetEvent(eventName);

            Assert.AreEqual(eventData, parsedData);
        }

        [Test]
        public void Notifications_ScheduleNotification_StoresNotification()
        {
            var date = DateTime.Now;
            var options = new Notifications.Options
            {
                message = "Test notification",
                date = date,
                deduplicationKey = "test-key",
                attemptPushNotification = true,
                data = { { "stringValue", "test" }, { "numberValue", 42 } }
            };

            _textclub.notifications.ScheduleNotification(options);

            var notifications = _textclub.notifications.GetNotifications();
            Assert.That(notifications, Has.Count.EqualTo(1));

            var result = notifications[0];
            Assert.AreEqual(options.attemptPushNotification, result.attemptPushNotification);
            Assert.AreEqual(options.deduplicationKey, result.deduplicationKey);
            Assert.AreEqual(options.date, result.date);
            Assert.AreEqual(options.data, result.data);
            Assert.AreEqual(options.message, result.message);
        }

        [System.Serializable]
        private struct TestEventData
        {
            public string stringValue;
            public int numberValue;
        }
    }
}
