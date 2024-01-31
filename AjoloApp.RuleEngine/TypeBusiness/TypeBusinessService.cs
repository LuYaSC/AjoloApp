using System.Data;
using System.Security.Principal;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AjoloApp.Core.Business;
using AjoloApp.Model.TypeBusiness;
using AjoloApp.Repository.Base;
using AjoloApp.Repository.AjoloAppRepository;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace AjoloApp.RuleEngine.TypeBusinessService
{
    public class TypeBusinessService<T> : BaseBusiness<T, AjoloAppContext>, ITypeBusinessService<T>
        where T : BaseType, new()
    {
        IMapper mapper;

        public TypeBusinessService(AjoloAppContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<T, GetTypeResult>()
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => CutUser(s.UserCreated.UserName)))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => CutUser(s.UserModificated.UserName)));
            });
            mapper = new Mapper(config);
        }

        public Result<GetTypeResult> GetById(GetTypeByIdDto dto)
        {
            var data = Context.Set<T>().Include(x => x.UserCreated).Include(x => x.UserModificated).Where(x => x.Id == dto.Id).FirstOrDefault();
            return Result<GetTypeResult>.SetOk(mapper.Map<GetTypeResult>(data));
        }

        public Result<List<GetTypeResult>> GetAll()
        {
            var data = Context.Set<T>().Include(x => x.UserCreated).Include(x => x.UserModificated).OrderBy(x => x.DateCreation).ToList();
            return data.Any() ? Result<List<GetTypeResult>>.SetOk(mapper.Map<List<GetTypeResult>>(data)) :
              Result<List<GetTypeResult>>.SetError("No se encontraron Registros");
        }

        public Result<string> Update(GetTypeByIdDto dto)
        {
            try
            {
                var data = Get(dto.Id);
                if (data == null) return Result<string>.SetError("Doesnt Exist Type");

                data.Description = dto.Description.Trim().ToUpper();
                data.Initial = dto.Initial?.Trim().ToUpper();
                data.IsDeleted = dto.IsDisabled;
                Save(data);
                return Result<string>.SetOk("Update Success");
            }
            catch (Exception ex)
            {
                return Result<string>.SetError($"{ex.Message}");
            }
        }

        public Result<string> Create(CreateTypeDto dto)
        {
            try
            {
                Save(new T
                {
                    Description = dto.Description.Trim().ToUpper(),
                    Initial = dto.Initial?.Trim().ToUpper(),
                    IsDeleted = false
                });
                return Result<string>.SetOk("Create Success"); ;
            }
            catch (Exception ex)
            {
                return Result<string>.SetError($"{ex.Message}");
            }
        }

        public Result<ReportResult> GeneratePdf(string title)
        {
            var list = Context.Set<T>().Include(x => x.UserCreated)
                        .Include(x => x.UserModificated).OrderBy(x => x.DateCreation).ToList();

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
            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase("Nombre", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Iniciales", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Usuario de Creación", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Fecha de Creación", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Usuario de Modificación", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            // Contenido de las celdas
            Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            foreach (var row in list)
            {
                table.AddCell(new PdfPCell(new Phrase(row.Description, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Initial, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.UserCreated.UserName, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.DateCreation.ToString(), cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.UserModificated.UserName, cellFont)));
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
