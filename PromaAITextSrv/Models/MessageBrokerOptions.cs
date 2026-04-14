using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromaAITextSrv.Models
{
    public class MessageBrokerOptions
    {
        public const string Key = "MessageBrokerOptions";
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string URL { get; set; } = string.Empty;
        public string VirtualHost { get; set; } = "/";
    }
    
}
