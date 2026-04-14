using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromaAITextSrv.Helpers
{
    public enum RequestType
    {
        TextRewrite = 0,
        TextCorrection = 1,
        Question = 2,
        TranscribeAudio = 3
    }
}
