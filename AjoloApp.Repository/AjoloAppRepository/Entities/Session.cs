using AjoloApp.Repository.AjoloAppRepository.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AjoloApp.Repository.AjoloAppRepository.Entities
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
