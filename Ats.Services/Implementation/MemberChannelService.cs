using Ats.Domain;
using Ats.Domain.Account.Models;
using Ats.Domain.Channel.Models;
using Ats.Domain.Loan.Models;
using Ats.Domain.Store.Models;
using Ats.Models;
using Ats.Models.Channel;
using Ats.Models.Product;
using Ats.Services.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ats.Commons.Constants;
using Ats.Services.Interfaces;

namespace Ats.Services.Implementation
{
    public class MemberChannelService : BaseService, IMemberChannelService
    {
        private IConfiguration _config;
        private int pageSize;

        public MemberChannelService(IUnitOfWork unitOfWork, IConfiguration config, ILoggerManager logger) : base(unitOfWork, logger, config)
        {
            _config = config;
            pageSize = _config.GetValue<int>("PageSize");
        }

        public List<ChannelViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total)
        {
            _logger.LogDebug($"Search={searchText}, Page={page}");
            if (!String.IsNullOrEmpty(searchText)) searchText = searchText.Trim();
            var query = UnitOfWork.MemberChannelRepo.GetAll().Where(x => string.IsNullOrEmpty(searchText) ||
                                x.ChannelName != null && x.ChannelName.ToLower().Contains(searchText.ToLower()));
            total = query.Count();
            if (!String.IsNullOrEmpty(orderField))
            {
                switch (orderField.ToLower())
                {
                    case "id":
                        query = IsAscOrder ? query.OrderBy(x => x.Id) : query.OrderByDescending(x => x.Id);
                        break;
                    case "isactive":
                        query = IsAscOrder ? query.OrderBy(x => x.Active) : query.OrderByDescending(x => x.Active);
                        break;
                    case "name":
                        query = IsAscOrder ? query.OrderBy(x => x.ChannelName) : query.OrderByDescending(x => x.ChannelName);
                        break;
                }
            }

            var data = query.Select(x => new ChannelViewModel()
            {
                Id = x.Id,
                ChannelName = x.ChannelName,
                Desc = x.Desc,
                Active = x.Active
            }).Skip((page - 1) * size).Take(size).ToList();

            return data;
        }

        public List<ChannelViewModel> GetChannels()
        {
            _logger.LogDebug($"Get all channel category");
            var channels = UnitOfWork.MemberChannelRepo.GetAll().Where(x => x.Active).Select(x => new ChannelViewModel()
            {
                Id = x.Id,
                ChannelName = x.ChannelName
            }).OrderBy(x => x.ChannelName).ToList();

            return channels;
        }

        public ChannelViewModel GetChannel(int id)
        {
            _logger.LogDebug($"Channel Detail service (Id: {id})");
            var entity = UnitOfWork.MemberChannelRepo.GetById(id);
            if (entity != null)
            {
                var model = new ChannelViewModel()
                {
                    Id = entity.Id,
                    ChannelName = entity.ChannelName,
                    Desc = entity.Desc,
                    Active = entity.Active
                };

                return model;
            }
            return null;
        }
        public void CreateChannel(ChannelViewModel model)
        {
            _logger.LogDebug($"Create Channel (Name: {model.ChannelName})");
            var entity = new MemberChannel
            {
                Id = model.Id,
                ChannelName = model.ChannelName,
                Desc = model.Desc,
                Active = model.Active
            };

            UnitOfWork.MemberChannelRepo.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateChannel(ChannelViewModel model)
        {
            _logger.LogDebug($"Edit Channel (Id: {model.Id})");
            var entity = UnitOfWork.MemberChannelRepo.GetById(model.Id);
            if (entity != null)
            {
                entity.ChannelName = model.ChannelName;
                entity.Desc = model.Desc;
                entity.Active = model.Active;
                UnitOfWork.MemberChannelRepo.Update(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteChannel(int id)
        {
            _logger.LogDebug($"Delete Channel (Id: {id})");
            var entity = UnitOfWork.MemberChannelRepo.GetById(id);
            if (entity != null)
            {
                UnitOfWork.MemberChannelRepo.Delete(entity);
                UnitOfWork.SaveChanges();
            }
        }
        public List<MemberChannel> GetChannelCategories()
        {
            _logger.LogDebug($"Get Voucher Categories Enter.");
            var channelCategories = UnitOfWork.MemberChannelRepo.GetAll().Where(x => x.Active).Select(x => new MemberChannel()
            {
                Id = x.Id,
                ChannelName = x.ChannelName
            }).OrderBy(x => x.ChannelName).ToList();

            return channelCategories;
        }
    }
}
