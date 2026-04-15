using Shared.Models;

namespace Shared.Interfaces
{
    public interface IChatUIService
    {
        /// <summary>
        /// Occurs when a new chat response is received.
        /// </summary>
        /// <remarks>Subscribers are notified each time a chat response is available. Handlers receive the
        /// response as a parameter. This event may be raised on a background thread; ensure thread safety when
        /// accessing shared resources.</remarks>
        public event Action<ChatResponse>? ChatResponseReceived;
        /// <summary>
        /// Occurs when a request is made to close the associated resource or component.
        /// </summary>
        /// <remarks>Subscribers can use this event to perform cleanup or prompt the user before closing. The
        /// event is typically raised when the user initiates a close action, such as clicking a close button.</remarks>
        public event Action? OnCloseRequested;

        /// <summary>
        /// Occurs when the value of the input field changes.
        /// </summary>
        /// <remarks>Subscribers receive the new value of the input field as a string. The value may be
        /// null if the input field is cleared or set to an undefined state.</remarks>
        public event Action<string?> InputFieldChanged;

        /// <summary>
        /// Occurs when a request to send a chat message is made.
        /// </summary>
        /// <remarks>Subscribers can handle this event to process or transmit the specified chat message.
        /// The event provides the message to be sent as a parameter.</remarks>
        event Action<ChatMessage>? SendMessageRequested;

        /// <summary>
        /// Represents an event that is raised when a state change is requested.
        /// </summary>
        /// <remarks>Subscribers can handle this event to respond to state change requests. The event is
        /// internal and intended for use within the assembly for the component to get the state change.</remarks>
        internal event Action? StateChangeRequested;
        /// <summary>
        /// This property can be used to determine if the application should support multiple themes.
        /// </summary>
        /// <value>
        /// <c>true</c> if multiple themes are allowed; otherwise, <c>false</c>.
        /// </value>
        bool AllowMultipleThemes { get; set; }

        /// <summary>
        /// Gets a value indicating whether voice input has been started.
        /// </summary>
        bool IsVoiceInputStarted { get; }



        #region Methods
        /// <summary>
        /// a method that is called when a chat response is received, allowing the implementing class to handle the response appropriately.
        /// </summary>
        /// <param name="response"></param>
        void OnChatResponseReceived(ChatResponse response);
        /// <summary>
        /// Requests that the associated view be closed.
        /// </summary>
        /// <remarks>This method is typically used to signal that the view should close,
        /// such as when a user completes or cancels an operation. The actual closing behavior depends on how the view
        /// or framework responds to the request.</remarks>
        public void RequestClose();

        /// <summary>
        /// Handles changes to the input field value.
        /// </summary>
        /// <param name="value">The new value of the input field, or null if the field is empty.</param>
        internal void OnInputFieldChanged(string? value);
        /// <summary>
        /// Handles a request to send a chat message within the chat system.
        /// </summary>
        /// <param name="message">The chat message to be sent. Cannot be null.</param>
        internal void OnSendMessageRequested(ChatMessage message);
        void VoiceInputStarted(bool voiceInputStarted);
        #endregion

    }
}