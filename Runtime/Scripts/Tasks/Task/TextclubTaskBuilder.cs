using System;
using System.Runtime.CompilerServices;

namespace Textclub
{
    /// <summary>
    /// Provides a builder for TextclubTask that implements the async/await pattern.
    /// </summary>
    public readonly struct TextclubTaskBuilder
    {
        /// <summary>
        /// Gets the underlying task being built.
        /// </summary>
        public TextclubTask Task { get; }

        /// <summary>
        /// Initializes a new instance of TextclubTaskBuilder.
        /// </summary>
        /// <param name="task">The task to build</param>
        private TextclubTaskBuilder(TextclubTask task) => Task = task;

        /// <summary>
        /// Sets the task to a faulted state with the specified exception.
        /// </summary>
        /// <param name="e">The exception that caused the fault</param>
        public void SetException(Exception e) => Task.SetException(e);

        /// <summary>
        /// Sets the task as completed successfully.
        /// </summary>
        public void SetResult() => Task.SetResult();

        /// <summary>
        /// Creates a new instance of TextclubTaskBuilder.
        /// </summary>
        /// <returns>A new TextclubTaskBuilder instance</returns>
        public static TextclubTaskBuilder Create() => new TextclubTaskBuilder(new TextclubTask());

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

        /// <summary>
        /// Sets the state machine. This method is required by the compiler but has no implementation.
        /// </summary>
        /// <param name="_">The state machine to set</param>
        public void SetStateMachine(IAsyncStateMachine _)
        {
            // method is required, but there's nothing to do here
        }
    }
}
