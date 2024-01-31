using AutoMapper;
using AjoloApp.Core.Business;
using AjoloApp.Model.EnrolledChildren;
using AjoloApp.Repository.AjoloAppRepository.Entities;
using AjoloApp.Repository.AjoloAppRepository;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using AjoloApp.Model.AssignationRoom;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using AjoloApp.Model.Kid;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace AjoloApp.RuleEngine.EnrolledChildrenService
{
    public class EnrolledChildrenService : BaseBusiness<EnrolledChildren, AjoloAppContext>, IEnrolledChildrenService
    {
        IMapper mapper;
        int quantityTutors = 0;

        public EnrolledChildrenService(AjoloAppContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EnrolledChildren, EnrolledChildrenResult>()
                   .ForMember(d => d.Parent, o => o.MapFrom(s => $"{s.AssignedTutor.Parent.Name} {s.AssignedTutor.Parent.FirstLastName} " +
                                                                 $"{s.AssignedTutor.Parent.SecondLastName}"))
                   .ForMember(d => d.Kid, o => o.MapFrom(s => $"{s.AssignedTutor.Kid.Name} {s.AssignedTutor.Kid.FirstLastName} " +
                                                              $"{s.AssignedTutor.Kid.SecondLastName}"))
                   .ForMember(d => d.Collaborator, o => o.MapFrom(s => $"{s.AssignedRoom.Collaborator.Name} {s.AssignedRoom.Collaborator.FirstLastName} " +
                                                                       $"{s.AssignedRoom.Collaborator.SecondLastName}"))
                   .ForMember(d => d.KidId, o => o.MapFrom(s => s.AssignedTutor.KidId))
                   .ForMember(d => d.Room, o => o.MapFrom(s => s.AssignedRoom.Room.Description))
                   .ForMember(d => d.Turn, o => o.MapFrom(s => s.AssignedRoom.Turn.Description))
                   .ForMember(d => d.Modality, o => o.MapFrom(s => s.AssignedRoom.Modality.Description))
                   .ForMember(d => d.BranchOffice, o => o.MapFrom(s => s.AssignedRoom.BranchOffice.Description))
                   .ForMember(d => d.UserCreation, o => o.MapFrom(s => CutUser(s.UserCreated.UserName)))
                   .ForMember(d => d.AssignedRoomId, o => o.MapFrom(s => s.AssignedRoomId))
                   .ForMember(d => d.UserModification, o => o.MapFrom(s => CutUser(s.UserModificated.UserName)));
                cfg.CreateMap<CreateEnrolledChildrenDto, EnrolledChildren>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<UpdateEnrolledChildrenDto, EnrolledChildren>().AfterMap<TrimAllStringProperty>();
                cfg.CreateMap<EnrolledChildren, EnrollChildrenDetailResult>()
                    //Kid Data
                    .ForMember(d => d.KidName, o => o.MapFrom(s => $"{s.AssignedTutor.Kid.Name} {s.AssignedTutor.Kid.FirstLastName} " +
                                                              $"{s.AssignedTutor.Kid.SecondLastName}"))
                    .ForMember(d => d.KidId, o => o.MapFrom(s => s.AssignedTutor.KidId))
                    .ForMember(d => d.KidId, o => o.MapFrom(s => s.AssignedRoomId))
                    .ForMember(d => d.BloodTypeKid, o => o.MapFrom(s => s.AssignedTutor.Kid.BloodType.Description))
                    .ForMember(d => d.SexKid, o => o.MapFrom(s => s.AssignedTutor.Kid.SexType.Description))
                    .ForMember(d => d.BornDateKid, o => o.MapFrom(s => s.AssignedTutor.Kid.BornDate))
                    .ForMember(d => d.AgeKid, o => o.MapFrom(s => CalculateAge(s.AssignedTutor.Kid.BornDate.Value)))
                    //Room Data
                    .ForMember(d => d.StartDateKid, o => o.MapFrom(s => s.AssignedTutor.Kid.StartDate))
                    .ForMember(d => d.Room, o => o.MapFrom(s => s.AssignedRoom.Room.Description))
                    .ForMember(d => d.City, o => o.MapFrom(s => s.AssignedRoom.City.Description))
                    .ForMember(d => d.Turn, o => o.MapFrom(s => s.AssignedRoom.Turn.Description))
                    .ForMember(d => d.Observations, o => o.MapFrom(s => s.AssignedRoom.Observations))
                    .ForMember(d => d.Modality, o => o.MapFrom(s => s.AssignedRoom.Modality.Description))
                    .ForMember(d => d.BranchOffice, o => o.MapFrom(s => s.AssignedRoom.BranchOffice.Description))
                    .ForMember(d => d.Collaborator, o => o.MapFrom(s => $"{s.AssignedRoom.Collaborator.Name} {s.AssignedRoom.Collaborator.FirstLastName} " +
                                                                       $"{s.AssignedRoom.Collaborator.SecondLastName}"));
                cfg.CreateMap<AssignedTutor, Parents>()
                    .ForMember(d => d.ParentName, o => o.MapFrom(s => $"{s.Parent.Name} {s.Parent.FirstLastName} {s.Parent.SecondLastName}"))
                    .ForMember(d => d.Relation, o => o.MapFrom(s => s.Relationship.Description))
                    .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.Parent.PhoneNumber))
                    .ForMember(d => d.BloodTypeParent, o => o.MapFrom(s => s.Parent.BloodType.Description))
                    .ForMember(d => d.SexParent, o => o.MapFrom(s => s.Parent.SexType.Description))
                    .ForMember(d => d.Address, o => o.MapFrom(s => s.Parent.Address))
                    .ForMember(d => d.MaritalStatus, o => o.MapFrom(s => s.Parent.MaritalStatus.Description))
                    .ForMember(d => d.IsAuthorized, o => o.MapFrom(s => s.IsAuthorized));


            });
            mapper = new Mapper(config);
        }

        public Result<List<EnrolledChildrenResult>> GetFilter(EnrollFilterDto dto)
        {
            var enrolleds = ListComplete<EnrolledChildren>()
                .Include(x => x.AssignedTutor).Include(x => x.AssignedRoom).Include(x => x.AssignedTutor.Kid)
                .Include(x => x.AssignedRoom.Room)
                .Where(x => dto.KidId != 0 ? x.AssignedTutor.KidId == dto.KidId : true)
                .Where(x => dto.RoomId != 0 ? x.AssignedRoom.RoomId == dto.RoomId : true)
                .OrderBy(x => x.DateCreation).ToList();
            return enrolleds.Any() ? Result<List<EnrolledChildrenResult>>.SetOk(mapper.Map<List<EnrolledChildrenResult>>(enrolleds))
                                    : Result<List<EnrolledChildrenResult>>.SetError("La Busqueda con los criterios seleccionados, no tiene Resultados, pruebe de nuevo porfavor");
        }

        public Result<List<EnrolledChildrenResult>> GetAll()
        {
            var assingations = ListComplete<EnrolledChildren>().Include(x => x.AssignedTutor).Include(x => x.AssignedRoom).Include(x => x.AssignedTutor.Kid)
                                                               .Include(x => x.AssignedTutor.Parent).Include(x => x.AssignedRoom.Collaborator)
                                                               .Include(x => x.AssignedRoom.Room).Include(x => x.AssignedRoom.Turn)
                                                               .Include(x => x.AssignedRoom.Modality).Include(x => x.AssignedRoom.BranchOffice).ToList();
            return assingations.Any() ? Result<List<EnrolledChildrenResult>>.SetOk(mapper.Map<List<EnrolledChildrenResult>>(assingations))
                                    : Result<List<EnrolledChildrenResult>>.SetError("La Busqueda con los criterios seleccionados, no tiene Resultados, pruebe de nuevo porfavor");
        }

        public Result<EnrollChildrenDetailResult> GetDetail(UpdateAssignedRoomDto dto)
        {
            var data = Context.EnrolledChildrens.Where(x => x.Id == dto.Id).Include(x => x.AssignedTutor).Include(x => x.AssignedRoom)
                .Include(x => x.AssignedTutor.Kid).Include(x => x.AssignedTutor.Kid.BloodType).Include(x => x.AssignedTutor.Kid.SexType)
                .Include(x => x.AssignedRoom.Turn).Include(x => x.AssignedRoom.City).Include(x => x.AssignedRoom.BranchOffice)
                .Include(x => x.AssignedRoom.Room).Include(x => x.AssignedRoom.Collaborator).Include(x => x.AssignedRoom.Modality).SingleOrDefault();
            var parents = Context.AssignedTutors.Where(x => x.KidId == data.AssignedTutor.KidId).Include(x => x.Parent).Include(x => x.Parent.BloodType)
                .Include(x => x.Parent.SexType).Include(x => x.Relationship).Include(x => x.Parent.MaritalStatus).ToList();
            var result = mapper.Map<EnrollChildrenDetailResult>(data);
            result.Parents = mapper.Map<List<Parents>>(parents);
            quantityTutors = parents.Count;
            return Result<EnrollChildrenDetailResult>.SetOk(result);
        }

        public Result<string> Create(CreateEnrolledChildrenDto dto)
        {
            try
            {
                var tutors = Context.AssignedTutors.Where(x => x.KidId == dto.KidId).FirstOrDefault();
                if (tutors == null)
                {
                    return Result<string>.SetError("El menor seleccionado no tiene tutores asignados");
                }
                dto.AssignedTutorId = tutors.Id;
                var ifExists = Context.EnrolledChildrens.Where(x => x.AssignedTutorId == dto.AssignedTutorId && x.AssignedRoomId == dto.AssignedRoomId)
                    .FirstOrDefault();
                if (ifExists != null)
                {
                    return Result<string>.SetError("El menor ya fue registrado en esa sala");
                }
                var enrollData = Context.Save(mapper.Map<EnrolledChildren>(dto));
                if (dto.GeneratePayments && !Context.Payments.Where(x => x.EnrolledChildrenId == enrollData.Id).Any())
                {
                    if (enrollData == null)
                    {
                        return Result<string>.SetError("La operacion no se realizo correctamente, verifique y vuelva a intertarlo");
                    }
                    GeneratePayments(enrollData, dto);
                }
            }
            catch (Exception ex)
            {
                return Result<string>.SetError("Not Saved");
            }
            return Result<string>.SetOk("Saved with success");
        }

        public Result<string> Update(UpdateEnrolledChildrenDto dto)
        {

            var row = GetById<EnrolledChildren>(dto.Id);
            if (row != null)
            {
                dto.AssignedTutorId = row.AssignedTutorId;
                Context.Save(mapper.Map(dto, row));
                return Result<string>.SetOk("Operacion Exitosa");
            }
            else
            { return Result<string>.SetError("No se pudo realizar la operacion"); }
        }

        public Result<string> ActivateOrDeactivate(UpdateEnrolledChildrenDto dto)
        {
            var data = Get(dto.Id);
            if (data == null) return Result<string>.SetError("El registro no existe, revise porfavor");
            data.IsDeleted = dto.IsDeleted;
            Context.Save(data);
            return Result<string>.SetOk("Operacion Exitosa");
        }
        public Result<ReportResult> GeneratePdf(string title)
        {
            var list = ListComplete<EnrolledChildren>().Include(x => x.AssignedTutor).Include(x => x.AssignedRoom).Include(x => x.AssignedTutor.Kid)
                        .Include(x => x.AssignedTutor.Parent).Include(x => x.AssignedRoom.Collaborator)
                        .Include(x => x.AssignedRoom.Room).Include(x => x.AssignedRoom.Turn)
                        .Include(x => x.AssignedRoom.Modality).Include(x => x.AssignedRoom.BranchOffice)
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
            PdfPTable table = new PdfPTable(11);
            table.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase("Educadora", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Padre", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Telefono Tutor", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Niño", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Fecha de N. Niño", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Edad Niño", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Fecha de I. Centro", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
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

            cell = new PdfPCell(new Phrase("Antiguedad", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(255, 89, 114); // Color rosa
            table.AddCell(cell);

            // Contenido de las celdas
            Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            foreach (var row in list)
            {
                table.AddCell(new PdfPCell(new Phrase($"{row.AssignedRoom.Collaborator.Name} {row.AssignedRoom.Collaborator.FirstLastName} {row.AssignedRoom.Collaborator.SecondLastName}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{row.AssignedTutor.Parent.Name} {row.AssignedTutor.Parent.FirstLastName} {row.AssignedTutor.Parent.SecondLastName}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.AssignedTutor.Parent.PhoneNumber, cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{row.AssignedTutor.Kid.Name} {row.AssignedTutor.Kid.FirstLastName} {row.AssignedTutor.Kid.SecondLastName}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{row.AssignedTutor.Kid.BornDate.Value.ToShortDateString()}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(CalculateAge(row.AssignedTutor.Kid.BornDate.Value), cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.AssignedTutor.Kid.StartDate.ToShortDateString(), cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{row.AssignedRoom.Room.Description}", cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.AssignedRoom.Turn.Description, cellFont)));
                table.AddCell(new PdfPCell(new Phrase(row.AssignedRoom.BranchOffice.Description, cellFont)));
                table.AddCell(new PdfPCell(new Phrase($"{CalculateAge(row.AssignedTutor.Kid.StartDate)}", cellFont)));
            }

            document.Add(table);

            // Cerrar el documento
            document.Close();
            var result = new ReportResult { ReportName = $"{title}-{DateTime.Now}", Report = stream.ToArray() };

            // Devolver el PDF generado como un array de bytes
            return result.Report.Length > 0 ? Result<ReportResult>.SetOk(result)
                                            : Result<ReportResult>.SetError("No se pudo generar el reporte");

        }



        private void GeneratePayments(EnrolledChildren enrollData, CreateEnrolledChildrenDto dto)
        {

            Context.Save(new Payment
            {
                Amount = dto.Amount,
                EnrolledChildrenId = enrollData.Id,
                Description = "Primer pago de Inscripcion",
                IsVerified = true,
                NumberBill = string.Empty,
                DateToPay = DateTime.UtcNow,
                AuditPaymentId = Context.AuditPaymentTypes.Where(x => x.Description.Contains("FACTURA VENTA")).FirstOrDefault().Id,
                PaymentTypeId = Context.PaymentTypes.Where(x => x.Description.Contains("EFECTIVO")).FirstOrDefault().Id,
                PaymentOperationId = Context.PaymentOperations.Where(x => x.Description.Contains("PAGADO")).FirstOrDefault().Id,
                Observations = "Pago Generado Existosamente",
            });
        }
    }
}
