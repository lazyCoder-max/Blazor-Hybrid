namespace Shared.Models
{
    public class ChatResponse
    {
        public string session_token { get; set; }
        public string mode { get; set; }
        public string language { get; set; }
        public Response response { get; set; }
        public bool rag_used { get; set; }
        public object[] rag_sources { get; set; }
        public int input_tokens { get; set; }
        public int output_tokens { get; set; }
        public int duration_ms { get; set; }
        public bool thinking_hidden { get; set; }
    }

}
