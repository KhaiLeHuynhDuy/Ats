using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Channel.Models;
using Ats.Models;
using Ats.Models.Channel;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface IMemberChannelService
    {
        List<ChannelViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        ChannelViewModel GetChannel(int id);
        void CreateChannel(ChannelViewModel model);
        void UpdateChannel(ChannelViewModel model);
        void DeleteChannel(int id);
        List<ChannelViewModel> GetChannels();
        List<MemberChannel> GetChannelCategories();
    }
}
