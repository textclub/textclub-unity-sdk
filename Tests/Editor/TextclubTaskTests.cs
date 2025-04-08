using NUnit.Framework;
using System;
using System.Collections;
using UnityEngine.TestTools;

namespace Textclub.Tests
{
    public class TextclubTaskTests
    {
        [Test]
        public void Task_NewInstance_IsNotCompleted()
        {
            var task = new TextclubTask();
            Assert.That(task.IsCompleted, Is.False);
            Assert.That(task.IsFaulted, Is.False);
            Assert.That(task.Exception, Is.Null);
        }

        [Test]
        public void Task_SetResult_CompletesTask()
        {
            var task = new TextclubTask();
            task.SetResult();

            Assert.That(task.IsCompleted, Is.True);
            Assert.That(task.IsFaulted, Is.False);
        }

        [Test]
        public void Task_SetException_MarksTaskAsFaulted()
        {
            var task = new TextclubTask();
            var exception = new Exception("Test exception");

            task.SetException(exception);

            Assert.That(task.IsCompleted, Is.True);
            Assert.That(task.IsFaulted, Is.True);
            Assert.That(task.Exception, Is.EqualTo(exception));
        }

        [UnityTest]
        public IEnumerator Task_WaitForCompletion_WaitsUntilTaskCompletes()
        {
            var task = new TextclubTask();

            // Start a coroutine that completes the task after a frame
            TestCoroutineRunner.StartCoroutine(CompleteTaskAfterDelay(task));

            // Wait for completion
            yield return task.WaitForCompletion();

            Assert.That(task.IsCompleted, Is.True);
        }

        [Test]
        public void Task_GetResult_ThrowsWhenNotCompleted()
        {
            var task = new TextclubTask();

            Assert.Throws<InvalidOperationException>(() => task.GetResult());
        }

        [Test]
        public void Task_GetResult_ThrowsTaskException()
        {
            var task = new TextclubTask();
            var exception = new Exception("Test exception");
            task.SetException(exception);

            var thrownException = Assert.Throws<Exception>(() => task.GetResult());
            Assert.That(thrownException, Is.EqualTo(exception));
        }

        [Test]
        public void Task_ContinueWith_ExecutesAfterCompletion()
        {
            var task = new TextclubTask();
            var continuationExecuted = false;

            var continuation = task.ContinueWith(_ => continuationExecuted = true);
            Assert.That(continuationExecuted, Is.False);

            task.SetResult();
            Assert.That(continuationExecuted, Is.True);
            Assert.That(continuation.IsCompleted, Is.True);
        }

        [Test]
        public void Task_SetResult_ThrowsWhenAlreadyCompleted()
        {
            var task = new TextclubTask();
            task.SetResult();

            Assert.Throws<InvalidOperationException>(() => task.SetResult());
        }

        [Test]
        public void Task_SetException_ThrowsWhenAlreadyCompleted()
        {
            var task = new TextclubTask();
            task.SetResult();

            Assert.Throws<InvalidOperationException>(() =>
                task.SetException(new Exception("Test exception")));
        }

        private IEnumerator CompleteTaskAfterDelay(TextclubTask task)
        {
            yield return null; // Wait one frame
            task.SetResult();
        }
    }

    // Helper class to run coroutines in tests
    public static class TestCoroutineRunner
    {
        public static void StartCoroutine(IEnumerator routine)
        {
            while (routine.MoveNext())
            {
                // Process the coroutine step
                if (routine.Current is IEnumerator subroutine)
                {
                    StartCoroutine(subroutine);
                }
            }
        }
    }
}
