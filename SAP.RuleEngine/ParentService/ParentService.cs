using AutoMapper;
using SAP.Core.Business;
using SAP.Model.Parent;
using SAP.Repository.SAPRepository.Entities;
using SAP.Repository.SAPRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SAP.RuleEngine.ParentService
{
    public class ParentService : BaseBusiness<Parent, SAPContext>, IParentService
    {
        public IMapper mapper;

        public ParentService(SAPContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Parent, ParentsResult>()
                    .ForMember(d => d.MaritalStatus, o => o.MapFrom(s => s.MaritalStatus.Description))
                    .ForMember(d => d.DocumentType, o => o.MapFrom(s => s.DocumentType.Description))
                    .ForMember(d => d.Sex, o => o.MapFrom(s => s.SexType.Description))
                    .ForMember(d => d.BloodType, o => o.MapFrom(s => s.BloodType.Description))
                    .ForMember(d => d.UserCreation, o => o.MapFrom(s => CutUser(s.UserCreated.UserName)))
                    .ForMember(d => d.UserModification, o => o.MapFrom(s => CutUser(s.UserModificated.UserName)));
                cfg.CreateMap<CreateParentDto, Parent>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdateParentDto, Parent>().AfterMap<TrimAllStringProperty>();
            });
            mapper = new Mapper(config);
        }

        public Result<ParentsResult> GetParentById(ParentByIdDto dto)
        {
            return Result<ParentsResult>.SetOk(mapper.Map<ParentsResult>(GetById<Parent>(dto.Id)));
        }

        public Result<ParentsResult> GetParent(GetParentDto dto)
        {
            var Parent = GetComplete<Parent>(dto.Name, dto.FirstLastName, dto.SecondLastName, dto.SexTypeId);
            return Parent.FirstOrDefault() == null ? Result<ParentsResult>.SetError("Doesnt Exists") : Result<ParentsResult>.SetOk(mapper.Map<ParentsResult>(Parent.First()));
        }

        public Result<List<ParentsResult>> GetAllParents()
        {
            return Result<List<ParentsResult>>.SetOk(mapper.Map<List<ParentsResult>>(ListComplete<Parent>().Include(x => x.MaritalStatus).Include(x => x.DocumentType)
                .Include(x => x.BloodType).Include(x => x.SexType)));
        }

        public Result<string> CreateParent(CreateParentDto dto)
        {
            try
            {
                var Parent = GetComplete<Parent>(dto.Name, dto.FirstLastName, dto.SecondLastName, 0).FirstOrDefault();
                if (Parent != null) return Result<string>.SetError("Parent exists");
                Context.Save(mapper.Map<Parent>(dto));
                return Result<string>.SetOk("Parent Create with Success");
            }
            catch (Exception ex)
            {
                return Result<string>.SetError($"Doesnt Parent create {ex.Message}");
            }
        }

        public Result<string> UpdateParent(UpdateParentDto dto)
        {
            try
            {
                var Parent = Get(dto.Id);
                if (Parent == null) return Result<string>.SetError("Doesnt exists Parent");
                Context.Save(mapper.Map(dto, Parent));
                return Result<string>.SetOk("Parent Create with Success");
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("Doesnt Parent create");
            }
        }
    }
}
