using System.Data;
using System.Security.Principal;
using AutoMapper;
using SAP.Core.Business;
using SAP.Model.Roles;
using SAP.Model.TypeBusiness;
using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Entities;

namespace SAP.RuleEngine.TypeBusinessService
{
    public class RolesBusinessService : IRolesBusinessService
    {
        IMapper mapper;
        SAPContext Context;

        public RolesBusinessService(SAPContext context, IPrincipal userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Role, RolesResult>();
            });
            mapper = new Mapper(config);
            this.Context = context;
        }

        public Result<RolesResult> GetById(GetRolesDto dto)
        {
            var data = Context.Roles.Where(x => x.Id == dto.Id).FirstOrDefault();
            return Result<RolesResult>.SetOk(mapper.Map<RolesResult>(data));
        }

        public Result<List<RolesResult>> GetAll()
        {
            var data = Context.Roles.ToList();
            return data.Any() ? Result<List<RolesResult>>.SetOk(mapper.Map<List<RolesResult>>(data)) :
                Result<List<RolesResult>>.SetError("No se encontraron Registros");
        }

        public Result<string> Update(RolesDto dto)
        {
            try
            {
                var data = Context.Roles.Where(x => x.Id == dto.Id).FirstOrDefault();
                if (data == null) return Result<string>.SetError("Doesnt Exist Type");

                data.Id = dto.Id;
                data.Name = dto.Name?.Trim().ToUpper();
                data.NormalizedName = dto.Name?.Trim().ToUpper();
                data.DateModification = DateTime.UtcNow;
                data.IsDeleted = dto.IsDeleted;
                Context.Roles.Add(data);
                Context.SaveChanges();
                return Result<string>.SetOk("Update Success");
            }
            catch (Exception ex)
            {
                return Result<string>.SetError($"{ex.Message}");
            }
        }

        public Result<string> Create(RolesDto dto)
        {
            try
            {
                var role = new Role
                {
                    Name = dto.Name?.Trim().ToUpper(),
                    NormalizedName = dto.Name?.Trim().ToUpper(),
                    IsDeleted = false,
                    DateCreation = DateTime.UtcNow,
                };
                Context.Roles.Add(role);
                Context.SaveChanges();
                return Result<string>.SetOk("Create Success"); ;
            }
            catch (Exception ex)
            {
                return Result<string>.SetError($"{ex.Message}");
            }
        }
    }
}
