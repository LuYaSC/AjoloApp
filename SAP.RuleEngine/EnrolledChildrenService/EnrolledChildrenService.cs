using AutoMapper;
using SAP.Core.Business;
using SAP.Model.EnrolledChildren;
using SAP.Repository.SAPRepository.Entities;
using SAP.Repository.SAPRepository;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using SAP.Model.AssignationRoom;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using SAP.Model.Kid;

namespace SAP.RuleEngine.EnrolledChildrenService
{
    public class EnrolledChildrenService : BaseBusiness<EnrolledChildren, SAPContext>, IEnrolledChildrenService
    {
        IMapper mapper;
        int quantityTutors = 0;

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
                   .ForMember(d => d.KidId, o => o.MapFrom(s => s.AssignedTutor.KidId))
                   .ForMember(d => d.Room, o => o.MapFrom(s => s.AssignedRoom.Room.Description))
                   .ForMember(d => d.Turn, o => o.MapFrom(s => s.AssignedRoom.Turn.Description))
                   .ForMember(d => d.Modality, o => o.MapFrom(s => s.AssignedRoom.Modality.Description))
                   .ForMember(d => d.BranchOffice, o => o.MapFrom(s => s.AssignedRoom.BranchOffice.Description))
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                   .ForMember(d => d.AssignedRoomId, o => o.MapFrom(s => s.AssignedRoomId))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
                cfg.CreateMap<CreateEnrolledChildrenDto, EnrolledChildren>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdateEnrolledChildrenDto, EnrolledChildren>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<EnrolledChildren, EnrollChildrenDetailResult>()
                    //Kid Data
                    .ForMember(d => d.KidName, o => o.MapFrom(s => $"{s.AssignedTutor.Kid.Name} {s.AssignedTutor.Kid.FirstLastName} " +
                                                              $"{s.AssignedTutor.Kid.SecondLastName}"))
                    .ForMember(d => d.KidId, o => o.MapFrom(s => s.AssignedTutor.KidId))
                    .ForMember(d => d.KidId, o => o.MapFrom(s => s.AssignedRoomId))
                    .ForMember(d => d.BloodTypeKid, o => o.MapFrom(s => s.AssignedTutor.Kid.BloodType.Description))
                    .ForMember(d => d.SexKid, o => o.MapFrom(s => s.AssignedTutor.Kid.SexType.Description))
                    .ForMember(d => d.BornDateKid, o => o.MapFrom(s => s.AssignedTutor.Kid.BornDate))
                    .ForMember(d => d.AgeKid, o => o.MapFrom(s => CalculateAge(s.AssignedTutor.Kid.BornDate.Value)))
                    //Room Data
                    .ForMember(d => d.StartDateKid, o => o.MapFrom(s => s.AssignedTutor.Kid.StartDate))
                    .ForMember(d => d.Room, o => o.MapFrom(s => s.AssignedRoom.Room.Description))
                    .ForMember(d => d.City, o => o.MapFrom(s => s.AssignedRoom.City.Description))
                    .ForMember(d => d.Turn, o => o.MapFrom(s => s.AssignedRoom.Turn.Description))
                    .ForMember(d => d.Observations, o => o.MapFrom(s => s.AssignedRoom.Observations))
                    .ForMember(d => d.Modality, o => o.MapFrom(s => s.AssignedRoom.Modality.Description))
                    .ForMember(d => d.BranchOffice, o => o.MapFrom(s => s.AssignedRoom.BranchOffice.Description))
                    .ForMember(d => d.Collaborator, o => o.MapFrom(s => $"{s.AssignedRoom.Collaborator.Name} {s.AssignedRoom.Collaborator.FirstLastName} " +
                                                                       $"{s.AssignedRoom.Collaborator.SecondLastName}"));
                cfg.CreateMap<AssignedTutor, Parents>()
                    .ForMember(d => d.ParentName, o => o.MapFrom(s => $"{s.Parent.Name} {s.Parent.FirstLastName} {s.Parent.SecondLastName}"))
                    .ForMember(d => d.Relation, o => o.MapFrom(s => s.Relationship.Description))
                    .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.Parent.PhoneNumber))
                    .ForMember(d => d.BloodTypeParent, o => o.MapFrom(s => s.Parent.BloodType.Description))
                    .ForMember(d => d.SexParent, o => o.MapFrom(s => s.Parent.SexType.Description))
                    .ForMember(d => d.Address, o => o.MapFrom(s => s.Parent.Address))
                    .ForMember(d => d.MaritalStatus, o => o.MapFrom(s => s.Parent.MaritalStatus.Description))
                    .ForMember(d => d.IsAuthorized, o => o.MapFrom(s => s.IsAuthorized));


            });
            mapper = new Mapper(config);
        }

        public Result<List<EnrolledChildrenResult>> GetFilter(EnrollFilterDto dto)
        {
            var enrolleds = ListComplete<EnrolledChildren>()
                .Include(x => x.AssignedTutor).Include(x => x.AssignedRoom).Include(x => x.AssignedTutor.Kid)
                .Include(x => x.AssignedRoom.Room)
                .Where(x => dto.KidId != 0 ? x.AssignedTutor.KidId == dto.KidId : true)
                .Where(x => dto.RoomId != 0 ? x.AssignedRoom.RoomId == dto.RoomId : true)
                .OrderBy(x => x.DateCreation).ToList();
            return enrolleds.Any() ? Result<List<EnrolledChildrenResult>>.SetOk(mapper.Map<List<EnrolledChildrenResult>>(enrolleds))
                                    : Result<List<EnrolledChildrenResult>>.SetError("La Busqueda con los criterios seleccionados, no tiene Resultados, pruebe de nuevo porfavor");
        }

        public Result<List<EnrolledChildrenResult>> GetAll()
        {
            var assingations = ListComplete<EnrolledChildren>().Include(x => x.AssignedTutor).Include(x => x.AssignedRoom).Include(x => x.AssignedTutor.Kid)
                                                               .Include(x => x.AssignedTutor.Parent).Include(x => x.AssignedRoom.Collaborator)
                                                               .Include(x => x.AssignedRoom.Room).Include(x => x.AssignedRoom.Turn)
                                                               .Include(x => x.AssignedRoom.Modality).Include(x => x.AssignedRoom.BranchOffice).ToList();
            return assingations.Any() ? Result<List<EnrolledChildrenResult>>.SetOk(mapper.Map<List<EnrolledChildrenResult>>(assingations))
                                    : Result<List<EnrolledChildrenResult>>.SetError("La Busqueda con los criterios seleccionados, no tiene Resultados, pruebe de nuevo porfavor");
        }

        public Result<EnrollChildrenDetailResult> GetDetail(UpdateAssignedRoomDto dto)
        {
            var data = Context.EnrolledChildrens.Where(x => x.Id == dto.Id).Include(x => x.AssignedTutor).Include(x => x.AssignedRoom)
                .Include(x => x.AssignedTutor.Kid).Include(x => x.AssignedTutor.Kid.BloodType).Include(x => x.AssignedTutor.Kid.SexType)
                .Include(x => x.AssignedRoom.Turn).Include(x => x.AssignedRoom.City).Include(x => x.AssignedRoom.BranchOffice)
                .Include(x => x.AssignedRoom.Room).Include(x => x.AssignedRoom.Collaborator).Include(x => x.AssignedRoom.Modality).SingleOrDefault();
            var parents = Context.AssignedTutors.Where(x => x.KidId == data.AssignedTutor.KidId).Include(x => x.Parent).Include(x => x.Parent.BloodType)
                .Include(x => x.Parent.SexType).Include(x => x.Relationship).Include(x => x.Parent.MaritalStatus).ToList();
            var result = mapper.Map<EnrollChildrenDetailResult>(data);
            result.Parents = mapper.Map<List<Parents>>(parents);
            quantityTutors = parents.Count;
            return Result<EnrollChildrenDetailResult>.SetOk(result);
        }

        public Result<string> Create(CreateEnrolledChildrenDto dto)
        {
            try
            {
                var tutors = Context.AssignedTutors.Where(x => x.KidId == dto.KidId).FirstOrDefault();
                if (tutors == null)
                {
                    return Result<string>.SetError("El menor seleccionado no tiene tutores asignados");
                }
                dto.AssignedTutorId = tutors.Id;
                var ifExists = Context.EnrolledChildrens.Where(x => x.AssignedTutorId == dto.AssignedTutorId && x.AssignedRoomId == dto.AssignedRoomId)
                    .FirstOrDefault();
                if (ifExists != null)
                {
                    return Result<string>.SetError("El menor ya fue registrado en esa sala");
                }
                var enrollData = Context.Save(mapper.Map<EnrolledChildren>(dto));
                if (dto.GeneratePayments && !Context.Payments.Where(x => x.EnrolledChildrenId == enrollData.Id).Any())
                {
                    if (enrollData == null)
                    {
                        return Result<string>.SetError("La operacion no se realizo correctamente, verifique y vuelva a intertarlo");
                    }
                    GeneratePayments(enrollData, dto);
                }
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("Not Saved");
            }
            return Result<string>.SetOk("Saved with success");
        }

        public Result<string> Update(UpdateEnrolledChildrenDto dto)
        {

            var row = GetById<EnrolledChildren>(dto.Id);
            if (row != null)
            {
                dto.AssignedTutorId = row.AssignedTutorId;
                Context.Save(mapper.Map(dto, row));
                return Result<string>.SetOk("Operacion Exitosa");
            }
            else
            { return Result<string>.SetError("No se pudo realizar la operacion"); }
        }

        public Result<string> ActivateOrDeactivate(UpdateEnrolledChildrenDto dto)
        {
            var data = Get(dto.Id);
            if (data == null) return Result<string>.SetError("El registro no existe, revise porfavor");
            data.IsDeleted = dto.IsDeleted;
            Context.Save(data);
            return Result<string>.SetOk("Operacion Exitosa");
        }

        private (int currentMonth, int monthsToDecember) CalculateMonthsToDecember(DateTime date)
        {
            int currentMonth = date.Month;
            int monthsToDecember;

            if (currentMonth == 12)
            {
                monthsToDecember = 0;
            }
            else
            {
                monthsToDecember = 12 - currentMonth;
            }

            return (currentMonth, monthsToDecember);
        }

        private void GeneratePayments(EnrolledChildren enrollData, CreateEnrolledChildrenDto dto)
        {
            var calculateMonths = CalculateMonthsToDecember(enrollData.DateCreation);
            for (var i = 1; i <= calculateMonths.monthsToDecember; i++)
            {
                var dateToPay = new DateTime(enrollData.DateCreation.Year, enrollData.DateCreation.Month + i, 9, 12, 0, 0, DateTimeKind.Utc);
                Context.Save(new Payment
                {
                    Amount = dto.Amount,
                    EnrolledChildrenId = enrollData.Id,
                    Description = "Pago Autogenerado en la Inscripcion",
                    IsVerified = false,
                    NumberBill = string.Empty,
                    DateToPay = dateToPay,
                    AuditPaymentId = Context.AuditPaymentTypes.Where(x => x.Description.Contains("SIN SELECCIONAR")).FirstOrDefault().Id,
                    PaymentTypeId = Context.PaymentTypes.Where(x => x.Description.Contains("SIN SELECCIONAR")).FirstOrDefault().Id,
                    PaymentOperationId = Context.PaymentOperations.Where(x => x.Description.Contains("REGISTRADO")).FirstOrDefault().Id,
                    Observations = "Pagos Generados con exito",
                });
            }
        }
    }
}
