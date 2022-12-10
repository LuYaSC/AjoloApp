using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAP.Repository.SAPRepository.Base;
using SAP.Repository.SAPRepository.Entities;
using System.Security.Claims;
using System.Security.Principal;

namespace SAP.Repository.SAPRepository
{
    public class SAPContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public SAPContext(DbContextOptions<SAPContext> options) : base(options)
        {
        }

        public DbSet<Collaborator> Collaborators { get; set; }
        public DbSet<Modality> Modalities { get; set; }
        public DbSet<Turn> Turns { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Kid> Kids { get; set; }
        public DbSet<KidBackground> KidBackground { get; set; }
        public DbSet<KidBirthBackground> KidBirthBackground { get; set; }
        public DbSet<KidConditionNewBorn> KidConditionNewBorn { get; set; }
        public DbSet<KidDreamBackground> KidDreamBackground { get; set; }
        public DbSet<KidFoodBackground> KidFoodBackground { get; set; }
        public DbSet<KidLanguageBackground> KidLanguageBackground { get; set; }
        public DbSet<KidPsychomotorBackgroud> KidPsychomotorBackgroud { get; set; }
        public DbSet<KidRelationBackground> KidRelationBackground { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<AssignedRoom> AssignedRooms { get; set; }
        public DbSet<AssignedTutor> AssignedTutors { get; set; }
        public DbSet<EnrolledChildren> EnrolledChildrens { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<AuditPaymentType> AuditPaymentTypes { get; set; }
        public DbSet<PaymentOperation> PaymentOperations { get; set; }
        public DbSet<BranchOffice> BranchOffices { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<SexType> SexTypes { get; set; }
        public DbSet<BloodType> BloodTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public IPrincipal userInfo { get; set; }

        public void Detach<T>(T entity) where T : class
        {
            Entry(entity).State = EntityState.Detached;
        }

        //public void ExecuteCommand(string sql, params object[] parameters)
        //{
        //    Database.ExecuteSqlCommand(sql, parameters);
        //}

        //public T ExecuteScalar<T>(string sql, params object[] parameters)
        //{
        //    return Database.SqlQuery<T>(sql, parameters).Single();
        //}

        //public T ExecuteScalar<T>(string sql, int timeOut, params object[] parameters)
        //{
        //    Database.SetCommandTimeout(timeOut);
        //    try
        //    {
        //        return Database.ex <T>(sql, parameters).Single();
        //    }
        //    finally
        //    {
        //        Database.Connection.Close();
        //    }

        //}

        //public List<T> ExecuteScalarList<T>(string sql, int timeOut, params object[] parameters)
        //{
        //    Database.CommandTimeout = timeOut;
        //    try
        //    {
        //        return Database.SqlQuery<T>(sql, parameters).ToList();
        //    }
        //    finally
        //    {
        //        Database.Connection.Close();
        //    }

        //}

        //public List<T> SqlQuery<T>(string sql, params object[] parameters)
        //{
        //    return Database.sq.SqlQuery<T>(sql, parameters).ToList();
        //}

        public void Remove<T>(T entity)
            where T : class, IBase<int>
        {
            Remove<T, int>(entity);
        }

        public void RemoveRange<T>(List<T> entities)
          where T : class, IBase<int>
        {
            foreach (var entity in entities)
            {
                Remove<T, int>(entity);
            }
        }

        public void RemoveData<T>(List<T> entities)
          where T : class, IBase<int>
        {
            foreach (var entity in entities)
            {
                Entry(entity).State = EntityState.Deleted;
            }
        }

        public void Remove<T, TypeKey>(T entity)
            where T : class, IBase<TypeKey>
            where TypeKey : IEquatable<TypeKey>, IConvertible
        {
            if (entity == null)
            {
                return;
            }
            if (entity is ILogicalDelete entityLogicalDelete)
            {
                entityLogicalDelete.IsDeleted = true;
                Save<T, TypeKey>((T)entityLogicalDelete);

                return;
            }
            Entry(entity).State = EntityState.Deleted;
            SaveChanges();
        }

        public T Save<T>(T entity)
            where T : class, IBase<int>
        {
            return Save<T, int>(entity);
        }

        public T Save<T, TypeKey>(T entity)
            where T : class, IBase<TypeKey>
            where TypeKey : IEquatable<TypeKey>, IConvertible
        {
            SetEntity<T, TypeKey>(entity);
            SaveChanges();
            return entity;
        }

        public void SetEntity<TEntity>(TEntity entity)
            where TEntity : class, IBase<TEntity>
        {
            SetEntity(entity);
        }

        public void SetEntity<TEntity, TypeKey>(TEntity entity)
            where TEntity : class, IBase<TypeKey>
            where TypeKey : IEquatable<TypeKey>, IConvertible
        {
            var claimsIdentity = userInfo != null ? (ClaimsIdentity)this.userInfo.Identity : null;
            if (entity == null)
            {
                return;
            }
            if (entity.Id.Equals(0))
            {
                Entry(entity).State = EntityState.Added;
                if (entity is IDateCreation)
                {
                    (entity as IDateCreation).DateCreation = DateTime.UtcNow;
                }
                if (entity is IDateModification)
                {
                    (entity as IDateModification).DateModification = DateTime.UtcNow;
                }
                if (entity is IUserCreation<int>)
                {
                    (entity as IUserCreation<int>).UserCreation = claimsIdentity != null ? 
                        int.Parse(claimsIdentity.Claims.Where(x => x.Type == "identifier").FirstOrDefault().Value) : 9;
                }
                if (entity is IUserModification<int>)
                {
                    (entity as IUserModification<int>).UserModification = claimsIdentity != null ? 
                        int.Parse(claimsIdentity.Claims.Where(x => x.Type == "identifier").FirstOrDefault().Value) : 9;
                }
            }
            else
            {
                UpdateEntity<TEntity, TypeKey>(entity);
            }
        }

        private void UpdateEntity<TEntity, TypeKey>(TEntity entity)
           where TEntity : class, IBase<TypeKey>
           where TypeKey : IEquatable<TypeKey>, IConvertible
        {
            var claimsIdentity = userInfo != null ? (ClaimsIdentity)this.userInfo.Identity : null;
            Entry(entity).State = EntityState.Modified;
            if (entity is IDateModification)
            {
                (entity as IDateModification).DateModification = DateTime.UtcNow;
            }
            if (entity is IUserModification<int>)
            {
                (entity as IUserModification<int>).UserModification = claimsIdentity != null ?
                       int.Parse(claimsIdentity.Claims.Where(x => x.Type == "identifier").FirstOrDefault().Value) : 9;
            }
        }
    }
}
