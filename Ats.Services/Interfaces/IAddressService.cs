using Ats.Domain.Address;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Ats.Services.Interfaces
{
    public interface IAddressService : IBaseService
    {
        IEnumerable<District> GetAllDistrict();
        IEnumerable<Province> GetAllProvince();
        IEnumerable<Province> GetAllProvinces();
        IEnumerable<Country> GetAllCountry();
        IEnumerable<District> GetDistrictByProvinceId(int provinceId);
        SelectList GetCountrySelectList();
        SelectList GetProvinceSelectList();
        SelectList GetDistrictSelectList();
        string GetProvinceName(int id);
    }
}