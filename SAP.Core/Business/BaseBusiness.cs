namespace SAP.Core.Business
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using SAP.Repository.SAPRepository;
    using SAP.Repository.SAPRepository.Base;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;

    public abstract class BaseBusiness
    {
        public BaseBusiness(IConfiguration configuration, IPrincipal userInfo)
        {
            this.configuration = configuration;
        }

        protected IConfiguration configuration;
        protected IPrincipal UserInfo;
    }

    public abstract class BaseBusiness<T, CONTEXT> : BaseBusiness<T, int, CONTEXT>
        where T : class, IBase<int>
        where CONTEXT : SAPContext
    {
        public BaseBusiness(CONTEXT context, IPrincipal userInfo, IConfiguration configuration = null)
            : base(context, userInfo, configuration)
        { }
    }

    public abstract class BaseBusiness<T, TypeKey, CONTEXT> : BaseBusiness, IBaseBusiness<T, TypeKey, CONTEXT>
        where T : class, IBase<TypeKey>
        where TypeKey : IEquatable<TypeKey>, IConvertible
        where CONTEXT : SAPContext
    {
        public int userId;
        public bool seeIsDeleted = false;
        public List<string> roles;
        public BaseBusiness(CONTEXT context, IPrincipal userInfo, IConfiguration configuration = null)
            : base(configuration, userInfo)
        {
            this.Context = context;
            this.Context.userInfo = userInfo;
            this.UserInfo = userInfo;
            var claimsIdentity = (ClaimsIdentity) userInfo.Identity;
            userId = userInfo != null ? int.Parse(claimsIdentity.Claims.Where(x => x.Type == "identifier").FirstOrDefault()?.Value) : 0;
            roles = claimsIdentity.Claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Select(x => x.Value).ToList();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        public virtual T Get(TypeKey id)
        {
            var entity = this.Context.Set<T>().Find(id);
            return entity;
        }

        public virtual T GetById<T>(int id)
            where T : BaseTrace
        {
            var entity = this.Context.Set<T>().Include(x => x.UserCreated).Include(x => x.UserModificated).Where(x => x.Id == id).FirstOrDefault();
            return entity;
        }

        public virtual IQueryable<ENTITY> GetComplete<ENTITY>(string name, string firstLastName, string secondLastName, int sex)
           where ENTITY : BaseTrace, IName, IFirstLastName, ISecondLastName, ISexType
        {
            IQueryable<ENTITY> entity = this.Context.Set<ENTITY>().Include(x => x.UserCreated).Include(x => x.UserModificated);
            if (!string.IsNullOrEmpty(name)) entity = entity.Where(x => x.Name.Contains(name.ToUpper()));
            if (!string.IsNullOrEmpty(firstLastName)) entity = entity.Where(x => x.FirstLastName.Contains(firstLastName.ToUpper()));
            if (!string.IsNullOrEmpty(secondLastName)) 
                entity = entity.Where(x => x.SecondLastName != null ? x.SecondLastName.Contains(secondLastName.ToUpper()) : x.SecondLastName == string.Empty );
            if (sex != 0 ) entity = entity.Where(x => x.SexTypeId == sex);
            return entity;
        }

        public virtual IQueryable<T> List()
        {
            var isLogical = typeof(T).GetInterfaces().Contains(typeof(ILogicalDelete));
            var isAdmin = roles.Where(x => x == "ADMIN").SingleOrDefault();
            if (isLogical && isAdmin != null)
            {
                return this.Context.Set<T>();
            }
            else
            {
                var returnCol = this.Context.Set<T>().Where(x => seeIsDeleted || (x as BaseLogicalDelete<TypeKey>).IsDeleted == false);
                return returnCol;
            }
        }

        public virtual IQueryable<ENTITY> ListComplete<ENTITY>()
            where ENTITY : BaseTrace
        {
            var isLogical = typeof(T).GetInterfaces().Contains(typeof(ILogicalDelete));
            var isAdmin = roles.Where(x => x == "ADMIN").ToList();
            if (isLogical && isAdmin.Any())
            {
                return this.Context.Set<ENTITY>().Include(x => x.UserCreated).Include(x => x.UserModificated).OrderBy(x => x.DateCreation);
            }
            else
            {
                var returnCol = this.Context.Set<ENTITY>().Include(x => x.UserCreated)
                                           .Include(x => x.UserModificated).OrderBy(x => x.DateCreation).Where(x => seeIsDeleted || (x as BaseLogicalDelete<TypeKey>).IsDeleted == false);
                return returnCol;
            }
        }

        public string CalculateAge(DateTime bornDate)
        {
            TimeSpan dateDiff = DateTime.Now - bornDate;
            DateTime age = new DateTime(dateDiff.Ticks);

            var Years = age.Year - 1;
            var Month = age.Month - 1;
            var Days = age.Day - 1;

            return $"{Years} años{(Month > 0 ? $", {Month} meses" : "")} {(Days > 0 ? $"y {Days} dias" : "")}";
        }

        public virtual void Remove(TypeKey id)
        {
            var entity = this.Get(id);
            this.Context.Remove<T, TypeKey>(entity);
        }

        public virtual T Save(T entity)
        {
            this.Validate(entity);
            return this.Context.Save<T, TypeKey>(entity);
        }

        protected virtual void Validate(T entity)
        {

        }

        public virtual Result<string> WarmUp()
        {
            var data = Context.Set<T>().FirstOrDefault();
            return Result<string>.SetOk(string.Empty);
        }

        protected CONTEXT Context;
    }
}
