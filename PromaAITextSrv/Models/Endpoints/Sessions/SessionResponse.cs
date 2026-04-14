namespace PromaAITextSrv.Models.Endpoints.Sessions
{
    public class SessionResponse
    {
        public bool success { get; set; }
        public string? session_token { get; set; }
        public int? expires_in_seconds { get; set; }
        public string? message { get; set; }
        public string? default_language { get; set; }
        public object? meta { get; set; }
        public object? rag_collection { get; set; }
        public int? message_count { get; set; }
        public DateTime? last_active { get; set; }
        public DateTime? created_at { get; set; }
        public bool active { get; set; }

    }

}




