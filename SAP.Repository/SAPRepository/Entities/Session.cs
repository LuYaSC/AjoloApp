using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class Session : BaseData, IDateCreation
    {
        public string Name { get; set; }

        public string Password { get; set; }

        [MaxLength(50)]
        public string Action { get; set; }

        public DateTime DateCreation { get; set; }
    }
}
