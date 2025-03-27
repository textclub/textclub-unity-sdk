using System;
using System.Runtime.CompilerServices;

namespace Textclub
{
    public readonly struct TextclubTaskBuilder
    {

        public TextclubTask Task { get; }

        private TextclubTaskBuilder(TextclubTask task) => Task = task;

        public void SetException(Exception e) => Task.SetException(e);

        public void SetResult() => Task.SetResult();

        public static TextclubTaskBuilder Create() => new TextclubTaskBuilder(new TextclubTask());

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
