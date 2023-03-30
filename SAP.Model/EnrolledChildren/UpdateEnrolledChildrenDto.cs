using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Model.EnrolledChildren
{
    public class UpdateEnrolledChildrenDto : CreateEnrolledChildrenDto
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
