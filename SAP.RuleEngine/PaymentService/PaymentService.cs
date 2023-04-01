using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SAP.Core.Business;
using SAP.Model.AssignationRoom;
using SAP.Model.EnrolledChildren;
using SAP.Model.Payment;
using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Entities;
using System.Security.Principal;

namespace SAP.RuleEngine.PaymentService
{
    public class PaymentService : BaseBusiness<Payment, SAPContext>, IPaymentService
    {
        IMapper mapper;

        public PaymentService(SAPContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Payment, PaymentResult>()
                   .ForMember(d => d.Parent, o => o.MapFrom(s => $"{s.EnrolledChildren.AssignedTutor.Parent.Name} {s.EnrolledChildren.AssignedTutor.Parent.FirstLastName} " +
                                                                 $"{s.EnrolledChildren.AssignedTutor.Parent.SecondLastName}"))
                   .ForMember(d => d.Kid, o => o.MapFrom(s => $"{s.EnrolledChildren.AssignedTutor.Kid.Name} {s.EnrolledChildren.AssignedTutor.Kid.FirstLastName} " +
                                                              $"{s.EnrolledChildren.AssignedTutor.Kid.SecondLastName}"))
                   .ForMember(d => d.Collaborator, o => o.MapFrom(s => $"{s.EnrolledChildren.AssignedRoom.Collaborator.Name} {s.EnrolledChildren.AssignedRoom.Collaborator.FirstLastName} " +
                                                                       $"{s.EnrolledChildren.AssignedRoom.Collaborator.SecondLastName}"))
                   .ForMember(d => d.Room, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.Room.Description))
                   .ForMember(d => d.Turn, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.Turn.Description))
                   .ForMember(d => d.Modality, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.Modality.Description))
                   .ForMember(d => d.BranchOffice, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.BranchOffice.Description))
                   .ForMember(d => d.PaymentOperation, o => o.MapFrom(s => s.PaymentOperation.Description))
                   .ForMember(d => d.PaymentType, o => o.MapFrom(s => s.PaymentType.Description))
                   .ForMember(d => d.AuditPayment, o => o.MapFrom(s => s.AuditPaymentType.Description))
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
                cfg.CreateMap<CreatePaymentDto, Payment>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdatePaymentDto, Payment>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<Payment, PaymentDetailResult>()
                    .ForMember(d => d.ParentName, o => o.MapFrom(s => $"{s.EnrolledChildren.AssignedTutor.Parent.Name} {s.EnrolledChildren.AssignedTutor.Parent.FirstLastName} " +
                                                                        $"{s.EnrolledChildren.AssignedTutor.Parent.SecondLastName}"))
                    .ForMember(d => d.ParentRelation, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Relationship.Description))
                    .ForMember(d => d.ParentMaritalStatus, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Parent.MaritalStatus.Description))
                    .ForMember(d => d.ParentDocumentType, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Parent.DocumentType.Description))
                    .ForMember(d => d.ParentDocumentNumber, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Parent.DocumentNumber))
                    .ForMember(d => d.ParentPhoneNumber, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Parent.PhoneNumber))
                    .ForMember(d => d.CollaboratorName, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Parent.PhoneNumber))
                    .ForMember(d => d.Room, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.Room.Description))
                    .ForMember(d => d.BranchOffice, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.BranchOffice.Description))
                    .ForMember(d => d.City, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.City.Description))
                    .ForMember(d => d.Turn, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.Turn.Description))
                    .ForMember(d => d.Modality, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.Modality.Description))
                    .ForMember(d => d.StartDateKid, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Kid.StartDate))
                    .ForMember(d => d.Antiquity, o => o.MapFrom(s => CalculateAge(s.EnrolledChildren.AssignedTutor.Kid.StartDate)));
            });
            mapper = new Mapper(config);
        }

        public Result<List<PaymentResult>> GetFilter(PaymentFilterDto dto)
        {
            var payments = ListComplete<Payment>().Include(x => x.EnrolledChildren.AssignedTutor).Include(x => x.EnrolledChildren.AssignedRoom).Include(x => x.EnrolledChildren.AssignedTutor.Kid)
                            .Include(x => x.EnrolledChildren.AssignedTutor.Parent).Include(x => x.EnrolledChildren.AssignedRoom.Collaborator)
                            .Include(x => x.EnrolledChildren.AssignedRoom.Room).Include(x => x.EnrolledChildren.AssignedRoom.Turn)
                            .Include(x => x.EnrolledChildren.AssignedRoom.Modality).Include(x => x.EnrolledChildren.AssignedRoom.BranchOffice)
                            .Include(x => x.PaymentOperation).Include(x => x.PaymentType).Include(x => x.AuditPaymentType)
                            .Where(x => dto.KidId != 0 ? x.EnrolledChildren.AssignedTutor.KidId == dto.KidId : true)
                            .Where(x => dto.RoomId != 0 ? x.EnrolledChildren.AssignedRoom.RoomId == dto.RoomId : true)
                            .Where(x => dto.BranchOfficeId != 0 ? x.EnrolledChildren.AssignedRoom.BranchOfficeId == dto.BranchOfficeId : true)
                            .OrderBy(x => x.DateCreation).ToList();

            return payments.Any() ? Result<List<PaymentResult>>.SetOk(mapper.Map<List<PaymentResult>>(payments))
                                    : Result<List<PaymentResult>>.SetError("No existen resultados en la busqueda");
        }

        public Result<List<PaymentResult>> GetAll()
        {
            var payments = ListComplete<Payment>().Include(x => x.EnrolledChildren.AssignedTutor).Include(x => x.EnrolledChildren.AssignedRoom).Include(x => x.EnrolledChildren.AssignedTutor.Kid)
                            .Include(x => x.EnrolledChildren.AssignedTutor.Parent).Include(x => x.EnrolledChildren.AssignedRoom.Collaborator)
                            .Include(x => x.EnrolledChildren.AssignedRoom.Room).Include(x => x.EnrolledChildren.AssignedRoom.Turn)
                            .Include(x => x.EnrolledChildren.AssignedRoom.Modality).Include(x => x.EnrolledChildren.AssignedRoom.BranchOffice)
                            .Include(x => x.PaymentOperation).Include(x => x.PaymentType).Include(x => x.AuditPaymentType)
                            .OrderBy(x => x.DateCreation).ToList();

            return payments.Any() ? Result<List<PaymentResult>>.SetOk(mapper.Map<List<PaymentResult>>(payments))
                                      : Result<List<PaymentResult>>.SetError("No existen resultados en la busqueda");
        }
        public Result<PaymentDetailResult> GetDetail(PaymentDetailDto dto)
        {
            var payment = Context.Payments.Where(x => x.Id == dto.Id).Include(x => x.EnrolledChildren)
                //Parent Data
                .Include(x => x.EnrolledChildren.AssignedTutor)
                .Include(x => x.EnrolledChildren.AssignedTutor.Parent)
                .Include(x => x.EnrolledChildren.AssignedTutor.Parent.MaritalStatus)
                .Include(x => x.EnrolledChildren.AssignedTutor.Parent.DocumentType)
                .Include(x => x.EnrolledChildren.AssignedTutor.Relationship)
                // Room Data
                .Include(x => x.EnrolledChildren.AssignedRoom)
                .Include(x => x.EnrolledChildren.AssignedRoom.Collaborator)
                .Include(x => x.EnrolledChildren.AssignedRoom.Room)
                .Include(x => x.EnrolledChildren.AssignedRoom.BranchOffice)
                .Include(x => x.EnrolledChildren.AssignedRoom.City)
                .Include(x => x.EnrolledChildren.AssignedRoom.Modality)
                .Include(x => x.EnrolledChildren.AssignedRoom.Turn)
                // Kid Data
                .Include(x => x.EnrolledChildren.AssignedTutor.Kid)
                .FirstOrDefault();

            return payment != null ? Result<PaymentDetailResult>.SetOk(mapper.Map<PaymentDetailResult>(payment)) :
               Result<PaymentDetailResult>.SetError("Pago no encontrado");
        }
        public Result<string> Create(List<CreatePaymentDto> dto)
        {
            try
            {
                dto.ForEach(x => Context.Save(mapper.Map<Payment>(x)));
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("Not Saved");
            }
            return Result<string>.SetOk("Saved with success");
        }

        public Result<string> Update(List<UpdatePaymentDto> dto)
        {
            foreach (var list in dto)
            {
                var row = GetById<Payment>(list.Id);
                if (row != null)
                {
                    row = mapper.Map<Payment>(row);
                    Context.Save(row);
                }
            }
            return Result<string>.SetOk("Success");
        }

        public Result<string> ActivateOrDeactivate(PaymentDetailDto dto)
        {
            var data = Get(dto.Id);
            if (data == null) return Result<string>.SetError("El registro no existe, revise porfavor");
            data.IsDeleted = dto.IsDeleted;
            Context.Save(data);
            return Result<string>.SetOk("Operacion Exitosa");
        }
    }
}
