using System.Data;
using System.Security.Principal;
using AutoMapper;
using AjoloApp.Core.Business;
using AjoloApp.Model.Roles;
using AjoloApp.Model.TypeBusiness;
using AjoloApp.Repository.AjoloAppRepository;
using AjoloApp.Repository.AjoloAppRepository.Entities;

namespace AjoloApp.RuleEngine.TypeBusinessService
{
    public class RolesBusinessService : IRolesBusinessService
    {
        IMapper mapper;
        AjoloAppContext Context;

        public RolesBusinessService(AjoloAppContext context, IPrincipal userInfo)
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
