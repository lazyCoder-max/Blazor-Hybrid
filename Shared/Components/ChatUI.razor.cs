using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using Shared.Interfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Components
{
    public partial class ChatUI
    {
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private readonly List<ChatMessage> _messages = [];
        private string? _userInput;
        private bool _isTyping;
        private bool _isDarkMode;
        private ElementReference _scrollContainer;
        private MudTextField<string?>? _inputField;

        private readonly MudTheme _theme = new()
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#1976d2",
                AppbarBackground = "#1976d2"
            },
            PaletteDark = new PaletteDark
            {
                Primary = "#90caf9",
                AppbarBackground = "#1e1e2e"
            }
        };
        protected override void OnInitialized()
        {
            NavService.StateChangeRequested += NavService_StateChangeRequested;
        }

        private void NavService_StateChangeRequested()
        {
            InvokeAsync(StateHasChanged);
        }

        private void OnInputChanged(string? value)
        {
            _userInput = value;
            NavService.OnInputFieldChanged(value);
        }
        private async Task HandleKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter" && !e.ShiftKey)
            {
                await SendMessage();
            }
        }
        private async Task StartVoiceInputAsync()
        {
            if(NavService.IsVoiceInputStarted)
            {
                NavService.VoiceInputStarted(false);
                await JSRuntime.InvokeVoidAsync("audioVisualizer.stop");
                return;
            }
            NavService.VoiceInputStarted(true);
            StateHasChanged();
            await Task.Delay(50); // allow canvas to render in DOM
            await JSRuntime.InvokeVoidAsync("audioVisualizer.start");
        }
        private async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(_userInput) || _isTyping)
                return;

            var userText = _userInput.Trim();
            _userInput = string.Empty;

            var message = new ChatMessage(true, DateTime.Now) { Content = userText };
            _messages.Add(message);
            NavService.OnSendMessageRequested(message);
            _isTyping = true;
            StateHasChanged();
            await ScrollToBottom();

            var response = await GetAIResponseAsync(userText);

            var assistantMessage = new ChatMessage(false, DateTime.Now) { Content = string.Empty };
            _messages.Add(assistantMessage);
            _isTyping = false;
            StateHasChanged();

            for (var i = 0; i < response.Length; i++)
            {
                assistantMessage.Content += response[i];
                StateHasChanged();
                await Task.Delay(20);

                if (i % 15 == 0)
                {
                    await ScrollToBottom();
                }
            }

            await ScrollToBottom();
        }

        private async Task ScrollToBottom()
        {
            await Task.Delay(50); // allow DOM to update
            await JSRuntime.InvokeVoidAsync("scrollToBottom", "message-area");
        }

        private void ClearChat()
        {
            _messages.Clear();
        }

        /// <summary>
        /// Replace this stub with your real AI model integration.
        /// </summary>
        private static async Task<string> GetAIResponseAsync(string prompt)
        {
            // Simulate network latency
            await Task.Delay(1000);
            return $"This is a placeholder response to: \"{prompt}\"\n\nReplace GetAIResponseAsync() with your actual AI model call.";
        }
    }
}
