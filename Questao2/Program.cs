using Newtonsoft.Json;
using Questao2.Proxy;
using Questao2.Proxy.Shared;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        var proxy = new FootballMatchesProxy();
        int totalGoalsTeam1 = proxy.GetTotalScoredGoals(team, year, new Team1ScoredGoals());
        int totalGoalsTeam2 = proxy.GetTotalScoredGoals(team, year, new Team2ScoredGoals());

        return totalGoalsTeam1 + totalGoalsTeam2;
    }

}