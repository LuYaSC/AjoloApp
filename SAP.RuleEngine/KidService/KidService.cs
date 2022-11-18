using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SAP.Core.Business;
using SAP.Model.Kid;
using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Entities;

namespace SAP.RuleEngine.KidService
{
    public class KidService : BaseBusiness<Kid, SAPContext>, IKidService
    {
        IMapper mapper;

        public KidService(SAPContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Kid, KidsResult>()
                   .ForMember(d => d.Age, o => o.MapFrom(s => CalculateAge(s.BornDate.Value)))
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
                cfg.CreateMap<CreateKidDto, Kid>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdateKidDto, Kid>().AfterMap<TrimAllStringProperty>();
            });
            mapper = new Mapper(config);
        }

        private string CalculateAge(DateTime bornDate)
        {
            TimeSpan dateDiff = DateTime.Now - bornDate;
            DateTime age = new DateTime(dateDiff.Ticks);

            var Years = age.Year - 1;
            var Month = age.Month - 1;
            var Days = age.Day - 1;

            return $"{Years} años{(Month > 0 ? $", {Month} meses" : "")} {(Days > 0 ? $"y {Days} dias" : "")}";
        }

        public Result<KidsResult> GetKidById(KidByIdDto dto)
        {
            return Result<KidsResult>.SetOk(mapper.Map<KidsResult>(GetById<Kid>(dto.Id)));
        }

        public Result<KidsResult> GetKid(GetKidDto dto)
        {
            var kid = GetComplete<Kid>(dto.Name, dto.FirstLastName, dto.SecondLastName, dto.SexTypeId);
            if(dto.BornDate != null) kid = kid.Where(x => x.BornDate == dto.BornDate);
            if(dto.StartDate != null) kid = kid.Where(x => x.StartDate == dto.StartDate);
            return kid.FirstOrDefault() == null ? Result<KidsResult>.SetError("Doesnt Exists") : Result<KidsResult>.SetOk(mapper.Map<KidsResult>(kid.First()));
        }

        public Result<List<KidsResult>> GetAllKids()
        {
            return Result<List<KidsResult>>.SetOk(mapper.Map<List<KidsResult>>(ListComplete<Kid>().Include(x => x.DocumentType)));
        }

        public Result<string> CreateKid(CreateKidDto dto)
        {
            try
            {
                var kid = GetComplete<Kid>(dto.Name, dto.FirstLastName, dto.SecondLastName, 0).FirstOrDefault();
                if(kid != null) return Result<string>.SetError("kid exists");
                Context.Save(mapper.Map<Kid>(dto));
                return Result<string>.SetOk("Kid Create with Success");
            }
            catch(Exception ex)
            {
                return Result<string>.SetError($"Doesnt kid create {ex.Message}");
            }
        }

        public Result<string> UpdateKid(UpdateKidDto dto)
        {
            try
            {
                var kid = Get(dto.Id);
                if(kid == null) return Result<string>.SetError("Doesnt exists kid");
                Context.Save(mapper.Map<Kid>(kid));
                return Result<string>.SetOk("Kid Create with Success");
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("Doesnt kid create");
            }
        }
    }
}
