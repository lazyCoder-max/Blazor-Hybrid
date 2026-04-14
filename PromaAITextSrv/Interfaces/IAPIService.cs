using PromaAITextSrv.Models.Endpoints.Chat;
using PromaAITextSrv.Models.Endpoints.Sessions;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromaAITextSrv.Interfaces
{
    public interface IAPIService
    {
        [Post("/session/create")]
        Task<SessionResponse> CreateSessionAsync([Body] SessionRequest body);
        [Get("/session/{sessionToken}/status")]
        Task<SessionResponse> GetSessionStatusAsync([AliasAs("sessionToken")] string sessionToken, CancellationToken cancellationToken);

        [Post("/chat")]
        Task<ChatResponse> ChatAsync([Body] ChatRequest body);
    }
}
