using AutoMapper;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using AjoloApp.Core.Business;
using AjoloApp.Model.Authentication;
using AjoloApp.Model.Collaborator;
using AjoloApp.Model.Kid;
using AjoloApp.Repository.AjoloAppRepository;
using AjoloApp.Repository.AjoloAppRepository.Entities;
using AjoloApp.RuleEngine.AuthenticationService;
using System.Security.Cryptography;
using System.Security.Principal;

namespace AjoloApp.RuleEngine.CollaboratorService
{
    public class CollaboratorService : BaseBusiness<Collaborator, AjoloAppContext>, ICollaboratorService
    {
        IMapper mapper;
        IAuthenticationService authService;
        int newUserId = 0;
        List<string> roles = new List<string>();

        public CollaboratorService(AjoloAppContext context, IPrincipal userInfo, IAuthenticationService authService) : base(context, userInfo)
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
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => CutUser(s.UserCreated.UserName)))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => CutUser(s.UserModificated.UserName)));
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
                //Password = $"Sapg{dto.DocumentNumber}-{DateTime.Now.Year}$"
                Password = $"pato123AB$"
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
                return Result<string>.SetError("El Colaborador no existe, revise porfavor");
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

            return Result<string>.SetOk("Operacion Exitosa");
        }

        public Result<string> ActivateOrDeactivate(DeleteDto dto)
        {
            var collaborator = Context.Collaborators.Where(x => x.Id == dto.Id).Include(x => x.User).FirstOrDefault();
            if (collaborator == null)
            {
                return Result<string>.SetError("El colaborador no existe, verifique porfavor");
            }
            collaborator.IsDeleted = dto.IsDeleted;
            if (dto.IsDeleted)
            {
                collaborator.User.State = "B";
            }
            else
            {
                collaborator.User.State = "C";
            }
            Save(collaborator);
            return Result<string>.SetOk("Operacion Exitosa");
        }

        public Result<ReportResult> GeneratePdf(string title)
        {
            var list = ListComplete<Collaborator>()
                .Include(x => x.DocumentType).Include(x => x.SexType).Include(x => x.BloodType).Include(x => x.User)
                .Include(x => x.User.UserDetail)
                .OrderBy(x => x.DateCreation).ToList();

            var description = "Esta lista esta generada con datos hasta la fecha";

            // Crear un documento PDF
            Document document = new Document(PageSize.LETTER, 50, 50, 50, 50);

            // Crear un objeto MemoryStream para almacenar el PDF generado
            MemoryStream stream = new MemoryStream();

            // Crear un escritor de PDF que escriba en el MemoryStream
            PdfWriter writer = PdfWriter.GetInstance(document, stream);

            document.Open();

            // Agregar el título y la descripción
            Paragraph titleParagraph = new Paragraph(title, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK));
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            //titleParagraph.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            titleParagraph.SpacingAfter = 20f; // Espacio después del título
            document.Add(titleParagraph);

            Paragraph descriptionParagraph = new Paragraph("Esta lista está generada con datos hasta la fecha", FontFactory.GetFont(FontFactory.HELVETICA, 12));
            descriptionParagraph.Alignment = Element.ALIGN_CENTER;
            descriptionParagraph.SpacingAfter = 10f; // Espacio después de la descripción
            document.Add(descriptionParagraph);

            // Agregar la tabla
            PdfPTable table = new PdfPTable(8);
            table.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase("Nombre", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Edad", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Direccion", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Fecha de Inicio", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Documento", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Nro de Contacto", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Email", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Tipo de Sangre", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            // Contenido de las celdas
            Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            foreach (var row in list)
            {
                table.AddCell(new PdfPCell(new Phrase($"{row.Name} {row.FirstLastName} {row.SecondLastName}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(CalculateAge(row.BornDate), cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Address, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.StartDate.ToShortDateString(), cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{row.DocumentType.Initial}: {row.DocumentNumber}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.PhoneNumber, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.User.Email, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.BloodType.Initial, cellFont)));
            }

            document.Add(table);

            // Cerrar el documento
            document.Close();
            var result = new ReportResult { ReportName = $"{title}-{DateTime.Now}", Report = stream.ToArray() };

            // Devolver el PDF generado como un array de bytes
            return result.Report.Length > 0 ? Result<ReportResult>.SetOk(result)
                                            : Result<ReportResult>.SetError("No se pudo generar el reporte");

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
