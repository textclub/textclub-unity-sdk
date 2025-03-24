using NUnit.Framework;
using UnityEngine;
using System;

namespace Textclub.Tests
{
    public class TextclubTests
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
        public void Player_SetAndGetValue_WorksWithVectors()
        {
            var vector2 = new Vector2(1, 2);
            var vector3 = new Vector3(1, 2, 3);

            _textclub.player.Set("vector2", vector2);
            _textclub.player.Set("vector3", vector3);

            Assert.That(_textclub.player.Get<Vector2>("vector2"), Is.EqualTo(vector2));
            Assert.That(_textclub.player.Get<Vector3>("vector3"), Is.EqualTo(vector3));
        }

        public void Analytics_CaptureEvent_StoresEventData()
        {
            var eventName = "test-event";
            var eventData = new TestEventData { stringValue = "test", numberValue = 42 };
            _textclub.analytics.CaptureEvent(eventName, eventData);

            var parsedEvent = _textclub.analytics.GetEvent<TestEventData>(eventName);

            Assert.AreEqual(eventData, parsedEvent);
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
                attemptPushNotification = true
            };

            _textclub.notifications.ScheduleNotification(options);

            var notifications = _textclub.notifications.GetNotifications();
            Assert.That(notifications, Has.Count.EqualTo(1));

            var result = notifications[0];
            Assert.AreEqual(options.attemptPushNotification, result.attemptPushNotification);
            Assert.AreEqual(options.deduplicationKey, result.deduplicationKey);
            Assert.AreEqual(options.date, result.date);
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
