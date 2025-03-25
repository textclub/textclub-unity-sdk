using System;
using System.Collections;
using System.Runtime.InteropServices;


namespace Textclub
{
    public class TextclubTask
    {

        public Exception Exception { get; private set; }

        public bool IsFaulted { get => Exception != null; }

        public bool IsCompleted { get; private set; }

        private TextclubTaskAwaiter Awaiter { get; }

        private event Action Finished;

        public TextclubTaskAwaiter GetAwaiter() => Awaiter;

        public TextclubTask() => Awaiter = new TextclubTaskAwaiter(this);

        internal TextclubTask(Action<IntPtr> action)
        {
            Awaiter = new TextclubTaskAwaiter(this);
            GCHandle handle = GCHandle.Alloc(this);
            IntPtr taskPtr = GCHandle.ToIntPtr(handle);
            action(taskPtr);
        }

        /// <summary>
        public IEnumerator WaitForCompletion()
        {
            while (!IsCompleted)
            {
                yield return null;
            }
        }

        public TextclubTask ContinueWith(Action<TextclubTask> action)
        {
            var continuationTask = new TextclubTask();
            OnCompleted(() =>
            {
                action(this);
                continuationTask.SetResult();
            });
            return continuationTask;
        }

        internal void GetResult()
        {
            if (!IsCompleted)
            {
                throw new InvalidOperationException("Task is not completed");
            }

            if (IsFaulted)
            {
                throw this.Exception;
            }
        }

        internal void SetResult()
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("SetResult failed. Task is already completed.");
            }

            IsCompleted = true;
            Finished?.Invoke();
        }

        internal void SetException(Exception e)
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("SetException failed. Task is already completed.", e);
            }

            IsCompleted = true;
            Exception = e;
            Finished?.Invoke();
        }

        internal void OnCompleted(Action continuation)
        {
            if (IsCompleted)
            {
                continuation();
            }
            else
            {
                Finished += continuation;
            }
        }
    }
}
