
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Entities;

namespace SAP.Service.Models
{
    public class BaseController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration configuration;

        public BaseController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            this.configuration = configuration;
        }

        protected async void SaveRequest([FromQueryAttribute] dynamic request = null, dynamic response = null)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var allowMethods = configuration.GetSection("PeticionLogs").Get<string[]>();
                var requestMethod = httpContext.Request.Method;
                if (allowMethods.Contains(requestMethod))
                {
                    var requestUrl = httpContext.Request.Path;
                    var requestBody = JsonConvert.SerializeObject(request);
                    var requestHeaders = JsonConvert.SerializeObject(httpContext.Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString()));

                    var responseBody = response != null ? JsonConvert.SerializeObject(response) : null;

                    using (var dbContext = new SAPContext(new DbContextOptionsBuilder<SAPContext>()
                            .UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                             .Options))
                    {
                        var httpLog = new ManageLog
                        {
                            RequestTime = DateTime.UtcNow,
                            RequestMethod = requestMethod,
                            RequestUrl = requestUrl,
                            RequestBody = requestBody,
                            RequestHeaders = requestHeaders,
                            ResponseBody = responseBody,
                        };

                        dbContext.ManageLogs.Add(httpLog);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

    }
}
