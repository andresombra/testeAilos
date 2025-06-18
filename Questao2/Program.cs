using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Questao2.Model;

public class Program
{
    private static readonly HttpClient client = new HttpClient();
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year); // Await the async method

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year); // Await the async method

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint-Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        int currentPage = 1;
        int totalPages = 1;

        do
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={Uri.EscapeDataString(team)}&page={currentPage}";

            try
            {
                string response = await client.GetStringAsync(url);
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response); 

                if (apiResponse != null) // Ensure apiResponse is not null
                {
                    totalPages = apiResponse.total_pages;

                    foreach (var match in apiResponse.data)
                    {
                        if (int.TryParse(match.team2goals, out int goals))
                        {
                            totalGoals += goals;
                        }
                    }
                }

                currentPage++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ao consultar dados do team2: {ex.Message}");
                break;
            }
        }
        while (currentPage <= totalPages);

        return totalGoals;
    }    
}