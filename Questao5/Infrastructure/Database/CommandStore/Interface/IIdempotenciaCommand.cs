namespace Questao5.Infrastructure.Database.CommandStore.Interface;

public interface IIdempotenciaCommand
{
    Task Registrar(string idempotenciaId, string requisicao, string resultado);
}
