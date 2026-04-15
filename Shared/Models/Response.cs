namespace Shared.Models
{
    public class Response
    {
        public string corrected_text { get; set; }
        public Correction[] corrections { get; set; }
    }

}
