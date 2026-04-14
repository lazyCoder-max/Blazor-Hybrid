using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromaAITextSrv.Models.Endpoints.Sessions
{
    public class SessionRequest
    {
        public string identity_token { get; set; }
        public string session_token { get; set; }
        public Meta meta { get; set; }
        public string default_language { get; set; }
    }

}




