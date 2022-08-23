using FunctionAppApuracaoPesquisa.EventHubs;

namespace FunctionAppApuracaoPesquisa.Data;

public interface IVotacaoRepository
{
    void Save(VotoEventData voto);
}