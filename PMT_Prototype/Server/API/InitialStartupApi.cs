using Microsoft.AspNetCore.Http.HttpResults;
using PMT_Prototype.Shared;

namespace PMT_Prototype.Server.API
{
    internal static class InitialStartupApi
    {
        internal static RouteGroupBuilder MapInitialStartup(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/InitialStartup");
            group.WithTags("Startup");

            group.RequirePerUserRateLimit();

            group.MapGet("/companies", async Task<Results<Ok<List<string>>, NotFound>> () => 
            {
                var companies = await DatabaseWrapper.RunQueryAsync<CompanyNameModel>("exec SSP_GetCompanyNames_v3 ''", DatabaseWrapper.DatabaseEnvironment.Test) ?? new List<CompanyNameModel>();
                if (companies.Any())
                {
                    var results = companies.Select(x => x.CompanyName ?? string.Empty).ToList();
                    return TypedResults.Ok(results);
                }

                return TypedResults.NotFound();
            });

            group.MapGet("/folders", Results<Ok<List<string>>, NotFound> () =>
            {
                return TypedResults.Ok(new List<string>() { "All", "Farm Bureau", "Broker Assumed Domestic", "Broker Assumed International", "Reinsurers", "Corporate", "Misc. Analyses" });
            });

            group.MapGet("/years/{company}", async Task<Results<Ok<List<int>>, NotFound>> (string company) =>
            {
                var companies = await DatabaseWrapper.RunQueryAsync<CompanyNameModel>($"exec SSP_GetCompanyTreatyYears '{company}'", DatabaseWrapper.DatabaseEnvironment.Test) ?? new List<CompanyNameModel>();
                if (companies.Any())
                {
                    var stringYears = companies.Select(x => x.TreatyYear);
                    var intYears = new List<int>();

                    foreach (var year in stringYears)
                    {
                        if (int.TryParse(year, out var resYear))
                        {
                            intYears.Add(resYear);
                        }
                        else
                        {
                            var message = $"Year {year} parsed from table is invalid. Please contact a developer to resolve this issue.";
                            await Logger.Log(message, Logger.LogLevel.Error);
                        }
                    }

                    return TypedResults.Ok(intYears);
                }

                return TypedResults.NotFound();
            });

            return group;
        }
    }
}
