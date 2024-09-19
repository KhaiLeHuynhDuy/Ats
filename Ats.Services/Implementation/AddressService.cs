using Ats.Domain;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Ats.Domain.Address;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Ats.Commons.Constants;
using Ats.Services.Extensions;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class AddressService : BaseService, IAddressService
    {
        private IConfiguration _config;
        public AddressService(IUnitOfWork unitOfWork, ILoggerManager logger,
            IConfiguration config) : base(unitOfWork, logger, config)
        {
            _config = config;
        }
        public IEnumerable<District> GetAllDistrict()
        {
            return this.UnitOfWork.DistrictRepo.GetAll().OrderBy(p => p.Name);
        }
        public IEnumerable<Province> GetAllProvince()
        {
            return this.UnitOfWork.ProvinceRepo.GetAll().OrderBy(p => p.Name);
        }

        public IEnumerable<Province> GetAllProvinces()
        {
            Province _province = new Province();
            _province.Id = 0;
            _province.Name = "--------- Khu vực ---------";
            _province.ProvinceCode = "0";
            List<Province> _lstProvince = this.UnitOfWork.ProvinceRepo.GetAll().OrderBy(p => p.Name).ToList();
            return _lstProvince.Append(_province);
        }
        public string GetProvinceName(int id)
        {
            _logger.LogDebug($"Get province name service (Id: {id})");
            var province = UnitOfWork.ProvinceRepo.GetById(id);
            if (province != null) return province.Name;
            return string.Empty;            
        }

        public IEnumerable<Country> GetAllCountry()
        {
            return this.UnitOfWork.CountryRepo.GetAll().OrderBy(p => p.Name);
        }

        public IEnumerable<District> GetDistrictByProvinceId(int provinceId)
        {
            var dis = this.UnitOfWork.ProvinceRepo.GetById(provinceId);
            return this.UnitOfWork.DistrictRepo.GetAll().Where(x => x.ProvinceCode.Equals(dis.ProvinceCode)).ToList();
        }
        public SelectList GetCountrySelectList()
        {
            _logger.LogDebug($"Get country select list");
            var countries = UnitOfWork.CountryRepo.GetAll().OrderBy(x => x.Name).ToList();
            var data = countries.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            var selectList = new SelectList(data, "Value", "Text");
            return selectList;
        }
        public SelectList GetProvinceSelectList()
        {
            _logger.LogDebug($"Get province select list");
            var provinces = UnitOfWork.ProvinceRepo.GetAll().OrderBy(x => x.Name).ToList();
            var data = provinces.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            var selectList = new SelectList(data, "Value", "Text");
            return selectList;
        }
        public SelectList GetDistrictSelectList()
        {
            _logger.LogDebug($"Get district select list");
            var districts = UnitOfWork.DistrictRepo.GetAll().OrderBy(x => x.Name).ToList();
            var data = districts.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            var selectList = new SelectList(data, "Value", "Text");
            return selectList;
        }
    }
}