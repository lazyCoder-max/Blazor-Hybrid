namespace Shared.Models
{
    public sealed record ChatMessage(bool IsUser, DateTime Timestamp)
    {
        public string Content { get; set; } = string.Empty;
    }
}
