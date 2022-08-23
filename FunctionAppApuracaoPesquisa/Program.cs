using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FunctionAppApuracaoPesquisa.Data;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((services) =>
    {
        services.AddScoped<IVotacaoRepository, VotacaoRepository>();
    })
    .Build();

host.Run();
