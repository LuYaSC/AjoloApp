using AutoMapper;
using AjoloApp.Core.Business;
using AjoloApp.Model.AssignationRoom;
using AjoloApp.Repository.AjoloAppRepository.Entities;
using AjoloApp.Repository.AjoloAppRepository;
using AjoloApp.RuleEngine.AssignationRoomService;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using AjoloApp.Model.AssignationTutor;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace AjoloApp.RuleEngine.AssignationRoomService
{
    public class AssignationRoomService : BaseBusiness<AssignedRoom, AjoloAppContext>, IAssignationRoomService
    {
        IMapper mapper;

        public AssignationRoomService(AjoloAppContext context, IPrincipal userInfo) : base(context, userInfo)
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
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => CutUser(s.UserCreated.UserName)))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => CutUser(s.UserModificated.UserName)));
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

        public Result<ReportResult> GeneratePdf(string title)
        {
            var list = ListComplete<AssignedRoom>()
                .Include(x => x.Collaborator).Include(x => x.Room).Include(x => x.Turn).Include(x => x.City)
                .Include(x => x.Modality).Include(x => x.BranchOffice).OrderBy(x => x.DateCreation).OrderBy(x => x.CollaboratorId).ToList();

            var description = "Esta lista esta generada con datos hasta la fecha";

            // Crear un documento PDF
            Document document = new Document(PageSize.A4.Rotate(), 50, 50, 50, 50);

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
            PdfPTable table = new PdfPTable(7);
            table.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase("Educadora", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Edad", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Nro Contacto", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Sala", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Turno", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Modalidad", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Ciudad", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            // Contenido de las celdas
            Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            foreach (var row in list)
            {
                table.AddCell(new PdfPCell(new Phrase($"{row.Collaborator.Name} {row.Collaborator.FirstLastName} {row.Collaborator.SecondLastName}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(CalculateAge(row.Collaborator.BornDate), cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Collaborator.PhoneNumber, cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{row.Room.Description}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Turn.Description, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Modality.Description, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.City.Description, cellFont)));
            }

            document.Add(table);

            // Cerrar el documento
            document.Close();
            var result = new ReportResult { ReportName = $"{title}-{DateTime.Now}", Report = stream.ToArray() };

            // Devolver el PDF generado como un array de bytes
            return result.Report.Length > 0 ? Result<ReportResult>.SetOk(result)
                                            : Result<ReportResult>.SetError("No se pudo generar el reporte");

        }
    }
}
