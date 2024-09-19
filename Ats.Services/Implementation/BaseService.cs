using Ats.Domain;
using Ats.Commons;
using System;
using Microsoft.AspNetCore.Identity;
using Ats.Domain.Identity.Models;
using Ats.Services.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public abstract class BaseService : DisposableObject, IBaseService
    {
        //public ILog Logger { get; set; }
        protected UserManager<User> _userManager;
        protected SignInManager<User> _signInManager;
        protected ILoggerManager _logger;
        protected IConfiguration _configuration;
        protected IHttpContextAccessor _httpContextAccessor;
        protected IUnitOfWork UnitOfWork { get; set; }

        private BaseService()
        {

        }

        protected BaseService(IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork; 
        }
        protected BaseService(IUnitOfWork unitOfWork, ILoggerManager logger, IHttpContextAccessor httpContextAccessor)
        {
            this.UnitOfWork = unitOfWork;
            this._logger = logger;
            this._httpContextAccessor = httpContextAccessor;
        }
        protected BaseService(IUnitOfWork unitOfWork, ILoggerManager logger, IConfiguration configuration)
        {
            UnitOfWork = unitOfWork;
            this._logger = logger;
            this._configuration = configuration;
        }

        #region Dispose
        private bool _disposed;

        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    UnitOfWork = null;
                }
                _disposed = true;
            }            
        }
        #endregion
    }
}
