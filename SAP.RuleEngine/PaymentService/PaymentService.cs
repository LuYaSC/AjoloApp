using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SAP.Core.Business;
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
                   .ForMember(d => d.PaymentOperation, o => o.MapFrom(s => s.PaymentOperation.Description))
                   .ForMember(d => d.PaymentType, o => o.MapFrom(s => s.PaymentType.Description))
                   .ForMember(d => d.AuditPayment, o => o.MapFrom(s => s.AuditPaymentType.Description))
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
                cfg.CreateMap<CreatePaymentDto, Payment>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdatePaymentDto, Payment>().AfterMap<TrimAllStringProperty>();
            });
            mapper = new Mapper(config);
        }

        public Result<List<PaymentResult>> GetAll()
        {
            var payments = ListComplete<Payment>().Include(x => x.AssignedTutor).Include(x => x.AssignedRoom).Include(x => x.AssignedTutor.Kid)
                                                      .Include(x => x.AssignedTutor.Parent).Include(x => x.AssignedRoom.Collaborator)
                                                      .Include(x => x.AssignedRoom.Room).Include(x => x.AssignedRoom.Turn)
                                                      .Include(x => x.AssignedRoom.Modality).Include(x => x.AssignedRoom.BranchOffice)
                                                      .Include(x => x.PaymentOperation).Include(x => x.PaymentType)
                                                      .Include(x => x.AuditPaymentType).ToList();

            return payments.Any() ? Result<List<PaymentResult>>.SetOk(mapper.Map<List<PaymentResult>>(payments))
                                      : Result<List<PaymentResult>>.SetError("Doesnt Exist Data");
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
    }
}
