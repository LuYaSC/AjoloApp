﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AjoloApp.Core.Business;
using AjoloApp.Model.TypeBusiness;
using AjoloApp.Repository.AjoloAppRepository.Entities;
using AjoloApp.RuleEngine.TypeBusinessService;
using AjoloApp.Service.Models;

namespace AjoloApp.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class MaritalStatusController : BaseController
    {
        ITypeBusinessService<MaritalStatus> service;
        
        public MaritalStatusController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
                    ITypeBusinessService<MaritalStatus> service) : base(httpContextAccessor, configuration)
        {
            this.service = service;
        }

        [HttpPost]
        public Result<GetTypeResult> GetById([FromBody] GetTypeByIdDto dto)
        {
            var result = service.GetById(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpGet]
        public Result<List<GetTypeResult>> GetAll()
        {
            var result = service.GetAll();
            SaveRequest(response: result);
            return result;
        }

        [HttpPost]
        public Result<string> Update([FromBody] GetTypeByIdDto dto)
        {
            var result = service.Update(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpPost]
        public Result<string> Create([FromBody] CreateTypeDto dto)
        {
            var result = service.Create(dto);
            SaveRequest(dto, result);
            return result;
        }

        [HttpGet]
        public Result<ReportResult> GenerateReport()
        {
            var result = service.GeneratePdf("Lista de Estado Civil Registrados");
            SaveRequest(response: result);
            return result;
        }
    }
}