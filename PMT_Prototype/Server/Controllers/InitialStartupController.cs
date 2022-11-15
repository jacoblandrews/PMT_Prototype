using Microsoft.AspNetCore.Mvc;
using PMT_Prototype.Shared;

namespace PMT_Prototype.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InitialStartupController : Controller
    {
        [HttpGet("companies")]
        public async Task<List<string>> GetCompaniesAsync(CancellationToken ct)
        {
            return (await DatabaseWrapper.RunQueryAsync<CompanyNameModel>("exec SSP_GetCompanyNames_v3 ''", DatabaseWrapper.DatabaseEnvironment.Test, ct)).Select(x=>x.CompanyName).ToList();
        }

    }
}
