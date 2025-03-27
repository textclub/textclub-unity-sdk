using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Textclub
{
    [AsyncMethodBuilder(typeof(TextclubTaskBuilder<>))]
    public class TextclubTask<T>
    {

        public Exception Exception { get; private set; }

        public bool IsFaulted { get => Exception != null; }

        public bool IsCompleted { get; private set; }

        private TextclubTaskAwaiter<T> Awaiter { get; }

        private event Action Finished;

        public TextclubTaskAwaiter<T> GetAwaiter() => Awaiter;

        public TextclubTask() => Awaiter = new TextclubTaskAwaiter<T>(this);

        public T Result
        {
            get => GetResult();
            private set => SetResult(value);
        }

        private T _result;

        internal TextclubTask(Action<IntPtr> action)
        {
            Awaiter = new TextclubTaskAwaiter<T>(this);
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

        public TextclubTask<T> ContinueWith(Action<TextclubTask<T>> action)
        {
            var continuationTask = new TextclubTask<T>();
            OnCompleted(() =>
            {
                action(this);
                continuationTask.SetResult(Result);
            });
            return continuationTask;
        }

        internal T GetResult()
        {
            if (!IsCompleted)
            {
                throw new InvalidOperationException("Task is not completed");
            }

            if (IsFaulted)
            {
                throw this.Exception;
            }

            return _result;
        }

        internal void SetResult(T result)
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException("SetResult failed. Task is already completed.");
            }

            _result = result;
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
