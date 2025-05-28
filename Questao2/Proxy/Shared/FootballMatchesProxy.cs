using Questao2.Response;
using System.Text.Json;

namespace Questao2.Proxy.Shared;
public class FootballMatchesProxy
{

    private readonly HttpClient _httpClient;

    public FootballMatchesProxy()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(Constants.urlApi)
        };
    }

    public int GetTotalScoredGoals(string team, int year, ITeamScoredGoals teamScoredGoals)
    {
        var page = 1;
        var TotalPage = 1;
        var totalGols = 0;
        do
        {

            var url = teamScoredGoals.GetUrlApi(team, year, page);

            var httpRequestMessage = CreateRequest(HttpMethod.Get, url);

            var response = _httpClient.Send(httpRequestMessage);

            ValidateResponse(response);

            var footballMatches = JsonSerializer.Deserialize<FootballMatchesRespose>(response.Content.ReadAsStream());

            if (footballMatches == null || footballMatches.data == null || footballMatches.data.Count == 0)
                return totalGols;

            totalGols = totalGols + footballMatches.data
                .Sum(x => teamScoredGoals.GetTotalGoals(x));

            page++;
            TotalPage = footballMatches.total_pages;

        } while (page <= TotalPage);

        return totalGols;
    }


    //private FootballMatchesRespose? GetFootballMatchesTeam1(string team, int year, int page)
    //{
    //    var httpRequestMessage = CreateRequest(HttpMethod.Get, $"{Constants.UrlApi}?year={year}&team1={team}&page={page}");

    //    return GetResponseFootballMatches(httpRequestMessage);
    //}

    //private FootballMatchesRespose? GetFootballMatchesTeam2(string team, int year, int page)
    //{
    //    var httpRequestMessage = CreateRequest(HttpMethod.Get, $"{Constants.UrlApi}?year={year}&team2={team}&page={page}");
    //    return GetResponseFootballMatches(httpRequestMessage);
    //}

    //private FootballMatchesRespose? GetResponseFootballMatches(HttpRequestMessage httpRequestMessage)
    //{
    //    var response = _httpClient.Send(httpRequestMessage);

    //    ValidateResponse(response);

    //    var footballMatches = JsonSerializer.Deserialize<FootballMatchesRespose>(response.Content.ReadAsStream());

    //    return footballMatches;
    //}

    private HttpRequestMessage CreateRequest(HttpMethod httpMethod, string url)
    {
        var httpRequestMessage = new HttpRequestMessage(httpMethod, url)
        {
            Headers = {
                { "ContentType", "application/json" }
            }
        };

        return httpRequestMessage;
    }

    private void ValidateResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }
    }
}
