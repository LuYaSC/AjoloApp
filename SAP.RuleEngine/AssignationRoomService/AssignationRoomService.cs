using AutoMapper;
using SAP.Core.Business;
using SAP.Model.AssignationRoom;
using SAP.Repository.SAPRepository.Entities;
using SAP.Repository.SAPRepository;
using SAP.RuleEngine.AssignationRoomService;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;

namespace SAP.RuleEngine.AssignationRoomService
{
    public class AssignationRoomService : BaseBusiness<AssignedRoom, SAPContext>, IAssignationRoomService
    {
        IMapper mapper;

        public AssignationRoomService(SAPContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssignedRoom, AssignationRoomResult>()
                   .ForMember(d => d.Collaborator, o => o.MapFrom(s => $"{s.Collaborator.Name} {s.Collaborator.FirstLastName} {s.Collaborator.SecondLastName}"))
                   .ForMember(d => d.Room, o => o.MapFrom(s => s.Room.Description))
                   .ForMember(d => d.Turn, o => o.MapFrom(s => s.Turn.Description))
                   .ForMember(d => d.Modality, o => o.MapFrom(s => s.Modality.Description))
                   .ForMember(d => d.BranchOffice, o => o.MapFrom(s => s.BranchOffice.Description))
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
                cfg.CreateMap<CreateAssignedRoomDto, AssignedRoom>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdateAssignedRoomDto, AssignedRoom>().AfterMap<TrimAllStringProperty>();
            });
            mapper = new Mapper(config);
        }

        public Result<List<AssignationRoomResult>> GetAll()
        {
            var assingations = ListComplete<AssignedRoom>().Include(x => x.Collaborator).Include(x => x.Room).Include(x => x.Turn)
                                                           .Include(x => x.Modality).Include(x => x.BranchOffice).ToList();
            return assingations.Any() ? Result<List<AssignationRoomResult>>.SetOk(mapper.Map<List<AssignationRoomResult>>(assingations))
                                      : Result<List<AssignationRoomResult>>.SetError("Doesnt Exist Data");
        }

        public Result<string> Create(List<CreateAssignedRoomDto> dto)
        {
            try
            {
                dto.ForEach(x => Context.Save(mapper.Map<AssignedRoom>(x)));
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("Not Saved");
            }
            return Result<string>.SetOk("Saved with success");
        }

        public Result<string> Update(List<UpdateAssignedRoomDto> dto)
        {
            foreach (var list in dto)
            {
                var row = GetById<AssignedRoom>(list.Id);
                if (row != null)
                {
                    row = mapper.Map<AssignedRoom>(row);
                    Context.Save(row);
                }
            }
            return Result<string>.SetOk("Success");
        }
    }
}
