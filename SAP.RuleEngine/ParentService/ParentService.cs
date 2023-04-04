using AutoMapper;
using SAP.Core.Business;
using SAP.Model.Parent;
using SAP.Repository.SAPRepository.Entities;
using SAP.Repository.SAPRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace SAP.RuleEngine.ParentService
{
    public class ParentService : BaseBusiness<Parent, SAPContext>, IParentService
    {
        public IMapper mapper;

        public ParentService(SAPContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Parent, ParentsResult>()
                    .ForMember(d => d.MaritalStatus, o => o.MapFrom(s => s.MaritalStatus.Description))
                    .ForMember(d => d.DocumentType, o => o.MapFrom(s => s.DocumentType.Description))
                    .ForMember(d => d.Sex, o => o.MapFrom(s => s.SexType.Description))
                    .ForMember(d => d.BloodType, o => o.MapFrom(s => s.BloodType.Description))
                    .ForMember(d => d.UserCreation, o => o.MapFrom(s => CutUser(s.UserCreated.UserName)))
                    .ForMember(d => d.UserModification, o => o.MapFrom(s => CutUser(s.UserModificated.UserName)));
                cfg.CreateMap<CreateParentDto, Parent>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdateParentDto, Parent>().AfterMap<TrimAllStringProperty>();
            });
            mapper = new Mapper(config);
        }

        public Result<ParentsResult> GetParentById(ParentByIdDto dto)
        {
            return Result<ParentsResult>.SetOk(mapper.Map<ParentsResult>(GetById<Parent>(dto.Id)));
        }

        public Result<ParentsResult> GetParent(GetParentDto dto)
        {
            var Parent = GetComplete<Parent>(dto.Name, dto.FirstLastName, dto.SecondLastName, dto.SexTypeId);
            return Parent.FirstOrDefault() == null ? Result<ParentsResult>.SetError("El Padre no esta registrado, verifique porfavor")
                                        : Result<ParentsResult>.SetOk(mapper.Map<ParentsResult>(Parent.First()));
        }

        public Result<List<ParentsResult>> GetAllParents()
        {
            return Result<List<ParentsResult>>.SetOk(mapper.Map<List<ParentsResult>>(ListComplete<Parent>().Include(x => x.MaritalStatus).Include(x => x.DocumentType)
                .Include(x => x.BloodType).Include(x => x.SexType).OrderBy(x => x.DateCreation)));
        }

        public Result<string> CreateParent(CreateParentDto dto)
        {
            try
            {
                var Parent = GetComplete<Parent>(dto.Name, dto.FirstLastName, dto.SecondLastName, 0).FirstOrDefault();
                if (Parent != null) return Result<string>.SetError("El Padre no esta registrado, verifique porfavor");
                Context.Save(mapper.Map<Parent>(dto));
                return Result<string>.SetOk("Operacion Exitosa");
            }
            catch (Exception ex)
            {
                return Result<string>.SetError($"Operacion Fallida");
            }
        }

        public Result<string> UpdateParent(UpdateParentDto dto)
        {
            try
            {
                var Parent = Get(dto.Id);
                if (Parent == null) return Result<string>.SetError("El Padre no esta registrado, verifique porfavor");
                Context.Save(mapper.Map(dto, Parent));
                return Result<string>.SetOk("Operacion Exitosa");
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("Operacion Fallida");
            }
        }

        public Result<string>ActivateOrDeactivate(DeleteDto dto)
        {
            var Parent = Get(dto.Id);
            if (Parent == null) return Result<string>.SetError("El Padre no esta registrado, verifique porfavor");
            Parent.IsDeleted = dto.IsDeleted;
            Save(Parent);
            return Result<string>.SetOk("Operacion Exitosa");
        }

        public Result<ReportResult> GeneratePdf(string title)
        {
            var list = ListComplete<Parent>()
                .Include(x => x.DocumentType).Include(x => x.SexType).Include(x => x.BloodType).Include(x => x.MaritalStatus)
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
            PdfPTable table = new PdfPTable(7);
            table.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase("Nombre", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Direccion", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Lugar de Nacimiento", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
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

            cell = new PdfPCell(new Phrase("Estado Civil", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
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
                table.AddCell(new PdfPCell(new Phrase(row.Address, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.PlaceBorn, cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{row.DocumentType.Initial}: {row.DocumentNumber}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.PhoneNumber, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.MaritalStatus.Description, cellFont)));
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
    }
}
