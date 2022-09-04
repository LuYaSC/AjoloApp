using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        int newUserId = 0;
        List<string> roles = new List<string>();

        public CollaboratorService(SAPContext context, IPrincipal userInfo, IAuthenticationService authService) : base(context, userInfo)
        {
            this.authService = authService;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Collaborator, CollaboratorsResult>()
                   .ForMember(d => d.UserAssigned, o => o.MapFrom(s => s.User.Email))
                   .ForMember(d => d.BranchOfficeAssigned, o => o.MapFrom(s => s.User.UserDetail.BranchOffice.Description))
                   .ForMember(d => d.CityAssigned, o => o.MapFrom(s => s.User.UserDetail.City.Description))
                   .ForMember(d => d.DocumentType, o => o.MapFrom(s => s.DocumentType.Description))
                   .ForMember(d => d.Roles, o => o.MapFrom(s => roles))
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
                cfg.CreateMap<CreateCollaboratorDto, Collaborator>().AfterMap<TrimAllStringProperty>()
                            .ForMember(d => d.UserId, o => o.MapFrom(s => newUserId));
                //cfg.CreateMap<UpdateKidDto, Kid>().AfterMap<TrimAllStringProperty>();
            });
            mapper = new Mapper(config);
        }

        public Result<List<CollaboratorsResult>> GetCollaborators()
        {
            var listCollab = ListComplete<Collaborator>().Include(x => x.User).Include(x => x.User.UserDetail.BranchOffice).Include(x => x.User.UserDetail.City)
                                                         .Include(x => x.DocumentType);
            GetRoles(listCollab.ToList());
            return listCollab.Any() ? Result<List<CollaboratorsResult>>.SetOk(mapper.Map<List<CollaboratorsResult>>(listCollab)) :
                 Result<List<CollaboratorsResult>>.SetError("Doest exist data");
        }

        public Result<CollaboratorsResult> GetCollaboratorById(CollaboratorByIdDto dto)
        {
            var collaborator = Context.Collaborators.Include(x => x.User).Include(x => x.User.UserDetail.BranchOffice).Include(x => x.User.UserDetail.City)
                                                    .Include(x => x.DocumentType).Include(x => x.UserCreated).Include(x => x.UserModificated)
                                                    .Where(x => x.Id == dto.Id).ToList();
            GetRoles(collaborator);
            return collaborator.Any() ? Result<CollaboratorsResult>.SetOk(mapper.Map<CollaboratorsResult>(collaborator.First())) :
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
                Password = $"Peq{dto.DocumentNumber}-{DateTime.Now.Year}$"
            });
            //Creation new User
            if(!newUserCollab.Result.IsValid) return Result<string>.SetError($"{newUserCollab.Result.Message}");
            newUserId = newUserCollab.Result.UserId;
            
            //Creation Collab
            var collab = Context.Save(mapper.Map<Collaborator>(dto));
            return collab.UserId != 0 ? Result<string>.SetOk("User created with success") : Result<string>.SetError("User dont created");
        }

        public Result<string> Update(UpdateCollaboratorDto dto)
        {
            var collaborator = GetById<Collaborator>(dto.Id);
            if (collaborator == null) return Result<string>.SetError("Collab doesnt Exists");

            return null;
            //var newUserCollab = authService.Register(new RegisterModel
            //{
            //    BranchOfficeId = dto.BranchOfficeId,
            //    CityId = dto.CityId,
            //    Email = dto.Email,
            //    Username = dto.Email,
            //    HaveDetail = true,
            //    Roles = dto.Roles,
            //    Password = $"Peq{dto.DocumentNumber}-{DateTime.Now.Year}$"
            //});
            ////Creation new User
            //if (!newUserCollab.Result.IsValid) return Result<string>.SetError($"{newUserCollab.Result.Message}");
            //newUserId = newUserCollab.Result.UserId;

            ////Creation Collab
            //var collab = Context.Save(mapper.Map<Collaborator>(dto));
            //return collab.UserId != 0 ? Result<string>.SetOk("User created with success") : Result<string>.SetError("User dont created");
        }

        #region private Methods
        private void GetRoles(List<Collaborator> listCollab)
        {
            foreach (var list in listCollab)
            {
                var userRoles = Context.UserRoles.Where(x => x.UserId == list.UserId).ToList();
                foreach (var userRole in userRoles)
                {
                    var rol = Context.Roles.Where(x => x.Id == userRole.RoleId).FirstOrDefault();
                    if (rol != null)
                    {
                        roles.Add(rol.Name);
                    }
                }
            }
        }
        #endregion private Methods
    }
}
