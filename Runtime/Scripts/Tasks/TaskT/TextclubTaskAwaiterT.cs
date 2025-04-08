using System;
using System.Runtime.CompilerServices;

namespace Textclub
{
    /// <summary>
    /// Provides an awaiter for TextclubTask<T> that implements the async/await pattern.
    /// </summary>
    /// <typeparam name="T">The type of the result produced by the associated task</typeparam>
    public readonly struct TextclubTaskAwaiter<T> : INotifyCompletion
    {
        /// <summary>
        /// Gets whether the task has completed.
        /// </summary>
        public bool IsCompleted => Task.IsCompleted;

        /// <summary>
        /// The underlying task being awaited.
        /// </summary>
        private TextclubTask<T> Task { get; }

        /// <summary>
        /// Initializes a new instance of TextclubTaskAwaiter<T>;.
        /// </summary>
        /// <param name="task">The task to be awaited</param>
        public TextclubTaskAwaiter(TextclubTask<T> task) => Task = task;

        /// <summary>
        /// Gets the result of the task.
        /// </summary>
        /// <returns>The result value of type T from the completed task</returns>
        public T GetResult() => Task.GetResult();

        /// <summary>
        /// Schedules the continuation action to be invoked when the task completes.
        /// </summary>
        /// <param name="continuation">The action to invoke when the task completes</param>
        public void OnCompleted(Action continuation) => Task.OnCompleted(continuation);
    }
}
