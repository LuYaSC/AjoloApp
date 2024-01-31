using AutoMapper;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using AjoloApp.Core.Business;
using AjoloApp.Model.AssignationTutor;
using AjoloApp.Repository.AjoloAppRepository;
using AjoloApp.Repository.AjoloAppRepository.Entities;
using System.Security.Principal;

namespace AjoloApp.RuleEngine.AssignationTutorService
{
    public class AssignationTutorService : BaseBusiness<AssignedTutor, AjoloAppContext>, IAssignationTutorService
    {
        IMapper mapper;

        public AssignationTutorService(AjoloAppContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssignedTutor, AssignationTutorResult>()
                   .ForMember(d => d.KidName, o => o.MapFrom(s => $"{s.Kid.Name} {s.Kid.FirstLastName} {s.Kid.SecondLastName}"))
                   .ForMember(d => d.ParentName, o => o.MapFrom(s => $"{s.Parent.Name} {s.Parent.FirstLastName} {s.Parent.SecondLastName}"))
                   .ForMember(d => d.Relation, o => o.MapFrom(s => s.Relationship.Description))
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => CutUser(s.UserCreated.UserName)))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => CutUser(s.UserModificated.UserName)));
                cfg.CreateMap<CreateAssignedTutorDto, AssignedTutor>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdateAssignedTutorDto, AssignedTutor>().AfterMap<TrimAllStringProperty>();
            });
            mapper = new Mapper(config);
        }

        public Result<List<AssignationTutorResult>> GetFilter(CreateAssignedTutorDto dto)
        {
            var assignationsFilter = ListComplete<AssignedTutor>().Include(x => x.Kid).Include(x => x.Parent).Include(x => x.Relationship)
                .Where(x => dto.KidId != 0 ? x.KidId == dto.KidId : true)
                .Where(x => dto.ParentId != 0 ? x.ParentId == dto.ParentId : true)
                .OrderBy(x => x.DateCreation).OrderBy(x => x.KidId).ToList();
            return assignationsFilter.Any() ? Result<List<AssignationTutorResult>>.SetOk(mapper.Map<List<AssignationTutorResult>>(assignationsFilter))
                                    : Result<List<AssignationTutorResult>>.SetError("Doesnt Exist Data");
        }

        public Result<List<AssignationTutorResult>> GetAll()
        {
            var assingations = ListComplete<AssignedTutor>().Include(x => x.Kid).Include(x => x.Parent)
                .Include(x => x.Relationship).OrderBy(x => x.DateCreation).OrderBy(x => x.KidId).ToList();
            return assingations.Any() ? Result<List<AssignationTutorResult>>.SetOk(mapper.Map<List<AssignationTutorResult>>(assingations))
                                      : Result<List<AssignationTutorResult>>.SetError("Doesnt Exist Data");
        }

        public Result<string> Create(CreateAssignedTutorDto dto)
        {
            try
            {
                var existsData = List();
                if (existsData.Where(x => x.KidId == dto.KidId && x.ParentId == dto.ParentId).FirstOrDefault() == null)
                {
                    Context.Save(mapper.Map<AssignedTutor>(dto));
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

        public Result<string> Update(UpdateAssignedTutorDto dto)
        {
            var row = GetById<AssignedTutor>(dto.Id);
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

        public Result<string> DisableOrEnable(UpdateAssignedTutorDto dto)
        {
            var row = GetById<AssignedTutor>(dto.Id);
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
            var list = ListComplete<AssignedTutor>().Include(x => x.Kid).Include(x => x.Parent)
                .Include(x => x.Relationship).OrderBy(x => x.DateCreation).OrderBy(x => x.KidId).ToList();

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
            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase("Nombre Tutor", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Nombre Niño", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Nro de Contacto", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Parentezco", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Autorizacion Recojo", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Fecha Inicio en el Centro", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            // Contenido de las celdas
            Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            foreach (var row in list)
            {
                table.AddCell(new PdfPCell(new Phrase($"{row.Parent.Name} {row.Parent.FirstLastName} {row.Parent.SecondLastName}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{row.Kid.Name} {row.Kid.FirstLastName} {row.Kid.SecondLastName}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Parent.PhoneNumber, cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{row.Relationship.Description}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.IsAuthorized ? "Autorizado" : "No Autorizado", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Kid.StartDate.ToShortDateString(), cellFont)));
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
