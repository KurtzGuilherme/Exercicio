﻿namespace Questao2.Response;
public sealed class FootballMatchesRespose
{
    public int page { get; set; }
    public int per_page { get; set; }
    public int total { get; set; }
    public int total_pages { get; set; }
    public List<FootballMatchesDataResponse> data { get; set; }
}
