using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class ChatRequest
    {
        public string identity_token { get; set; }
        public string session_token { get; set; }
        public string message { get; set; }
        public string mode { get; set; }
        public bool use_rag { get; set; }
        public Meta meta { get; set; }
        public string language { get; set; }
    }

}
