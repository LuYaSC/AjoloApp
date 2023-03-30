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
                    .ForMember(d => d.Sex, o => o.MapFrom(s => s.SexType.Description))
                    .ForMember(d => d.BloodType, o => o.MapFrom(s => s.BloodType.Description))
                    .ForMember(d => d.DocumentType, o => o.MapFrom(s => s.DocumentType.Description))
                    .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                    .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
                cfg.CreateMap<CreateKidDto, Kid>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdateKidDto, Kid>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<AssignedTutor, GetDetailKidResult>()
                    .ForMember(d => d.AgeKid, o => o.MapFrom(s => CalculateAge(s.Kid.BornDate.Value)))
                    .ForMember(d => d.DocumentType, o => o.MapFrom(s => s.Kid.DocumentType.Description))
                    .ForMember(d => d.DocumentNumber, o => o.MapFrom(s => s.Kid.DocumentNumber))
                    .ForMember(d => d.ParentName, o => o.MapFrom(s => $"{s.Parent.Name} {s.Parent.FirstLastName} {s.Parent.SecondLastName}"))
                    .ForMember(d => d.Relation, o => o.MapFrom(s => s.Relationship.Description))
                    .ForMember(d => d.MaritalStatus, o => o.MapFrom(s => s.Parent.MaritalStatus.Description))
                    .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.Parent.PhoneNumber))
                    .ForMember(d => d.IsAuthorized, o => o.MapFrom(s => s.IsAuthorized));
            });
            mapper = new Mapper(config);
        }

        public Result<KidsResult> GetKidById(KidByIdDto dto)
        {
            return Result<KidsResult>.SetOk(mapper.Map<KidsResult>(GetById<Kid>(dto.Id)));
        }

        public Result<KidsResult> GetKid(GetKidDto dto)
        {
            var kid = GetComplete<Kid>(dto.Name, dto.FirstLastName, dto.SecondLastName, dto.SexTypeId);
            if (dto.BornDate != null) kid = kid.Where(x => x.BornDate == dto.BornDate);
            if (dto.StartDate != null) kid = kid.Where(x => x.StartDate == dto.StartDate);
            return kid.FirstOrDefault() == null ? Result<KidsResult>.SetError("Doesnt Exists") : Result<KidsResult>.SetOk(mapper.Map<KidsResult>(kid.First()));
        }

        public Result<List<KidsResult>> GetAllKids()
        {
            return Result<List<KidsResult>>.SetOk(mapper.Map<List<KidsResult>>(ListComplete<Kid>().Include(x => x.DocumentType).Include(x => x.SexType).Include(x => x.BloodType)));
        }

        public Result<List<GetDetailKidResult>> GetDetailKid(GetDetailKidDto dto)
        {
            var data = Context.AssignedTutors.Where(x => x.KidId == dto.KidId)
                .Include(x => x.Kid)
                .Include(x => x.Parent).Include(x => x.Parent.MaritalStatus).Include(x => x.Relationship).ToList();
            var result = mapper.Map<List<GetDetailKidResult>>(data);

            return Result<List<GetDetailKidResult>>.SetOk(result);
        }

        public Result<string> CreateKid(CreateKidDto dto)
        {
            try
            {
                var kid = GetComplete<Kid>(dto.Name, dto.FirstLastName, dto.SecondLastName, 0).FirstOrDefault();
                if (kid != null) return Result<string>.SetError("kid exists");
                Context.Save(mapper.Map<Kid>(dto));
                return Result<string>.SetOk("Kid Create with Success");
            }
            catch (Exception ex)
            {
                return Result<string>.SetError($"Doesnt kid create {ex.Message}");
            }
        }

        public Result<string> UpdateKid(UpdateKidDto dto)
        {
            try
            {
                var kid = Get(dto.Id);
                if (kid == null) return Result<string>.SetError("Doesnt exists kid");
                mapper.Map(
                      source: dto,
                      destination: kid,
                      sourceType: typeof(UpdateKidDto),
                      destinationType: typeof(Kid)
                );
                Context.Save(kid);
                return Result<string>.SetOk("Kid Create with Success");
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("Doesnt kid create");
            }
        }

        public Result<string> ActivateOrDeactivate(KidByIdDto dto)
        {
            var kid = Get(dto.Id);
            if (kid == null) return Result<string>.SetError("Doesnt exists kid");
            kid.IsDeleted = dto.IsDeleted;
            Context.Save(kid);
            return Result<string>.SetOk("Disabled with success");
        }
    }
}
