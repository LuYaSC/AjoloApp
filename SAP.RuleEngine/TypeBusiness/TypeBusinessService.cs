using System.Security.Principal;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SAP.Core.Business;
using SAP.Model.TypeBusiness;
using SAP.Repository.Base;
using SAP.Repository.SAPRepository;

namespace SAP.RuleEngine.TypeBusinessService
{
    public class TypeBusinessService<T> : BaseBusiness<T, SAPContext>, ITypeBusinessService<T>
        where T : BaseType, new()
    {
        IMapper mapper;

        public TypeBusinessService(SAPContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, GetTypeResult>()
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
            });
            mapper = new Mapper(config);
        }

        public Result<GetTypeResult> GetById(GetTypeByIdDto dto)
        {
            var data = Context.Set<T>().Include(x => x.UserCreated).Include(x => x.UserModificated).Where(x => x.Id == dto.Id).FirstOrDefault();
            return Result<GetTypeResult>.SetOk(mapper.Map<GetTypeResult>(data));
        }

        public Result<List<GetTypeResult>> GetAll()
        {
            var data = Context.Set<T>().Include(x => x.UserCreated).Include(x => x.UserModificated).ToList();
<<<<<<< HEAD
            return data.Any() ?  Result<List<GetTypeResult>>.SetOk(mapper.Map<List<GetTypeResult>>(data)):
              Result<List<GetTypeResult>>.SetError("No se encontraron Registros");
=======
            return data.Any() ? Result<List<GetTypeResult>>.SetOk(mapper.Map<List<GetTypeResult>>(data)) :
                Result<List<GetTypeResult>>.SetError("No se encontraros resultados");
>>>>>>> 59409d67ca9ef9e7187a84d39e288dd8b053df4a
        }

        public Result<string> Update(GetTypeByIdDto dto)
        {
            try
            {
                var data = Get(dto.Id);
                if (data == null) return Result<string>.SetError("Doesnt Exist Type");

                data.Description = dto.Description.Trim().ToUpper();
                data.Initial = dto.Initial.Trim().ToUpper();
                data.IsDeleted = dto.IsDisabled;
                Save(data);
                return Result<string>.SetOk("Update Success");
            }
            catch (Exception ex)
            {
                return Result<string>.SetError($"{ex.Message}");
            }
        }

        public Result<string> Create(CreateTypeDto dto)
        {
            try
            {
                Save(new T
                {
                    Description = dto.Description.Trim().ToUpper(),
                    Initial = dto.Initial.Trim().ToUpper(),
                    IsDeleted = false
                });
                return Result<string>.SetOk("Create Success"); ;
            }
            catch (Exception ex)
            {
                return Result<string>.SetError($"{ex.Message}");
            }
        }

    }
}
