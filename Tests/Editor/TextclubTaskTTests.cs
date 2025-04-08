using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine.TestTools;

namespace Textclub.Tests
{
    public class TextclubTaskTTests
    {
        const int value = 42;

        [Test]
        public void TaskT_NewInstance_IsNotCompleted()
        {
            var task = new TextclubTask<int>();
            Assert.That(task.IsCompleted, Is.False);
            Assert.That(task.IsFaulted, Is.False);
            Assert.That(task.Exception, Is.Null);
        }

        [Test]
        public void TaskT_SetResult_CompletesTaskWithValue()
        {
            var task = new TextclubTask<int>();

            task.SetResult(value);

            Assert.That(task.IsCompleted, Is.True);
            Assert.That(task.IsFaulted, Is.False);
            Assert.That(task.Result, Is.EqualTo(value));
        }

        [Test]
        public void TaskT_SetResult_WorksWithReferenceType()
        {
            var task = new TextclubTask<string>();
            const string expected = "test value";

            task.SetResult(expected);

            Assert.That(task.IsCompleted, Is.True);
            Assert.That(task.Result, Is.EqualTo(expected));
        }

        [Test]
        public void TaskT_SetException_MarksTaskAsFaulted()
        {
            var task = new TextclubTask<int>();
            var exception = new Exception("Test exception");

            task.SetException(exception);

            Assert.That(task.IsCompleted, Is.True);
            Assert.That(task.IsFaulted, Is.True);
            Assert.That(task.Exception, Is.EqualTo(exception));
        }

        [UnityTest]
        public IEnumerator TaskT_WaitForCompletion_WaitsUntilTaskCompletes()
        {
            var task = new TextclubTask<int>();

            TestCoroutineRunner.StartCoroutine(CompleteTaskAfterDelay(task, value));

            yield return task.WaitForCompletion();

            Assert.That(task.IsCompleted, Is.True);
            Assert.That(task.Result, Is.EqualTo(value));
        }

        [Test]
        public void TaskT_GetResult_ThrowsWhenNotCompleted()
        {
            var task = new TextclubTask<int>();

            Assert.Throws<InvalidOperationException>(() => _ = task.GetResult());
        }

        [Test]
        public void TaskT_GetResult_ThrowsTaskException()
        {
            var task = new TextclubTask<int>();
            var exception = new Exception("Test exception");
            task.SetException(exception);

            var thrownException = Assert.Throws<Exception>(() => _ = task.GetResult());
            Assert.That(thrownException, Is.EqualTo(exception));
        }

        [Test]
        public void TaskT_ContinueWith_ExecutesAfterCompletionWithResult()
        {
            var task = new TextclubTask<int>();

            var continuationExecuted = false;
            int? capturedValue = null;

            var continuation = task.ContinueWith(t =>
            {
                continuationExecuted = true;
                capturedValue = t.Result;
            });

            Assert.That(continuationExecuted, Is.False);
            Assert.That(capturedValue, Is.Null);

            task.SetResult(value);

            Assert.That(continuationExecuted, Is.True);
            Assert.That(capturedValue, Is.EqualTo(value));
            Assert.That(continuation.IsCompleted, Is.True);
            Assert.That(continuation.Result, Is.EqualTo(value));
        }

        [Test]
        public void TaskT_SetResult_ThrowsWhenAlreadyCompleted()
        {
            const int otherValue = 43;

            var task = new TextclubTask<int>();
            task.SetResult(value);

            Assert.Throws<InvalidOperationException>(() => task.SetResult(otherValue));
        }

        [Test]
        public void TaskT_SetException_ThrowsWhenAlreadyCompleted()
        {
            var task = new TextclubTask<int>();
            task.SetResult(value);

            Assert.Throws<InvalidOperationException>(() =>
                task.SetException(new Exception("Test exception")));
        }

        [Test]
        public void TaskT_Awaiter_ReturnsCorrectResult()
        {
            var task = new TextclubTask<int>();

            var awaiter = task.GetAwaiter();
            Assert.That(awaiter.IsCompleted, Is.False);

            task.SetResult(value);

            Assert.That(awaiter.IsCompleted, Is.True);
            Assert.That(awaiter.GetResult(), Is.EqualTo(value));
        }

        private IEnumerator CompleteTaskAfterDelay<T>(TextclubTask<T> task, T result)
        {
            yield return null; // Wait one frame
            task.SetResult(result);
        }
    }

    // Complex type for testing reference type handling
    public class TestComplexType
    {
        public string StringValue { get; set; }
        public int IntValue { get; set; }
    }
}
