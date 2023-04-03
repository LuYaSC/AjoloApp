using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SAP.Core.Business;
using SAP.Model.AssignationTutor;
using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Entities;
using System.Security.Principal;

namespace SAP.RuleEngine.AssignationTutorService
{
    public class AssignationTutorService : BaseBusiness<AssignedTutor, SAPContext>, IAssignationTutorService
    {
        IMapper mapper;

        public AssignationTutorService(SAPContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssignedTutor, AssignationTutorResult>()
                   .ForMember(d => d.KidName, o => o.MapFrom(s => $"{s.Kid.Name} {s.Kid.FirstLastName} {s.Kid.SecondLastName}"))
                   .ForMember(d => d.ParentName, o => o.MapFrom(s => $"{s.Parent.Name} {s.Parent.FirstLastName} {s.Parent.SecondLastName}"))
                   .ForMember(d => d.Relation, o => o.MapFrom(s => s.Relationship.Description))
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => CutUser(s.UserCreated.UserName)))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => CutUser(s.UserModificated.UserName)));
                cfg.CreateMap<CreateAssignedTutorDto, AssignedTutor>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdateAssignedTutorDto, AssignedTutor>().AfterMap<TrimAllStringProperty>();
            });
            mapper = new Mapper(config);
        }

        public Result<List<AssignationTutorResult>> GetFilter(CreateAssignedTutorDto dto)
        {
            var assignationsFilter = ListComplete<AssignedTutor>().Include(x => x.Kid).Include(x => x.Parent).Include(x => x.Relationship)
                .Where(x => dto.KidId != 0 ? x.KidId == dto.KidId : true)
                .Where(x => dto.ParentId != 0 ? x.ParentId == dto.ParentId : true)
                .OrderBy(x => x.DateCreation).OrderBy(x => x.KidId).ToList();
            return assignationsFilter.Any() ? Result<List<AssignationTutorResult>>.SetOk(mapper.Map<List<AssignationTutorResult>>(assignationsFilter))
                                    : Result<List<AssignationTutorResult>>.SetError("Doesnt Exist Data");
        }

        public Result<List<AssignationTutorResult>> GetAll()
        {
            var assingations = ListComplete<AssignedTutor>().Include(x => x.Kid).Include(x => x.Parent)
                .Include(x => x.Relationship).OrderBy(x => x.DateCreation).OrderBy(x => x.KidId).ToList();
            return assingations.Any() ? Result<List<AssignationTutorResult>>.SetOk(mapper.Map<List<AssignationTutorResult>>(assingations))
                                      : Result<List<AssignationTutorResult>>.SetError("Doesnt Exist Data");
        }

        public Result<string> Create(CreateAssignedTutorDto dto)
        {
            try
            {
                var existsData = List();
                if (existsData.Where(x => x.KidId == dto.KidId && x.ParentId == dto.ParentId).FirstOrDefault() == null)
                {
                    Context.Save(mapper.Map<AssignedTutor>(dto));
                }
                else
                {
                    return Result<string>.SetError("Registro Existente");
                }
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("No fue posible Guardar Reintente porfavor");
            }
            return Result<string>.SetOk("Operacion Realizada con Exito");
        }

        public Result<string> Update(UpdateAssignedTutorDto dto)
        {
            var row = GetById<AssignedTutor>(dto.Id);
            if (row != null)
            {
                Context.Save(mapper.Map(dto, row));
            }
            else
            {
                Result<string>.SetOk("Registro no encontrado");
            }
            return Result<string>.SetOk("Success");
        }

        public Result<string> DisableOrEnable(UpdateAssignedTutorDto dto)
        {
            var row = GetById<AssignedTutor>(dto.Id);
            if (row != null)
            {
                row.IsDeleted = dto.IsDeleted;
                Save(row);
            }
            else
            {
                Result<string>.SetOk("Registro no encontrado");
            }
            return Result<string>.SetOk("Success");
        }

    }
}
