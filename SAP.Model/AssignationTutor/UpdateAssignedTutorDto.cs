using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Model.AssignationTutor
{
    public class UpdateAssignedTutorDto : CreateAssignedTutorDto
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
