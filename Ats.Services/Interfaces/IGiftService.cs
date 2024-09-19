using System;
using System.Collections.Generic;
using System.Text;
using Ats.Domain.Channel.Models;
using Ats.Domain.Gift.Models;
using Ats.Models;
using Ats.Models.Gift;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface IGiftService
    {     
        List<GiftViewModel> Search(string searchText, int? giftcategoryId, bool? giftType, int? channelId,
             int? stockFrom, int? stockTo,string orderField, bool IsAscOrder, int page, int size, out int total);
        GiftViewModel Get(Guid id);
        void Create(GiftViewModel model);
        void Update(GiftViewModel model);
        void Delete(Guid id);
        void Send(GiftViewModel model);
        List<GiftViewModel> Gets();
        List<GiftCategory> GetGiftCategories();
        List<MemberChannel> GetChannel();
    }
}
