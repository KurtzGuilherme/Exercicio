using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore.Interface;

public interface IMovimentoCommand
{
    Task PostMovimentoContaCorrente(Movimento movimento);
}
