using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using SAP.Core.Business;
using SAP.Model.Kid;
using SAP.Repository.SAPRepository;
using SAP.Repository.SAPRepository.Entities;

namespace SAP.RuleEngine.KidService
{
    public class KidService : BaseBusiness<Kid, SAPContext>, IKidService
    {
        IMapper mapper;

        public KidService(SAPContext context, IPrincipal userInfo) : base(context, userInfo)
        {
            //userId = int.Parse(claimsIdentity.Claims.Where(x => x.Type == "identifier").FirstOrDefault().Value);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Kid, KidsResult>();
                cfg.CreateMap<Kid, KidByIdResult>();
            });
            //var claimsIdentity = (ClaimsIdentity) userInfo.Identity;
            //userId = int.Parse(claimsIdentity.Claims.Where(x => x.Type == "identifier").FirstOrDefault().Value);
            //if (UserInfo.Identity.Name != null)
            //{
            //    companyId = UserInfo.Identity.GetCompanyId();
            //}
            mapper = new Mapper(config);
        }

        public Result<KidByIdResult> GetKidById(int kidId)
        {
            return Result<KidByIdResult>.SetOk(mapper.Map<KidByIdResult>(Get(kidId)));
        }

        public Result<List<KidsResult>> GetAllKids()
        {
            return Result<List<KidsResult>>.SetOk(mapper.Map<List<KidsResult>>(List()));
        }

    }
}
