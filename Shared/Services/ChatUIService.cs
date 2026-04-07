using Shared.Interfaces;
using Shared.Models;

namespace Shared.Services;

internal sealed class ChatUIService : IChatUIService
{
    private bool _allowMultipleThemes = false;
    private bool _isVoiceInputStarted = false;

    public event Action? OnCloseRequested;
    public event Action<string?> InputFieldChanged = default!;
    public event Action<ChatMessage>? SendMessageRequested;
    public event Action? StateChangeRequested;

    bool IChatUIService.AllowMultipleThemes 
    { 
        get => _allowMultipleThemes;
        set
        {
            _allowMultipleThemes = value;
            StateChangeRequested?.Invoke();
        }
    }

    public bool IsVoiceInputStarted
    {
        get { return _isVoiceInputStarted; }
    }

    void IChatUIService.OnInputFieldChanged(string? value)
    {
        InputFieldChanged?.Invoke(value);
    }

    void IChatUIService.OnSendMessageRequested(ChatMessage message)
    {
        SendMessageRequested?.Invoke(message);
    }

    void IChatUIService.RequestClose()
    {
        OnCloseRequested?.Invoke();
    }

    public void VoiceInputStarted(bool voiceInputStarted)
    {
        _isVoiceInputStarted = voiceInputStarted;
    }
}