using Questao2.Response;

namespace Questao2.Proxy;
public class Team1ScoredGoals : ITeamScoredGoals
{
    public int GetTotalGoals(FootballMatchesDataResponse footballMatchesDataResponse)
        => int.Parse(footballMatchesDataResponse.team1goals);

    public string GetUrlApi(string team, int year, int page)
        => $"{Constants.urlApi}?year={year}&team1={team}&page={page}";
}
