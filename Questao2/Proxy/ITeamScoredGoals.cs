using Questao2.Response;

namespace Questao2.Proxy;
public interface ITeamScoredGoals
{
    string GetUrlApi(string team, int year, int page);

    int GetTotalGoals(FootballMatchesDataResponse footballMatchesDataResponse);
}
