using System.Security.Principal;
using AutoMapper;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using AjoloApp.Core.Business;
using AjoloApp.Model.Kid;
using AjoloApp.Repository.AjoloAppRepository;
using AjoloApp.Repository.AjoloAppRepository.Entities;

namespace AjoloApp.RuleEngine.KidService
{
    public class KidService : BaseBusiness<Kid, AjoloAppContext>, IKidService
    {
        IMapper mapper;

        public KidService(AjoloAppContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Kid, KidsResult>()
                    .ForMember(d => d.Age, o => o.MapFrom(s => CalculateAge(s.BornDate.Value)))
                    .ForMember(d => d.Sex, o => o.MapFrom(s => s.SexType.Description))
                    .ForMember(d => d.BloodType, o => o.MapFrom(s => s.BloodType.Description))
                    .ForMember(d => d.DocumentType, o => o.MapFrom(s => s.DocumentType.Description))
                    .ForMember(d => d.UserCreation, o => o.MapFrom(s => CutUser(s.UserCreated.UserName)))
                    .ForMember(d => d.UserModification, o => o.MapFrom(s => CutUser(s.UserModificated.UserName)));
                cfg.CreateMap<CreateKidDto, Kid>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdateKidDto, Kid>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<AssignedTutor, GetDetailKidResult>()
                    .ForMember(d => d.KidName, o => o.MapFrom(s => $"{s.Kid.Name} {s.Kid.FirstLastName} {s.Kid.SecondLastName}"))
                    .ForMember(d => d.AgeKid, o => o.MapFrom(s => CalculateAge(s.Kid.BornDate.Value)))
                    .ForMember(d => d.DocumentType, o => o.MapFrom(s => s.Kid.DocumentType.Description))
                    .ForMember(d => d.DocumentNumber, o => o.MapFrom(s => s.Kid.DocumentNumber))
                    .ForMember(d => d.ParentName, o => o.MapFrom(s => $"{s.Parent.Name} {s.Parent.FirstLastName} {s.Parent.SecondLastName}"))
                    .ForMember(d => d.Relation, o => o.MapFrom(s => s.Relationship.Description))
                    .ForMember(d => d.MaritalStatus, o => o.MapFrom(s => s.Parent.MaritalStatus.Description))
                    .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.Parent.PhoneNumber))
                    .ForMember(d => d.IsAuthorized, o => o.MapFrom(s => s.IsAuthorized));
            });
            mapper = new Mapper(config);
        }

        public Result<KidsResult> GetKidById(KidByIdDto dto)
        {
            return Result<KidsResult>.SetOk(mapper.Map<KidsResult>(GetById<Kid>(dto.Id)));
        }

        public Result<KidsResult> GetKid(GetKidDto dto)
        {
            var kid = GetComplete<Kid>(dto.Name, dto.FirstLastName, dto.SecondLastName, dto.SexTypeId);
            if (dto.BornDate != null) kid = kid.Where(x => x.BornDate == dto.BornDate);
            if (dto.StartDate != null) kid = kid.Where(x => x.StartDate == dto.StartDate);
            return kid.FirstOrDefault() == null ? Result<KidsResult>.SetError("El Menor no esta Registrado") 
                : Result<KidsResult>.SetOk(mapper.Map<KidsResult>(kid.First()));
        }

        public Result<List<KidsResult>> GetAllKids()
        {
            return Result<List<KidsResult>>.SetOk(mapper.Map<List<KidsResult>>(ListComplete<Kid>()
                .Include(x => x.DocumentType).Include(x => x.SexType).Include(x => x.BloodType)
                .OrderBy(x => x.DateCreation)));
        }

        public Result<List<GetDetailKidResult>> GetDetailKid(GetDetailKidDto dto)
        {
            var data = Context.AssignedTutors.Where(x => x.KidId == dto.KidId)
                .Include(x => x.Kid).Include(x => x.Kid.DocumentType)
                .Include(x => x.Parent).Include(x => x.Parent.MaritalStatus).Include(x => x.Relationship).ToList();
            var result = mapper.Map<List<GetDetailKidResult>>(data);

            return Result<List<GetDetailKidResult>>.SetOk(result);
        }

        public Result<string> CreateKid(CreateKidDto dto)
        {
            try
            {
                var kid = GetComplete<Kid>(dto.Name, dto.FirstLastName, dto.SecondLastName, 0).FirstOrDefault();
                if (kid != null) return Result<string>.SetError("Menor no esta registrado, verifique porfavor");
                Context.Save(mapper.Map<Kid>(dto));
                return Result<string>.SetOk("Operacion Exitosa");
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("Operacion Fallida, intente de nuevo");
            }
        }

        public Result<string> UpdateKid(UpdateKidDto dto)
        {
            try
            {
                var kid = Get(dto.Id);
                if (kid == null) return Result<string>.SetError("Menor no esta registrado, verifique porfavor");
                mapper.Map(
                      source: dto,
                      destination: kid,
                      sourceType: typeof(UpdateKidDto),
                      destinationType: typeof(Kid)
                );
                Context.Save(kid);
                return Result<string>.SetOk("Operacion Exitosa");
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("Operacion Fallida, intente de nuevo");
            }
        }

        public Result<string> ActivateOrDeactivate(KidByIdDto dto)
        {
            var kid = Get(dto.Id);
            if (kid == null) return Result<string>.SetError("Menor no esta registrado, verifique porfavor");
            kid.IsDeleted = dto.IsDeleted;
            Context.Save(kid);
            return Result<string>.SetOk("Operacion Exitosa");
        }


        public Result<ReportResult> GeneratePdf(string title)
        {
            var list = ListComplete<Kid>()
                .Include(x => x.DocumentType).Include(x => x.SexType).Include(x => x.BloodType)
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
            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase("Nombre", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Edad", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
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

            cell = new PdfPCell(new Phrase("Tipo de Sangre", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Genero", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            // Contenido de las celdas
            Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            foreach (var row in list)
            {
                table.AddCell(new PdfPCell(new Phrase($"{row.Name} {row.FirstLastName} {row.SecondLastName}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(CalculateAge(row.BornDate.Value), cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.PlaceBorn, cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{row.DocumentType.Initial}: {row.DocumentNumber}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.BloodType.Initial, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.SexType.Description, cellFont)));
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
