using SAP.Core.Business;
using SAP.Repository.SAPRepository.Entities;
using SAP.Repository.SAPRepository;
using AutoMapper;
using System.Security.Principal;
using SAP.Model.Dashboard;
using Microsoft.EntityFrameworkCore;

namespace SAP.RuleEngine.DashboardService
{
    public class DashboardService : BaseBusiness<Collaborator, SAPContext>, IDashboardService
    {
        IMapper mapper;
        public DashboardService(SAPContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
            });
            mapper = new Mapper(config);
        }

        public Result<DashboardResult> GetData()
        {
            var result = new DashboardResult();
            ValidateUnPayPayments();

            //first Section
            var payments = Context.Payments.Include(x => x.PaymentOperation).Include(x => x.PaymentType);
            var collaborators = Context.Collaborators.Include(x => x.User).Include(x => x.User.UserDetail).Include(x => x.User.UserDetail.BranchOffice)
                                                    .Include(x => x.User.UserDetail.City).ToList();
            var enrollChildren = Context.EnrolledChildrens.Include(x => x.AssignedRoom).ToList();
            result.QuantityRegisteredPayments = payments.Where(x => x.PaymentOperation.Description.Contains("ADELANTADO")).Count();
            result.TotalRegisteredPayments = payments.Where(x => x.PaymentOperation.Description.Contains("ADELANTADO")).Sum(x => x.Amount);
            result.QuantityPayedPayments = payments.Where(x => x.PaymentOperation.Description.Contains("PAGADO")).Count();
            result.TotalPayedPayments = payments.Where(x => x.PaymentOperation.Description.Contains("PAGADO")).Sum(x => x.Amount); ;
            result.QuantityPartiallyPayedPayments = payments.Where(x => x.PaymentOperation.Description.Contains("PAGADO PARCIALMENTE")).Count();
            result.TotalPartiallyPayedPayments = payments.Where(x => x.PaymentOperation.Description.Contains("PAGADO PARCIALMENTE")).Sum(x => x.Amount);
            var updateUnPayPayments = payments.Where(x => x.PaymentOperation.Description.Contains("MORA")).ToList();
            result.QuantityUnpayPayments = payments.Where(x => x.PaymentOperation.Description.Contains("MORA")).Count();
            result.TotalUnpayPayments = payments.Where(x => x.PaymentOperation.Description.Contains("MORA")).Sum(x => x.Amount);

            //TO DO Second Sectiion

            //Section TypePayments
            result.QuantityCashPayments = payments.Where(x => x.PaymentType.Description.Contains("EFECTIVO")).Count();
            result.TotalCashPayments = payments.Where(x => x.PaymentType.Description.Contains("EFECTIVO")).Sum(s => s.Amount);
            result.QuantityQrPayments = payments.Where(x => x.PaymentType.Description.Contains("CODIGO QR")).Count();
            result.TotalQrPayments = payments.Where(x => x.PaymentType.Description.Contains("CODIGO QR")).Sum(s => s.Amount);
            result.QuantityTransferPayments = payments.Where(x => x.PaymentType.Description.Contains("TRASNFERENCIA BANCARIA")).Count();
            result.TotalTransferPayments = payments.Where(x => x.PaymentType.Description.Contains("TRASNFERENCIA BANCARIA")).Sum(s => s.Amount);
            //Third Section
            result.QuantityTotalChildren = Context.Kids.Count();
            result.QuantityTotalParents = Context.Parents.Count();
            result.QuantityTotalCollaborators = collaborators.Count();
            result.QuantityTotalPayments = payments.Count();
            result.QuantityTotalInscriptions = enrollChildren.Count();

            //Four Section
            foreach (var collaborator in collaborators)
            {
                result.Collaborators.Add(new CollaboratorData
                {
                    Collaborator = $"{collaborator.Name} {collaborator.FirstLastName} {collaborator.SecondLastName}",
                    CollaboratorAge = CalculateAge(collaborator.BornDate),
                    CollaboratorBranchOffice = collaborator.User.UserDetail.BranchOffice.Description,
                    CollaboratorCity = collaborator.User.UserDetail.City.Description,
                    CollaboratorStartDate = collaborator.StartDate,
                    IsDeleted = collaborator.IsDeleted,
                    QuantityChildrenAssigned = enrollChildren.Where(x => x.AssignedRoom.CollaboratorId == collaborator.Id).Count()
                }); 
            }
            return Result<DashboardResult>.SetOk(result);
        }

        private void ValidateUnPayPayments()
        {
            var unPayments = Context.Payments.Include(x => x.PaymentOperation)
                .Where(x => x.PaymentOperationId == 6 || x.PaymentOperationId == 3).ToList();
            foreach (var payment in unPayments)
            {
                if(DateTime.UtcNow.Date >  payment.DateToPay.Date)
                {
                    payment.PaymentOperationId = 4;
                    payment.Observations = $"Pago en Mora fecha vencida: {payment.DateToPay.ToShortDateString()}";
                    Context.SaveChanges();
                }
            }
        }
    }
}
