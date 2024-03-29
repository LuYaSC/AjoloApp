﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AjoloApp.Core.Business;
using AjoloApp.Model.Roles;
using AjoloApp.RuleEngine.TypeBusinessService;
using AjoloApp.Service.Models;

namespace AjoloApp.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class RolesController : BaseController
    {
        IRolesBusinessService service;

        public RolesController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IRolesBusinessService service)
            : base(httpContextAccessor, configuration)
        {
            this.service = service;
        }

        [HttpPost]
        public Result<RolesResult> GetById([FromBody] GetRolesDto dto)
        {
            var result = service.GetById(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpGet]
        public Result<List<RolesResult>> GetAll()
        {
            var result = service.GetAll();
            SaveRequest(response: result);
            return result;
        }

        [HttpPost]
        public Result<string> Update([FromBody] RolesDto dto)
        {
            var result = service.Update(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> Create([FromBody] RolesDto dto)
        {
            var result = service.Create(dto);
            SaveRequest(dto, result);
            return result;
        }
    }
}
