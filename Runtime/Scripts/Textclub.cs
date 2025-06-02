using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Textclub
{
    /// <summary>
    /// Main entry point for the Textclub SDK, providing access to various features and services.
    /// </summary>
    public sealed class Textclub
    {
        /// <summary>
        /// Provides access to notification management functionality.
        /// </summary>
        public readonly Notifications notifications = new();

        /// <summary>
        /// Provides access to analytics tracking and reporting.
        /// </summary>
        public readonly Analytics analytics = new();

        /// <summary>
        /// Provides access to player-related functionality and data.
        /// </summary>
        public readonly Player player = new();

        /// <summary>
        /// Initializes the Textclub SDK and ensures it's ready for use.
        /// </summary>
        /// <returns>A task that completes when the SDK is ready</returns>
        public TextclubTask Init()
        {
            return JsBridge.CallAsyncVoid("isReady");
        }

        /// <summary>
        /// Retrieves the entry payload data associated with the current session entry.
        ///
        /// It may contain data passed from an onboarding to a game or referral data from another entry.
        /// </summary>
        /// <returns>A dictionary containing the entry payload data</returns>
        /// <example>
        /// <code>
        ///     var textclub = new Textclub();
        ///     var entryPayload = textclub.GetEntryPayload();
        ///     if (entryPayload.TryGetValue("petName", out var petName))
        ///     {
        ///         Debug.Log($"Pet name: {petName}");
        ///     }
        /// </code>
        /// </example>
        public Dictionary<string, object> GetEntryPayload()
        {
            string payloadString = JsBridge.GetEntryPayload();
            return Convert.FromString<Dictionary<string, object>>(payloadString);
        }
    }
}
