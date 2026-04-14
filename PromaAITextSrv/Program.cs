using PromaAITextSrv;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddPromanAITextService(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
