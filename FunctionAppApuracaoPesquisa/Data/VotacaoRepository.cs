using Microsoft.Data.SqlClient;
using Dapper.Contrib.Extensions;
using FunctionAppApuracaoPesquisa.EventHubs;

namespace FunctionAppApuracaoPesquisa.Data;

public class VotacaoRepository: IVotacaoRepository
{
    public void Save(VotoEventData voto)
    {
        using var conexao = new SqlConnection(
            Environment.GetEnvironmentVariable("BaseVotacao"));
        conexao.Insert<VotoTecnologia>(new()
        {
            DataProcessamento = DateTime.UtcNow.AddHours(-3),
            EventHub = AzureEventHubsConfigurations.EventHubName,
            Producer = voto.Producer,
            Consumer = Environment.MachineName,
            ConsumerGroup = AzureEventHubsConfigurations.ConsumerGroup,
            HorarioVoto = voto.Horario,
            IdVoto = voto.IdVoto,
            Interesse = voto.Interesse
        });
    }
}