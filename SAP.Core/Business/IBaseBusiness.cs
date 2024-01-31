using AjoloApp.Repository.AjoloAppRepository;
using AjoloApp.Repository.AjoloAppRepository.Base;
using System;
using System.Linq;

namespace AjoloApp.Core.Business
{
    public interface IBaseBusiness<T, TypeKey, CONTEXT>
       where T : IBase<TypeKey>
       where TypeKey : IEquatable<TypeKey>
       where CONTEXT : AjoloAppContext
    {
        void Dispose();

        T Get(TypeKey id);

        IQueryable<T> List();

        void Remove(TypeKey id);

        T Save(T entity);

        Result<string> WarmUp();
    }
}
