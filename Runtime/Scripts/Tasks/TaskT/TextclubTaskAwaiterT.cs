using System;
using System.Runtime.CompilerServices;

namespace Textclub
{
    public readonly struct TextclubTaskAwaiter<T> : INotifyCompletion
    {
        public bool IsCompleted => Task.IsCompleted;

        private TextclubTask<T> Task { get; }

        public TextclubTaskAwaiter(TextclubTask<T> task) => Task = task;

        public T GetResult() => Task.GetResult();

        public void OnCompleted(Action continuation) => Task.OnCompleted(continuation);
    }
}
