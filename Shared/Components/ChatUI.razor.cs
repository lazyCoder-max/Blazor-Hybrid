using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using Shared.Interfaces;
using Shared.Models;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.CategoryTypes;

namespace Shared.Components
{
    public partial class ChatUI
    {
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] private Shared.Interfaces.IMessageBrokerService _messageBrokerService { get; set; } = default!;

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
            ChatUIService.StateChangeRequested += ChatUIService_StateChangeRequested;
            ChatUIService.ChatResponseReceived += ChatUIService_ChatResponseReceived;
        }

        private async void ChatUIService_ChatResponseReceived(ChatResponse obj)
        {
            obj.response.corrected_text = $"**[Corrected Text]**:  <br>{obj.response.corrected_text}";
            var assistantMessage = new ChatMessage(false, DateTime.Now) { Content = string.Empty };
            for (var i = 0; i < obj.response.corrected_text.Length; i++)
            {
                assistantMessage.Content += obj.response.corrected_text[i];
                StateHasChanged();
                await Task.Delay(20);

                if (i % 15 == 0)
                {
                    await ScrollToBottom();
                }
            }
            _messages.Add(assistantMessage);
            _isTyping = false;
            StateHasChanged();
            await ScrollToBottom();
        }

        private void ChatUIService_StateChangeRequested()
        {
            InvokeAsync(StateHasChanged);
        }

        private void OnInputChanged(string? value)
        {
            _userInput = value;
            ChatUIService.OnInputFieldChanged(value);
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
            if(ChatUIService.IsVoiceInputStarted)
            {
                ChatUIService.VoiceInputStarted(false);
                await JSRuntime.InvokeVoidAsync("audioVisualizer.stop");
                return;
            }
            ChatUIService.VoiceInputStarted(true);
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
            ChatUIService.OnSendMessageRequested(message);
            _isTyping = true;
            StateHasChanged();
            await ScrollToBottom();

            var response = await _messageBrokerService.CallRpcAsync<ChatRequest, ChatResponse>(
                                Shared.Helpers.RequestType.TextCorrection,
                        new ChatRequest
                        {
                            message = userText,
                            mode = "text_correction",
                            language = "en",
                            use_rag = false
                        });
            if (response!.response == null)
            {
                return;
            }

            ChatUIService.OnChatResponseReceived(response);
            
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

    }
}
