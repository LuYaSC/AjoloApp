using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Model.Parent
{
    public class UpdateParentDto : CreateParentDto
    {
        public int Id { get; set; }
    }
}
