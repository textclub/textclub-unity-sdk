using System;
using System.Runtime.CompilerServices;

namespace Textclub
{
    public readonly struct TextclubTaskAwaiter : INotifyCompletion
    {
        public bool IsCompleted => Task.IsCompleted;

        private TextclubTask Task { get; }

        public TextclubTaskAwaiter(TextclubTask task) => Task = task;

        public void GetResult() => Task.GetResult();

        public void OnCompleted(Action continuation) => Task.OnCompleted(continuation);
    }
}
