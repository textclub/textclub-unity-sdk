using System;
using System.Runtime.CompilerServices;

namespace Textclub
{
    public readonly struct TextclubTaskBuilder<T>
    {
        public TextclubTask<T> Task { get; }

        private TextclubTaskBuilder(TextclubTask<T> task) => Task = task;

        public void SetException(Exception e) => Task.SetException(e);

        public void SetResult(T result) => Task.SetResult(result);

        public static TextclubTaskBuilder<T> Create() => new TextclubTaskBuilder<T>(new TextclubTask<T>());

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine => stateMachine.MoveNext();

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine => awaiter.OnCompleted(stateMachine.MoveNext);

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine => awaiter.UnsafeOnCompleted(stateMachine.MoveNext);

        public void SetStateMachine(IAsyncStateMachine _)
        {
            // method is required, but there's nothing to do here
        }
    }

}
