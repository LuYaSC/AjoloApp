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
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
                cfg.CreateMap<CreateAssignedTutorDto, AssignedTutor>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdateAssignedTutorDto, AssignedTutor>().AfterMap<TrimAllStringProperty>();
            });
            mapper = new Mapper(config);
        }

        public Result<List<AssignationTutorResult>> GetAll()
        {
            var assingations = ListComplete<AssignedTutor>().Include(x => x.Kid).Include(x => x.Parent).Include(x => x.Relationship).ToList();
            return assingations.Any() ? Result<List<AssignationTutorResult>>.SetOk(mapper.Map<List<AssignationTutorResult>>(assingations))
                                      : Result<List<AssignationTutorResult>>.SetError("Doesnt Exist Data");
        }

        public Result<string> Create(List<CreateAssignedTutorDto> dto)
        {
            try
            {
                dto.ForEach(x => Context.Save(mapper.Map<AssignedTutor>(x)));
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("Not Saved");
            }
            return Result<string>.SetOk("Saved with success");
        }

        public Result<string> Update(List<UpdateAssignedTutorDto> dto)
        {
            foreach (var list in dto)
            {
                var row = GetById<AssignedTutor>(list.Id);
                if (row != null)
                {
                    row = mapper.Map<AssignedTutor>(row);
                    Context.Save(row);
                }
            }
            return Result<string>.SetOk("Success");
        }
    }
}
