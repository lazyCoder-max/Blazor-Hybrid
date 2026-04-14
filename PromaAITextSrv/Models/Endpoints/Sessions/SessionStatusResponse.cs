namespace PromaAITextSrv.Models.Endpoints.Sessions
{
    public class SessionStatusResponse
    {
        public string session_token { get; set; }
        public bool active { get; set; }
        public DateTime created_at { get; set; }
        public DateTime last_active { get; set; }
        public int message_count { get; set; }
        public string rag_collection { get; set; }
        public Meta meta { get; set; }
        public int expires_in_seconds { get; set; }
        public string default_language { get; set; }
    }

}




