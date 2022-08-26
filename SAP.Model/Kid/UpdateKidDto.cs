using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.Kid
{
    public class UpdateKidDto : CreateKidDto
    {
        public int Id { get; set; }
    }
}
