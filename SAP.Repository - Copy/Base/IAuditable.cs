using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SAP.Repository.SAPRepository.Base
{
    public interface IUserCreation<TypeKey>
    {
        [Required]
        [MaxLength(6)]
        TypeKey UserCreation { get; set; }
    }

    public interface IUserModification<TypeKey>
    {
        [MaxLength(6)]
        TypeKey UserModification { get; set; }
    }

    public interface IDateCreation
    {
        [Required]
        DateTime DateCreation { get; set; }
    }

    public interface IDateModification
    {
        DateTime? DateModification { get; set; }
    }

    public interface IAuditable<TypeKeyUserCreation, TypeKeyUserModification> : IUserCreation<TypeKeyUserCreation>, IUserModification<TypeKeyUserModification>, IDateCreation, IDateModification
    { }

    public interface IBackOfficeAuditable : IUserCreation<string>, IUserModification<string>, IDateCreation, IDateModification
    {
    }
}
