using SAP.Repository.SAPRepository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Entities
{
    public class KidBackgroundBase : BaseTrace
    {
        public int KidId { get; set; }

        [ForeignKey("KidId")]
        public Kid Kid { get; set; }

    }
}
