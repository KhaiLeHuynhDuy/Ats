using Ats.Domain;
using Ats.Domain.Identity.Models;
using Ats.Domain.Lead.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Organization.Models;
using Ats.Models.OriginalFile;
using Ats.Services.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class CommonService : BaseService, ICommonService
    {
        private IConfiguration _config;
        private int pageSize;

        public CommonService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        //public List<IncomeAmount> GetIncomeAmounts()
        //{
        //    _logger.LogDebug($"Get Income Amount Enter.");
        //    var incomeAmounts = UnitOfWork.IncomeAmountRepo.GetAll().Where(x => x.IsActive).Select(x => new IncomeAmount()
        //    {
        //        Id = x.Id,
        //        Name = x.Name
        //    }).OrderBy(x => x.Id).ToList();

        //    return incomeAmounts;
        //}

        //public List<IncomeStream> GetIncomeStreams()
        //{
        //    _logger.LogDebug($"Get Income Stream Enter.");
        //    var incomeStreams = UnitOfWork.IncomeStreamRepo.GetAll().Where(x => x.IsActive).Select(x => new IncomeStream()
        //    {
        //        Id = x.Id,
        //        Name = x.Name
        //    }).OrderBy(x => x.Id).ToList();

        //    return incomeStreams;
        //}

        public List<OriginalFileAddition> GetOriginalFileAdditions()
        {
            _logger.LogDebug($"Get Original File Addition Enter.");
            var originalFileAdditions = UnitOfWork.OriginalFileAdditionRepo.GetAll().Where(x => x.IsActive).Select(x => new OriginalFileAddition()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Id).ToList();

            return originalFileAdditions;
        }

        public List<OriginalFile> GetOriginalFiles()
        {
            _logger.LogDebug($"Get Original File Enter.");
            var originalFiles = UnitOfWork.OriginalFileRepo.GetAll().Where(x => x.IsActive).Select(x => new OriginalFile()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Id).ToList();

            return originalFiles;
        }
        public List<Organization> GetOrganizations()
        {
            _logger.LogDebug($"Get Organizations Enter.");
            var organizations = UnitOfWork.OrganizationRepo.GetAll().Where(x => x.IsActive).Select(x => new Organization()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return organizations;
        }
        public List<LoanProductCategory> GetProductCategories()
        {
            _logger.LogDebug($"Get Product Categories Enter.");
            var productCategories = UnitOfWork.LoanProductCategoryRepo.GetAll().Where(x => x.IsActive).Select(x => new LoanProductCategory()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return productCategories;
        }
        public List<LoanProductProperty> GetProductProperties()
        {
            _logger.LogDebug($"Get Product Properties Enter.");
            var productProperties = UnitOfWork.LoanProductPropertyRepo.GetAll().Where(x => x.IsActive).Select(x => new LoanProductProperty()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return productProperties;
        }
        public List<AssetType> GetAssetTypes()
        {
            _logger.LogDebug($"Get Asset Types Enter.");
            var assetTypes = UnitOfWork.AssetTypeRepo.GetAll().Where(x => x.IsActive).Select(x => new AssetType()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return assetTypes;
        }
        public List<AssetProperty> GetAssetProperties()
        {
            _logger.LogDebug($"Get Asset Properties Enter.");
            var assetProperties = UnitOfWork.AssetPropertyRepo.GetAll().Where(x => x.IsActive).Select(x => new AssetProperty()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return assetProperties;
        }
        public List<Warehouse> GetWarehouses()
        {
            _logger.LogDebug($"Get Warehouses Enter.");
            var warehouses = UnitOfWork.WarehouseRepo.GetAll().Where(x => x.IsActive).Select(x => new Warehouse()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return warehouses;
        }
        public List<Loan> GetLoans()
        {
            _logger.LogDebug($"Get Loans Enter.");
            var loans = UnitOfWork.LoanRepo.GetAll().Select(x => new Loan()
            {
                Id = x.Id,
                Amount = x.Amount
            }).OrderBy(x => x.Amount).ToList();

            return loans;
        }
        public List<Group> GetRoleGroups()
        {
            _logger.LogDebug($"Get Role groups Enter.");
            var roleGroups = UnitOfWork.GroupRepo.GetAll().Select(x => new Group()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Id).ToList();

            return roleGroups;
        }
    }
}
