using AutoMapper;
using SAP.Core.Business;
using SAP.Model.Authentication;
using SAP.Model.Collaborator;
using SAP.Model.Kid;
using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Entities;
using SAP.RuleEngine.AuthenticationService;
using System.Security.Cryptography;
using System.Security.Principal;

namespace SAP.RuleEngine.CollaboratorService
{
    public class CollaboratorService : BaseBusiness<Collaborator, SAPContext>, ICollaboratorService
    {
        IMapper mapper;
        IAuthenticationService authService;
        public CollaboratorService(SAPContext context, IPrincipal userInfo, IAuthenticationService authService) : base(context, userInfo)
        {
            this.authService = authService;
            var config = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<Kid, KidsResult>()
                //   .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                //   .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
                cfg.CreateMap<CreateCollaboratorDto, Collaborator>().AfterMap<TrimAllStringProperty>();
                //cfg.CreateMap<UpdateKidDto, Kid>().AfterMap<TrimAllStringProperty>();
            });
            mapper = new Mapper(config);
        }

        public Result<List<CollaboratorsResult>> GetCollaborators()
        {
            var listCollab = List();
            return listCollab.Any() ? Result<List<CollaboratorsResult>>.SetOk(mapper.Map<List<CollaboratorsResult>>(listCollab)) :
                 Result<List<CollaboratorsResult>>.SetError("Doest exist data");
        }

        public Result<CollaboratorsResult> GetCollaboratorById(CollaboratorByIdDto dto)
        {
            var collaborator = GetById<Collaborator>(dto.Id);
            return collaborator == null ? Result<CollaboratorsResult>.SetOk(mapper.Map<CollaboratorsResult>(collaborator)) :
               Result<CollaboratorsResult>.SetError("Doest exist data");
        }

        public Result<string> CreateCollaborator(CreateCollaboratorDto dto)
        {
            var collaborator = GetComplete<Collaborator>(dto.Name, dto.FirstLastName, dto.SecondLastName, dto.Sex);
            if (collaborator.FirstOrDefault() != null) return Result<string>.SetError("Collab Exists");

            var newUserCollab = authService.Register(new RegisterModel
            {
                BranchOfficeId = dto.BranchOfficeId,
                CityId = dto.CityId,
                Email = dto.Email,
                Username = dto.Email,
                HaveDetail = true,
                Roles = dto.Roles,
                Password = $"{dto.DocumentNumber}{DateTime.Now.Year}{DateTime.Now.Month}$"
            });
            if(!newUserCollab.IsCompletedSuccessfully) return Result<string>.SetError("User Dont Create");

            var collab = Context.Save(mapper.Map<Collaborator>(dto));
            return collab.UserId != 0 ? Result<string>.SetOk("User created with success") : Result<string>.SetError("User dont created");
        }
    }
}
