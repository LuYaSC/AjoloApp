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
                //cfg.CreateMap<Collaborator, CollaboratorsResult>()
                //   .ForMember(d => d.BranchOfficeAssigned, o => o.MapFrom(s => s.User.UserDetail.BranchOffice.Description))
                //   .ForMember(d => d.CityAssigned, o => o.MapFrom(s => s.User.UserDetail.City.Description))
                //   .ForMember(d => d.DocumentType, o => o.MapFrom(s => s.DocumentType.Description))
                //   .ForMember(d => d.BloodType, o => o.MapFrom(s => s.BloodType.Description))
                //   .ForMember(d => d.Sex, o => o.MapFrom(s => s.SexType.Description))
                //   .ForMember(d => d.Email, o => o.MapFrom(s => s.User.Email))
                //   .ForMember(d => d.Roles, o => o.MapFrom(s => roles))
                //   .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                //   .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
                //cfg.CreateMap<CreateCollaboratorDto, Collaborator>().AfterMap<TrimAllStringProperty>()
                //            .ForMember(d => d.UserId, o => o.MapFrom(s => newUserId));
                //cfg.CreateMap<UpdateCollaboratorDto, Collaborator>().AfterMap<TrimAllStringProperty>();
            });
            mapper = new Mapper(config);
        }

        public Result<DashboardResult> GetData()
        {
            var result = new DashboardResult();

            //first Section
            var payments = Context.Payments.Include(x => x.PaymentOperation).ToList();
            var collaborators = Context.Collaborators.Include(x => x.User).Include(x => x.User.UserDetail).Include(x => x.User.UserDetail.BranchOffice)
                                                    .Include(x => x.User.UserDetail.City).ToList();
            var enrollChildren = Context.EnrolledChildrens.Include(x => x.AssignedRoom).ToList();
            result.QuantityRegisteredPayments = payments.Where(x => x.PaymentOperation.Description == "REGISTRO AUTOMATICO").Count();
            result.TotalRegisteredPayments = payments.Where(x => x.PaymentOperation.Description == "REGISTRO AUTOMATICO").Sum(x => x.Amount);
            result.QuantityPayedPayments = payments.Where(x => x.PaymentOperation.Description == "PAGADO").Count();
            result.TotalPayedPayments = payments.Where(x => x.PaymentOperation.Description == "PAGADO").Sum(x => x.Amount); ;
            result.QuantityPartiallyPayedPayments = payments.Where(x => x.PaymentOperation.Description == "PAGADO PARCIALMENTE").Count();
            result.TotalPartiallyPayedPayments = payments.Where(x => x.PaymentOperation.Description == "PAGADO PARCIALMENTE").Sum(x => x.Amount);
            result.QuantityUnpayPayments = payments.Where(x => x.PaymentOperation.Description == "MORA").Count();
            result.TotalUnpayPayments = payments.Where(x => x.PaymentOperation.Description == "MORA").Sum(x => x.Amount);

            //TO DO Second Sectiion


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
    }
}
