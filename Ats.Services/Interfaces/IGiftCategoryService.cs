using System;
using System.Collections.Generic;
using System.Text;
using Ats.Models;
using Ats.Models.Gift;
using Ats.Models.Product;

namespace Ats.Services.Interfaces
{
    public interface IGiftCategoryService
    {
        List<GiftCategoryViewModel> Search(string searchText, string orderField, bool IsAscOrder, int page, int size, out int total);
        GiftCategoryViewModel Get(int id);
        void Create(GiftCategoryViewModel model);
        void Update(GiftCategoryViewModel model);
        void Delete(int id);
        List<GiftCategoryViewModel> Get();
    }
}
