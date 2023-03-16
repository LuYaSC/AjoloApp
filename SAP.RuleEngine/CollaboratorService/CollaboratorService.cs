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
                   .ForMember(d => d.BranchOfficeAssigned, o => o.MapFrom(s => s.User.UserDetail.BranchOffice.Description))
                   .ForMember(d => d.CityAssigned, o => o.MapFrom(s => s.User.UserDetail.City.Description))
                   .ForMember(d => d.DocumentType, o => o.MapFrom(s => s.DocumentType.Description))
                   .ForMember(d => d.BloodType, o => o.MapFrom(s => s.BloodType.Description))
                   .ForMember(d => d.Sex, o => o.MapFrom(s => s.SexType.Description))
                   .ForMember(d => d.Email, o => o.MapFrom(s => s.User.Email))
                   .ForMember(d => d.Roles, o => o.MapFrom(s => roles))
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => s.UserCreated.UserName))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => s.UserModificated.UserName));
                cfg.CreateMap<CreateCollaboratorDto, Collaborator>().AfterMap<TrimAllStringProperty>()
                            .ForMember(d => d.UserId, o => o.MapFrom(s => newUserId));
                cfg.CreateMap<UpdateCollaboratorDto, Collaborator>().AfterMap<TrimAllStringProperty>();
            });
            mapper = new Mapper(config);
        }

        public Result<List<CollaboratorsResult>> GetCollaborators()
        {
            var listCollab = ListComplete<Collaborator>().Include(x => x.User).Include(x => x.User.UserDetail.BranchOffice).Include(x => x.User.UserDetail.City).Include(x => x.BloodType)
                                                         .Include(x => x.DocumentType).Include(x => x.SexType);
            GetRoles(listCollab.ToList());
            return listCollab.Any() ? Result<List<CollaboratorsResult>>.SetOk(mapper.Map<List<CollaboratorsResult>>(listCollab)) :
                 Result<List<CollaboratorsResult>>.SetError("Doest exist data");
        }

        public Result<CollaboratorsResult> GetCollaboratorById(CollaboratorByIdDto dto)
        {
            var collaborator = Context.Collaborators.Include(x => x.User).Include(x => x.User.UserDetail.BranchOffice).Include(x => x.User.UserDetail.City).Include(x => x.BloodType)
                                                    .Include(x => x.DocumentType).Include(x => x.UserCreated).Include(x => x.UserModificated).Include(x => x.SexType)
                                                    .Where(x => x.Id == dto.Id).OrderBy(x => x.DateCreation).ToList();
            GetRoles(collaborator);
            return collaborator.Any() ? Result<CollaboratorsResult>.SetOk(mapper.Map<CollaboratorsResult>(collaborator.First())) :
               Result<CollaboratorsResult>.SetError("Doest exist data");
        }

        public Result<string> CreateCollaborator(CreateCollaboratorDto dto)
        {
            var collaborator = GetComplete<Collaborator>(dto.Name, dto.FirstLastName, dto.SecondLastName, dto.SexTypeId);
            if (collaborator.FirstOrDefault() != null) return Result<string>.SetError("Collab Exists");

            var newUserCollab = authService.Register(new RegisterModel
            {
                BranchOfficeId = dto.BranchOfficeId,
                CityId = dto.CityId,
                Email = dto.Email,
                Username = dto.Email,
                HaveDetail = true,
                Roles = dto.Roles,
                Password = $"Sapg{dto.DocumentNumber}-{DateTime.Now.Year}$"
            });
            //Creation new User
            if (!newUserCollab.Result.IsValid) return Result<string>.SetError($"{newUserCollab.Result.Message}");
            newUserId = newUserCollab.Result.UserId;

            //Creation Collab
            var collab = Context.Save(mapper.Map<Collaborator>(dto));
            return collab.UserId != 0 ? Result<string>.SetOk("User created with success") : Result<string>.SetError("User dont created");
        }

        public Result<string> UpdateCollaborator(UpdateCollaboratorDto dto)
        {
            var collaborator = GetById<Collaborator>(dto.Id);
            if (collaborator == null)
            {
                return Result<string>.SetError("Collab doesnt Exists");
            }

            Save(mapper.Map(dto, collaborator));

            var users = Context.Users.Where(x => x.Email == dto.Email).ToList();
            if (!users.Any())
            {
                return Result<string>.SetError("Email no existe para ningun usuario");
            }

            if (users.Count > 1)
            {
                return Result<string>.SetError("Email ya existe para otro usuario");
            }

            var user = users.Single();
            user.Email = dto.Email;

            UpdateUserRole(user.Id, dto.Roles.First());

            return Result<string>.SetOk("Collaborator updated successfully.");
        }

        #region private Methods

        private void UpdateUserRole(int userId, int roleDto)
        {
            var userRoles = Context.UserRoles.Where(x => x.UserId == userId).ToList();
            var roleId = roleDto;

            if (!userRoles.Any())
            {
                Context.Add(new UserRole
                {
                    RoleId = roleId,
                    DateCreation = DateTime.UtcNow,
                    IsDeleted = false,
                    UserId = userId
                });
                Context.SaveChanges();
                return;
            }

            userRoles.ForEach(r => r.IsDeleted = true);
            var existingRole = userRoles.SingleOrDefault(r => r.RoleId == roleId);

            if (existingRole != null)
            {
                existingRole.IsDeleted = false;
                Context.SaveChanges();
                return;
            }

            userRoles.ForEach(r => r.IsDeleted = true);

            Context.Add(new UserRole
            {
                RoleId = roleId,
                DateCreation = DateTime.UtcNow,
                IsDeleted = false,
                UserId = userId
            });

            Context.SaveChanges();
        }
     
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
