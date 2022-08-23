using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FunctionAppApuracaoPesquisa.EventHubs;
using FunctionAppApuracaoPesquisa.Data;

namespace FunctionAppApuracaoPesquisa;

public class ProcessarVoto
{
    private readonly ILogger _logger;
    private readonly IVotacaoRepository _repository;

    public ProcessarVoto(ILoggerFactory loggerFactory,
        IVotacaoRepository repository)
    {
        _logger = loggerFactory.CreateLogger<ProcessarVoto>();
        _repository = repository;
    }

    [Function("ProcessarVoto")]
    public void Run([EventHubTrigger(AzureEventHubsConfigurations.EventHubName,
        ConsumerGroup = AzureEventHubsConfigurations.ConsumerGroup,
        Connection = "AzureEventHubs")] VotoEventData[] input)
    {
        foreach (var voto in input)
        {
            bool dataProcessed = false;
            if (voto is not null &&
                !String.IsNullOrWhiteSpace(voto.IdVoto) &&
                !String.IsNullOrWhiteSpace(voto.Horario) &&
                !String.IsNullOrWhiteSpace(voto.Producer) &&
                !String.IsNullOrWhiteSpace(voto.Interesse))
            {
                _repository.Save(voto);
                _logger.LogInformation($"Voto = {voto.IdVoto} | " +
                    $"Tecnologia = {voto.Interesse} | " +
                    "Evento computado com sucesso!");
                dataProcessed = true;
            }

            if (!dataProcessed)
            {
                _logger.LogError(
                    $"Formato dos dados do evento invalido: {JsonSerializer.Serialize(voto)}");
            }           
        }
    }
}