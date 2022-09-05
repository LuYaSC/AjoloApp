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
                   .ForMember(d => d.Parent, o => o.MapFrom(s => $"{s.AssignedTutor.Parent.Name} {s.AssignedTutor.Parent.FirstLastName} " +
                                                                 $"{s.AssignedTutor.Parent.SecondLastName}"))
                   .ForMember(d => d.Kid, o => o.MapFrom(s => $"{s.AssignedTutor.Kid.Name} {s.AssignedTutor.Kid.FirstLastName} " +
                                                              $"{s.AssignedTutor.Kid.SecondLastName}"))
                   .ForMember(d => d.Collaborator, o => o.MapFrom(s => $"{s.AssignedRoom.Collaborator.Name} {s.AssignedRoom.Collaborator.FirstLastName} " +
                                                                       $"{s.AssignedRoom.Collaborator.SecondLastName}"))
                   .ForMember(d => d.Room, o => o.MapFrom(s => s.AssignedRoom.Room.Description))
                   .ForMember(d => d.Turn, o => o.MapFrom(s => s.AssignedRoom.Turn.Description))
                   .ForMember(d => d.Modality, o => o.MapFrom(s => s.AssignedRoom.Modality.Description))
                   .ForMember(d => d.BranchOffice, o => o.MapFrom(s => s.AssignedRoom.BranchOffice.Description))
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
                                                               .Include(x => x.AssignedTutor.Parent).Include(x => x.AssignedRoom.Collaborator)
                                                               .Include(x => x.AssignedRoom.Room).Include(x => x.AssignedRoom.Turn)
                                                               .Include(x => x.AssignedRoom.Modality).Include(x => x.AssignedRoom.BranchOffice).ToList();
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
