using System;

namespace Ats.Domain.Address
{
    public interface IAddressRepository : IRepository<Models.Address, Guid>
    {
        Address.Models.Address CreateAddress(string address1, int? provinceId, int? districtId);
    }
}
