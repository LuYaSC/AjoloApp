using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Base;
using System;
using System.Linq;

namespace SAP.Core.Business
{
    public interface IBaseBusiness<T, TypeKey, CONTEXT>
       where T : IBase<TypeKey>
       where TypeKey : IEquatable<TypeKey>
       where CONTEXT : SAPContext
    {
        void Dispose();

        T Get(TypeKey id);

        IQueryable<T> List();

        void Remove(TypeKey id);

        T Save(T entity);

        Result<string> WarmUp();
    }
}
