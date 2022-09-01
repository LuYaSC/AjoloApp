using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.Collaborator
{
    public class UpdateCollaboratorDto : CreateCollaboratorDto
    {
        public int Id { get; set; }
    }
}
