using Ats.Data.EntityFramework;
using Ats.Data.Repositories;
using Ats.Domain.Address;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;

namespace Ats.Domain.Address
{
    public class AddressRepository : Repository<Ats.Domain.Address.Models.Address, Guid>, IAddressRepository
    {
        public AddressRepository(SCDataContext context) : base(context)
        {
        }

        public Models.Address CreateAddress(string address1, int? provinceId, int? districtId)
        {
            Ats.Domain.Address.Models.Address add = new Models.Address()
            {
                Id = Guid.NewGuid(),
                Address1 = address1,
                ProvinceId = provinceId,
                DistrictId = districtId,           
            };

            this.Insert(add);

            return add;
        }
    }
}
