namespace PromaAITextSrv.Models.Endpoints.Chat
{
    public class Correction
    {
        public string type { get; set; }
        public string original { get; set; }
        public string corrected { get; set; }
        public string reason { get; set; }
    }

}
