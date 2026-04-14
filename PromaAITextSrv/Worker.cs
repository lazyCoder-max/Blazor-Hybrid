using PromaAITextSrv.Helpers;
using PromaAITextSrv.Interfaces;
using PromaAITextSrv.Models;
using PromaAITextSrv.Models.Endpoints.Chat;
using System.Text;

namespace PromaAITextSrv
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IAPIService _apiService;
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly PromaAITextOptions _options;

        public Worker(
            ILogger<Worker> logger,
            IAPIService apiService,
            PromaAITextOptions promaAITextOptions,
            IMessageBrokerService messageBrokerService)
        {
            _logger = logger;
            _apiService = apiService;
            _options = promaAITextOptions;
            _messageBrokerService = messageBrokerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker starting RPC server at {time}", DateTimeOffset.Now);

            var consumerTag = await _messageBrokerService.StartRpcServerAsync<ChatRequest, ChatResponse>(
                RequestType.TextCorrection,
                request => HandleChatRequestAsync(request, stoppingToken),
                stoppingToken);

            if (string.IsNullOrWhiteSpace(consumerTag))
            {
                _logger.LogError("Failed to start RPC server for TextCorrection");
                return;
            }

            _logger.LogInformation("RPC server started, consumer tag: {Tag}", consumerTag);

            // Keep the service alive until cancellation is requested
            try
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Worker stopping at {time}", DateTimeOffset.Now);
            }
            finally
            {
                await _messageBrokerService.CleanupAsync();
            }
        }

        private async Task<ChatResponse> HandleChatRequestAsync(ChatRequest request, CancellationToken stoppingToken)
        {
            _logger.LogInformation("New RPC request received at {time}", DateTimeOffset.Now);

            var identityToken = IdentityTokenGenerator.Generate(_options.identity_secret);

            var sessionStatus = await _apiService.GetSessionStatusAsync(_options.session_token, stoppingToken);

            if (!sessionStatus.active)
            {
                var sessionResult = await _apiService.CreateSessionAsync(new Models.Endpoints.Sessions.SessionRequest
                {
                    default_language = request.language ?? "en",
                    identity_token = identityToken,
                    session_token = _options.session_token,
                    meta = new()
                    {
                        mandant_id = _options.mandant_id,
                        mandant_name = _options.mandant_name,
                        projekt_id = _options.projekt_id,
                        projekt_name = _options.projekt_name,
                        bericht_id = _options.bericht_id,
                        bericht_name = _options.bericht_name,
                        username = _options.username,
                        lizenz = _options.lizenz,
                    },
                });

                if (!sessionResult.success)
                {
                    _logger.LogError("Failed to create session");
                    return new ChatResponse();
                }

                _logger.LogInformation("Session created: {Token}", sessionResult.session_token);

                // Use the newly created session token
                request.session_token = sessionResult.session_token!;
            }
            else
            {
                request.session_token = sessionStatus.session_token!;
            }

            request.identity_token = identityToken;
            request.meta ??= new()
            {
                mandant_id = _options.mandant_id,
                mandant_name = _options.mandant_name,
                projekt_id = _options.projekt_id,
                projekt_name = _options.projekt_name,
                bericht_id = _options.bericht_id,
                bericht_name = _options.bericht_name,
                username = _options.username,
                lizenz = _options.lizenz,
            };

            var response = await _apiService.ChatAsync(request);
            _logger.LogInformation("Chat response received in {Duration}ms", response.duration_ms);

            return response;
        }
    }
}
