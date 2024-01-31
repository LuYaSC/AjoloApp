using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AjoloApp.Model.TypeBusiness
{
    public class GetTypeByIdDto
    {
        public GetTypeByIdDto()
        {
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public string Initial { get; set; }

        public bool IsDisabled { get; set; }
    }
}
