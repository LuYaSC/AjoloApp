using AutoMapper;
using SAP.Core.Business;
using SAP.Model.AssignationRoom;
using SAP.Repository.SAPRepository.Entities;
using SAP.Repository.SAPRepository;
using SAP.RuleEngine.AssignationRoomService;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using SAP.Model.AssignationTutor;

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
                   .ForMember(d => d.City, o => o.MapFrom(s => s.City.Description))
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
                cfg.CreateMap<CreateAssignedRoomDto, AssignedRoom>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdateAssignedRoomDto, AssignedRoom>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<AssignedRoom, AssignationRoomDetailResult>()
                    .ForMember(d => d.Collaborator, o => o.MapFrom(s => $"{s.Collaborator.Name} {s.Collaborator.FirstLastName} {s.Collaborator.SecondLastName}"))
                    .ForMember(d => d.CollaboratorSex, o => o.MapFrom(s => s.Collaborator.SexType.Description))
                    .ForMember(d => d.CollaboratorCity, o => o.MapFrom(s => s.Collaborator.User.UserDetail.City.Description))
                    .ForMember(d => d.CollaboratorBranchOffice, o => o.MapFrom(s => s.Collaborator.User.UserDetail.BranchOffice.Description))
                    .ForMember(d => d.CollaboratorEmail, o => o.MapFrom(s => s.Collaborator.User.Email))
                    .ForMember(d => d.CollaboratorStartDate, o => o.MapFrom(s => s.Collaborator.StartDate))
                    .ForMember(d => d.Room, o => o.MapFrom(s => s.Room.Description))
                    .ForMember(d => d.City, o => o.MapFrom(s => s.City.Description))
                    .ForMember(d => d.BranchOffice, o => o.MapFrom(s => s.BranchOffice.Description))
                    .ForMember(d => d.Modality, o => o.MapFrom(s => s.Modality.Description));
            });
            mapper = new Mapper(config);
        }

        public Result<List<AssignationRoomResult>> GetFilter(CreateAssignedRoomDto dto)
        {
            var assignationsFilter = ListComplete<AssignedRoom>()
                .Include(x => x.Collaborator).Include(x => x.Room).Include(x => x.Turn)
                .Include(x => x.Modality).Include(x => x.BranchOffice).Include(x => x.City)
                .Where(x => dto.CollaboratorId != 0 ? x.CollaboratorId == dto.CollaboratorId : true)
                .Where(x => dto.RoomId != 0 ? x.RoomId == dto.RoomId : true)
                .Where(x => dto.TurnId != 0 ? x.TurnId == dto.TurnId : true)
                .Where(x => dto.ModalityId != 0 ? x.ModalityId == dto.ModalityId : true)
                .Where(x => dto.BranchOfficeId != 0 ? x.BranchOfficeId == dto.BranchOfficeId : true)
                .Where(x => dto.CityId != 0 ? x.CityId == dto.CityId : true)
                .OrderBy(x => x.DateCreation).OrderBy(x => x.CollaboratorId).ToList();
            return assignationsFilter.Any() ? Result<List<AssignationRoomResult>>.SetOk(mapper.Map<List<AssignationRoomResult>>(assignationsFilter))
                                    : Result<List<AssignationRoomResult>>.SetError("Doesnt Exist Data");
        }

        public Result<List<AssignationRoomResult>> GetAll()
        {
            var assingations = ListComplete<AssignedRoom>()
                .Include(x => x.Collaborator).Include(x => x.Room).Include(x => x.Turn).Include(x => x.City)
                .Include(x => x.Modality).Include(x => x.BranchOffice).OrderBy(x => x.DateCreation).OrderBy(x => x.CollaboratorId).ToList();
            return assingations.Any() ? Result<List<AssignationRoomResult>>.SetOk(mapper.Map<List<AssignationRoomResult>>(assingations))
                                      : Result<List<AssignationRoomResult>>.SetError("Doesnt Exist Data");
        }

        public Result<AssignationRoomDetailResult> GetDetail(AssignationRoomDetailDto dto)
        {
            var data = Context.AssignedRooms.Where(x => x.Id == dto.Id)
                .Include(x => x.Collaborator).Include(x => x.Room).Include(x => x.Turn).Include(x => x.Modality)
                .Include(x => x.BranchOffice).Include(x => x.City).Include(x => x.Collaborator.SexType)
                .Include(x => x.Collaborator.User)
                .Include(x => x.Collaborator.User.UserDetail.BranchOffice)
                .Include(x => x.Collaborator.User.UserDetail.City).FirstOrDefault();
            
            return data != null ? Result<AssignationRoomDetailResult>.SetOk(mapper.Map<AssignationRoomDetailResult>(data))
                : Result<AssignationRoomDetailResult>.SetError("Asignacion inexistente verifique porfavor");
        }
       

        public Result<string> Create(CreateAssignedRoomDto dto)
        {
            try
            {
                var existsData = List();
                if (existsData.Where(x => x.CollaboratorId == dto.CollaboratorId && x.RoomId == dto.RoomId &&
                x.TurnId == dto.TurnId && x.ModalityId == dto.ModalityId && x.BranchOfficeId == dto.BranchOfficeId).FirstOrDefault() == null)
                {
                    Context.Save(mapper.Map<AssignedRoom>(dto));
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

        public Result<string> Update(UpdateAssignedRoomDto dto)
        {
            var row = GetById<AssignedRoom>(dto.Id);
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

        public Result<string> DisableOrEnable(UpdateAssignedRoomDto dto)
        {
            var row = GetById<AssignedRoom>(dto.Id);
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
