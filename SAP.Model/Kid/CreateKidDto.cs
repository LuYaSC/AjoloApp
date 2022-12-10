using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.Kid
{
    public class CreateKidDto
    {
        public string Name { get; set; }

        public string FirstLastName { get; set; }

        public string? SecondLastName { get; set; }

        public int SexTypeId { get; set; }

        public DateTime? BornDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string PlaceBorn { get; set; }

        public int BloodTypeId { get; set; }

        public string DocumentNumber { get; set; }

        public int DocumentTypeId { get; set; }       
    }
}
