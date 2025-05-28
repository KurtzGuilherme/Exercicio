using Questao2.Response;

namespace Questao2.Proxy;
public class Team2ScoredGoals : ITeamScoredGoals
{
    public int GetTotalGoals(FootballMatchesDataResponse footballMatchesDataResponse)
        => int.Parse(footballMatchesDataResponse.team2goals);

    public string GetUrlApi(string team, int year, int page)
        => $"{Constants.urlApi}?year={year}&team2={team}&page={page}";
}
