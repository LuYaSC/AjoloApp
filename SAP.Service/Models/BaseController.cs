using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Entities;
using System.Net.Http;

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

        public async void SaveRequest(dynamic request = null)
        {
            try
            {
                // Obtener el contexto HTTP
                var httpContext = _httpContextAccessor.HttpContext;

                // Obtener los detalles de la solicitud
                var requestMethod = httpContext.Request.Method;
                var requestUrl = httpContext.Request.Path;
                var requestBody = JsonConvert.SerializeObject(request);
                var requestHeaders = JsonConvert.SerializeObject(httpContext.Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString()));


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
                        //ResponseStatusCode = responseStatusCode,
                        //ResponseBody = responseBody,
                        //ResponseHeaders = responseHeaders
                    };

                    dbContext.ManageLogs.Add(httpLog);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex) { }
        }
    }
}
