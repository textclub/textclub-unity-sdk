using System;
using System.Runtime.CompilerServices;

namespace Textclub
{
    /// <summary>
    /// Provides a builder for TextclubTask<T> that implements the async/await pattern.
    /// </summary>
    /// <typeparam name="T">The type of the result produced by the task being built</typeparam>
    public readonly struct TextclubTaskBuilder<T>
    {
        /// <summary>
        /// Gets the underlying task being built.
        /// </summary>
        public TextclubTask<T> Task { get; }

        /// <summary>
        /// Initializes a new instance of TextclubTaskBuilder<T>.
        /// </summary>
        /// <param name="task">The task to build</param>
        private TextclubTaskBuilder(TextclubTask<T> task) => Task = task;

        /// <summary>
        /// Creates a new instance of TextclubTaskBuilder<T>.
        /// </summary>
        /// <returns>A new TextclubTaskBuilder&lt;T> instance</returns>
        public static TextclubTaskBuilder<T> Create() => new TextclubTaskBuilder<T>(new TextclubTask<T>());

        /// <summary>
        /// Sets the task to a faulted state with the specified exception.
        /// </summary>
        /// <param name="exception">The exception that caused the fault</param>
        public void SetException(Exception exception) => Task.SetException(exception);

        /// <summary>
        /// Sets the task's result and marks it as completed.
        /// </summary>
        /// <param name="result">The result value to set</param>
        public void SetResult(T result) => Task.SetResult(result);

        /// <summary>
        /// Starts the state machine.
        /// </summary>
        /// <typeparam name="TStateMachine">The type of the state machine</typeparam>
        /// <param name="stateMachine">The state machine to start</param>
        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine => stateMachine.MoveNext();

        /// <summary>
        /// Schedules the continuation when the awaiter completes.
        /// </summary>
        /// <typeparam name="TAwaiter">The type of the awaiter</typeparam>
        /// <typeparam name="TStateMachine">The type of the state machine</typeparam>
        /// <param name="awaiter">The awaiter</param>
        /// <param name="stateMachine">The state machine to continue</param>
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine => awaiter.OnCompleted(stateMachine.MoveNext);

        /// <summary>
        /// Schedules the continuation when the awaiter completes, allowing for unsafe continuations.
        /// </summary>
        /// <typeparam name="TAwaiter">The type of the awaiter</typeparam>
        /// <typeparam name="TStateMachine">The type of the state machine</typeparam>
        /// <param name="awaiter">The awaiter</param>
        /// <param name="stateMachine">The state machine to continue</param>
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine => awaiter.UnsafeOnCompleted(stateMachine.MoveNext);

        public void SetStateMachine(IAsyncStateMachine _)
        {
            // method is required, but there's nothing to do here
        }
    }
}
