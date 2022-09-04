using AutoMapper;
using SAP.Core.Business;
using SAP.Model.EnrolledChildren;
using SAP.Repository.SAPRepository.Entities;
using SAP.Repository.SAPRepository;
using SAP.RuleEngine.EnrolledChildrenService;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;

namespace SAP.RuleEngine.EnrolledChildrenService
{
    public class EnrolledChildrenService : BaseBusiness<EnrolledChildren, SAPContext>, IEnrolledChildrenService
    {
        IMapper mapper;

        public EnrolledChildrenService(SAPContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EnrolledChildren, EnrolledChildrenResult>()
                   .ForMember(d => d.Collaborator, o => o.MapFrom(s => $"{s.Collaborator.Name} {s.Collaborator.FirstLastName} {s.Collaborator.SecondLastName}"))
                   .ForMember(d => d.Room, o => o.MapFrom(s => s.Room.Description))
                   .ForMember(d => d.Turn, o => o.MapFrom(s => s.Turn.Description))
                   .ForMember(d => d.Modality, o => o.MapFrom(s => s.Modality.Description))
                   .ForMember(d => d.BranchOffice, o => o.MapFrom(s => s.BranchOffice.Description))
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
                cfg.CreateMap<CreateEnrolledChildrenDto, EnrolledChildren>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdateEnrolledChildrenDto, EnrolledChildren>().AfterMap<TrimAllStringProperty>();
            });
            mapper = new Mapper(config);
        }

        public Result<List<EnrolledChildrenResult>> GetAll()
        {
            var assingations = ListComplete<EnrolledChildren>().Include(x => x.AssignedTutor).Include(x => x.AssignedRoom).Include(x => x.AssignedTutor.Kid)
                                                               .Include(x => x.AssignedTutor.Parent).Include(x => x.AssignedTutor.Relationship).ToList();
            return assingations.Any() ? Result<List<EnrolledChildrenResult>>.SetOk(mapper.Map<List<EnrolledChildrenResult>>(assingations))
                                      : Result<List<EnrolledChildrenResult>>.SetError("Doesnt Exist Data");
        }

        public Result<string> Create(List<CreateEnrolledChildrenDto> dto)
        {
            try
            {
                dto.ForEach(x => Context.Save(mapper.Map<EnrolledChildren>(x)));
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("Not Saved");
            }
            return Result<string>.SetOk("Saved with success");
        }

        public Result<string> Update(List<UpdateEnrolledChildrenDto> dto)
        {
            foreach (var list in dto)
            {
                var row = GetById<EnrolledChildren>(list.Id);
                if (row != null)
                {
                    row = mapper.Map<EnrolledChildren>(row);
                    Context.Save(row);
                }
            }
            return Result<string>.SetOk("Success");
        }
    }
}
