using Ats.Domain;
using Ats.Domain.Identity.Models;
using Ats.Domain.Lead.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Organization.Models;
using Ats.Domain.Store.Models;
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
    public class CommonServiceProduct : BaseService, ICommonServiceProduct
    {
        private IConfiguration _config;
        private int pageSize;

        public CommonServiceProduct(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }
        
        public List<ProductCategory> GetProductCategories()
        {
            _logger.LogDebug($"Get Product Categories Enter.");
            var productCategories = UnitOfWork.ProductCategoryRepo.GetAll().Where(x => x.IsActive).Select(x => new ProductCategory()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return productCategories;
        }
        public List<ProductProperty> GetProductProperties()
        {
            _logger.LogDebug($"Get Product Properties Enter.");
            var productProperties = UnitOfWork.ProductPropertyRepo.GetAll().Where(x => x.IsActive).Select(x => new ProductProperty()
            {
                Id = x.Id,
                Name = x.Name
            }).OrderBy(x => x.Name).ToList();

            return productProperties;
        }
       
    }
}
