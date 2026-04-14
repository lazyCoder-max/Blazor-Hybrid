using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromaAITextSrv.Models
{
    public class PromaAITextOptions
    {
        public const string SectionName = "PromaAITextOptions";
        public string X_API_Token { get; set; } = string.Empty;
        public string identity_secret { get; set; } = string.Empty;
        public string identity_token { get; set; } = string.Empty;
        public string session_token { get; set; } = string.Empty;
        public string mandant_id { get; set; } = string.Empty;
        public string mandant_name { get; set; } = string.Empty;
        public string projekt_id { get; set; } = string.Empty;
        public string projekt_name { get; set; } = string.Empty;
        public string bericht_id { get; set; } = string.Empty;
        public string bericht_name { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public string lizenz { get; set; } = string.Empty;
        public string language { get; set; } = string.Empty;
        public string webhook_url { get; set; } = string.Empty;
        public string base_url { get; set; } = string.Empty;
    }

}
