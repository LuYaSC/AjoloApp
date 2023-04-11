using AutoMapper;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using SAP.Core.Business;
using SAP.Model.Payment;
using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Entities;
using System.Security.Principal;

namespace SAP.RuleEngine.PaymentService
{
    public class PaymentService : BaseBusiness<Payment, SAPContext>, IPaymentService
    {
        IMapper mapper;

        public PaymentService(SAPContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Payment, PaymentResult>()
                   .ForMember(d => d.Parent, o => o.MapFrom(s => $"{s.EnrolledChildren.AssignedTutor.Parent.Name} {s.EnrolledChildren.AssignedTutor.Parent.FirstLastName} " +
                                                                 $"{s.EnrolledChildren.AssignedTutor.Parent.SecondLastName}"))
                   .ForMember(d => d.Kid, o => o.MapFrom(s => $"{s.EnrolledChildren.AssignedTutor.Kid.Name} {s.EnrolledChildren.AssignedTutor.Kid.FirstLastName} " +
                                                              $"{s.EnrolledChildren.AssignedTutor.Kid.SecondLastName}"))
                   .ForMember(d => d.Collaborator, o => o.MapFrom(s => $"{s.EnrolledChildren.AssignedRoom.Collaborator.Name} {s.EnrolledChildren.AssignedRoom.Collaborator.FirstLastName} " +
                                                                       $"{s.EnrolledChildren.AssignedRoom.Collaborator.SecondLastName}"))
                   .ForMember(d => d.Room, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.Room.Description))
                   .ForMember(d => d.Turn, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.Turn.Description))
                   .ForMember(d => d.Modality, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.Modality.Description))
                   .ForMember(d => d.BranchOffice, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.BranchOffice.Description))
                   .ForMember(d => d.PaymentOperation, o => o.MapFrom(s => s.PaymentOperation.Description))
                   .ForMember(d => d.PaymentType, o => o.MapFrom(s => s.PaymentType.Description))
                   .ForMember(d => d.AuditPayment, o => o.MapFrom(s => s.AuditPaymentType.Description))
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => CutUser(s.UserCreated.UserName)))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => CutUser(s.UserModificated.UserName)));
                cfg.CreateMap<CreatePaymentDto, Payment>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdatePaymentDto, Payment>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<Payment, PaymentDetailResult>()
                    .ForMember(d => d.ParentName, o => o.MapFrom(s => $"{s.EnrolledChildren.AssignedTutor.Parent.Name} {s.EnrolledChildren.AssignedTutor.Parent.FirstLastName} " +
                                                                        $"{s.EnrolledChildren.AssignedTutor.Parent.SecondLastName}"))
                    .ForMember(d => d.ParentRelation, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Relationship.Description))
                    .ForMember(d => d.ParentMaritalStatus, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Parent.MaritalStatus.Description))
                    .ForMember(d => d.ParentDocumentType, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Parent.DocumentType.Description))
                    .ForMember(d => d.ParentDocumentNumber, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Parent.DocumentNumber))
                    .ForMember(d => d.ParentPhoneNumber, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Parent.PhoneNumber))
                    .ForMember(d => d.CollaboratorName, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Parent.PhoneNumber))
                    .ForMember(d => d.Room, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.Room.Description))
                    .ForMember(d => d.BranchOffice, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.BranchOffice.Description))
                    .ForMember(d => d.City, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.City.Description))
                    .ForMember(d => d.Turn, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.Turn.Description))
                    .ForMember(d => d.Modality, o => o.MapFrom(s => s.EnrolledChildren.AssignedRoom.Modality.Description))
                    .ForMember(d => d.StartDateKid, o => o.MapFrom(s => s.EnrolledChildren.AssignedTutor.Kid.StartDate))
                    .ForMember(d => d.Antiquity, o => o.MapFrom(s => CalculateAge(s.EnrolledChildren.AssignedTutor.Kid.StartDate)));
            });
            mapper = new Mapper(config);
        }

        public Result<List<PaymentResult>> GetFilter(PaymentFilterDto dto)
        {
            var payments = ListComplete<Payment>().Include(x => x.EnrolledChildren.AssignedTutor).Include(x => x.EnrolledChildren.AssignedRoom).Include(x => x.EnrolledChildren.AssignedTutor.Kid)
                            .Include(x => x.EnrolledChildren.AssignedTutor.Parent).Include(x => x.EnrolledChildren.AssignedRoom.Collaborator)
                            .Include(x => x.EnrolledChildren.AssignedRoom.Room).Include(x => x.EnrolledChildren.AssignedRoom.Turn)
                            .Include(x => x.EnrolledChildren.AssignedRoom.Modality).Include(x => x.EnrolledChildren.AssignedRoom.BranchOffice)
                            .Include(x => x.PaymentOperation).Include(x => x.PaymentType).Include(x => x.AuditPaymentType)
                            .Where(x => dto.KidId != 0 ? x.EnrolledChildren.AssignedTutor.KidId == dto.KidId : true)
                            .Where(x => dto.RoomId != 0 ? x.EnrolledChildren.AssignedRoom.RoomId == dto.RoomId : true)
                            .Where(x => dto.BranchOfficeId != 0 ? x.EnrolledChildren.AssignedRoom.BranchOfficeId == dto.BranchOfficeId : true)
                            .Where(x => dto.PaymentOperationId != 0 ? x.PaymentOperationId == dto.PaymentOperationId : true)
                            .OrderBy(x => x.DateCreation).ToList();

            return payments.Any() ? Result<List<PaymentResult>>.SetOk(mapper.Map<List<PaymentResult>>(payments))
                                    : Result<List<PaymentResult>>.SetError("No existen resultados en la busqueda");
        }

        public Result<List<PaymentResult>> GetAll()
        {
            var payments = ListComplete<Payment>().Include(x => x.EnrolledChildren.AssignedTutor).Include(x => x.EnrolledChildren.AssignedRoom).Include(x => x.EnrolledChildren.AssignedTutor.Kid)
                            .Include(x => x.EnrolledChildren.AssignedTutor.Parent).Include(x => x.EnrolledChildren.AssignedRoom.Collaborator)
                            .Include(x => x.EnrolledChildren.AssignedRoom.Room).Include(x => x.EnrolledChildren.AssignedRoom.Turn)
                            .Include(x => x.EnrolledChildren.AssignedRoom.Modality).Include(x => x.EnrolledChildren.AssignedRoom.BranchOffice)
                            .Include(x => x.PaymentOperation).Include(x => x.PaymentType).Include(x => x.AuditPaymentType)
                            .OrderBy(x => x.DateCreation).ToList();

            return payments.Any() ? Result<List<PaymentResult>>.SetOk(mapper.Map<List<PaymentResult>>(payments))
                                      : Result<List<PaymentResult>>.SetError("No existen resultados en la busqueda");
        }
        public Result<PaymentDetailResult> GetDetail(PaymentDetailDto dto)
        {
            var payment = Context.Payments.Where(x => x.Id == dto.Id).Include(x => x.EnrolledChildren)
                //Parent Data
                .Include(x => x.EnrolledChildren.AssignedTutor)
                .Include(x => x.EnrolledChildren.AssignedTutor.Parent)
                .Include(x => x.EnrolledChildren.AssignedTutor.Parent.MaritalStatus)
                .Include(x => x.EnrolledChildren.AssignedTutor.Parent.DocumentType)
                .Include(x => x.EnrolledChildren.AssignedTutor.Relationship)
                // Room Data
                .Include(x => x.EnrolledChildren.AssignedRoom)
                .Include(x => x.EnrolledChildren.AssignedRoom.Collaborator)
                .Include(x => x.EnrolledChildren.AssignedRoom.Room)
                .Include(x => x.EnrolledChildren.AssignedRoom.BranchOffice)
                .Include(x => x.EnrolledChildren.AssignedRoom.City)
                .Include(x => x.EnrolledChildren.AssignedRoom.Modality)
                .Include(x => x.EnrolledChildren.AssignedRoom.Turn)
                // Kid Data
                .Include(x => x.EnrolledChildren.AssignedTutor.Kid)
                .FirstOrDefault();

            return payment != null ? Result<PaymentDetailResult>.SetOk(mapper.Map<PaymentDetailResult>(payment)) :
               Result<PaymentDetailResult>.SetError("Pago no encontrado");
        }
        public Result<string> Create(CreatePaymentDto dto)
        {
            try
            {
                var paymentBill = List().Where(x => x.NumberBill == dto.NumberBill).FirstOrDefault();
                if(string.IsNullOrEmpty(dto.NumberBill) || paymentBill == null)
                {
                    Context.Save(mapper.Map<Payment>(dto));
                }
                else
                {
                    return Result<string>.SetError("Factura ya registrada, verifique porfavor");
                }
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("No se realizo la operacion");
            }
            return Result<string>.SetOk("Operacion Exitosa");
        }

        public Result<string> Update(UpdatePaymentDto dto)
        {
            var paymentBill = List().FirstOrDefault(x => x.NumberBill == dto.NumberBill);
            var row = GetById<Payment>(dto.Id);

            if (row != null)
            {
                if (string.IsNullOrEmpty(dto.NumberBill) || paymentBill == null || paymentBill.Id == row.Id)
                {
                    Context.Save(mapper.Map(dto, row));
                    return Result<string>.SetOk("Actualización Exitosa");
                }
                else
                {
                    return Result<string>.SetError("Factura ya registrada, verifique por favor");
                }
            }
            else
            {
                return Result<string>.SetError("No se encontró el registro de pago para actualizar");
            }
        }


        public Result<string> ActivateOrDeactivate(PaymentDetailDto dto)
        {
            var data = Get(dto.Id);
            if (data == null) return Result<string>.SetError("El registro no existe, revise porfavor");
            data.IsDeleted = dto.IsDeleted;
            Context.Save(data);
            return Result<string>.SetOk("Operacion Exitosa");
        }

        public Result<ReportResult> GeneratePdf(PaymentFilterDto dto, string title)
        {
            var list = ListComplete<Payment>().Include(x => x.EnrolledChildren.AssignedTutor).Include(x => x.EnrolledChildren.AssignedRoom).Include(x => x.EnrolledChildren.AssignedTutor.Kid)
                            .Include(x => x.EnrolledChildren.AssignedTutor.Parent).Include(x => x.EnrolledChildren.AssignedRoom.Collaborator)
                            .Include(x => x.EnrolledChildren.AssignedRoom.Room).Include(x => x.EnrolledChildren.AssignedRoom.Turn)
                            .Include(x => x.EnrolledChildren.AssignedRoom.Modality).Include(x => x.EnrolledChildren.AssignedRoom.BranchOffice)
                            .Include(x => x.PaymentOperation).Include(x => x.PaymentType).Include(x => x.AuditPaymentType)
                            .Where(x => dto.KidId != 0 ? x.EnrolledChildren.AssignedTutor.KidId == dto.KidId : true)
                            .Where(x => dto.RoomId != 0 ? x.EnrolledChildren.AssignedRoom.RoomId == dto.RoomId : true)
                            .Where(x => dto.BranchOfficeId != 0 ? x.EnrolledChildren.AssignedRoom.BranchOfficeId == dto.BranchOfficeId : true)
                            .Where(x => dto.PaymentOperationId != 0 ? x.PaymentOperationId == dto.PaymentOperationId : true)
                            .OrderBy(x => x.DateCreation).ToList();

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
            PdfPTable table = new PdfPTable(12);
            table.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase("Niño", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Padre", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
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

            cell = new PdfPCell(new Phrase("Sucursal", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Inicio Centro", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("NIT Padre", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Nro de Factura", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Monto", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Tipo de Pago", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Estado", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Detalle", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            // Contenido de las celdas
            Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            foreach (var row in list)
            {
                table.AddCell(new PdfPCell(new Phrase($"{row.EnrolledChildren.AssignedTutor.Kid.Name} {row.EnrolledChildren.AssignedTutor.Kid.FirstLastName} {row.EnrolledChildren.AssignedTutor.Kid.SecondLastName}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{row.EnrolledChildren.AssignedTutor.Parent.Name} {row.EnrolledChildren.AssignedTutor.Parent.FirstLastName} {row.EnrolledChildren.AssignedTutor.Parent.SecondLastName}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.EnrolledChildren.AssignedRoom.Room.Description, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.EnrolledChildren.AssignedRoom.Turn.Description, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.EnrolledChildren.AssignedRoom.BranchOffice.Description, cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{row.EnrolledChildren.AssignedTutor.Kid.StartDate.ToShortDateString()}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.EnrolledChildren.AssignedTutor.Parent.DocumentNumber, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.NumberBill, cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{row.Amount} Bs.", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.PaymentType.Description, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.PaymentOperation.Description, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.Description, cellFont)));
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
